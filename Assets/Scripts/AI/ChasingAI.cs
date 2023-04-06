using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingAI : MonoBehaviour
{
    [SerializeField] LayerMask movementBlockLayer;
    [SerializeField] LayerMask wallMask;
    [SerializeField] GameObject chased;
    [SerializeField] Vector3 chasedLastKnownPosition;
    [SerializeField] Vector3 chasedBeforeLastKnownPosition;

    [SerializeField] bool chasing = false;

    [SerializeField] List<Vector3> availableDirections = new List<Vector3>();

    private void OnEnable()
    {
        PlayerMovement.OnMoved += RunAI;
    }

    private void OnDisable()
    {
        PlayerMovement.OnMoved -= RunAI;
    }

    void RunAI()
    {

        //Shoot raycast from chaser to chased and if it collides with any walls the chased is not visible setting chasing to false
        //If the raycast finds the player start chasing
        if (Physics.Raycast(transform.position, (chased.transform.position - transform.position).normalized, Vector3.Distance(transform.position, chased.transform.position), wallMask) && !chasing)
        {
            Debug.Log("wandering");
            Wander();
        }else if(Physics.Raycast(transform.position, (chased.transform.position - transform.position).normalized, Vector3.Distance(transform.position, chased.transform.position), wallMask) && chasing)
        {
            Chase();
        }
        else
        {
            if(chasedBeforeLastKnownPosition != Vector3.zero)
            {
                chasedBeforeLastKnownPosition = chasedLastKnownPosition;
            }
            else
            {
                chasedBeforeLastKnownPosition = Vector3Int.RoundToInt(chased.transform.position);
            }
            chasedLastKnownPosition = Vector3Int.RoundToInt(chased.transform.position);
            chasing = true;
            Chase();
        }
    }

    void Wander()
    {
        Ray forward = new Ray(transform.position, Vector3.forward);
        Ray back = new Ray(transform.position, Vector3.back);
        Ray left = new Ray(transform.position, Vector3.left);
        Ray right = new Ray(transform.position, Vector3.right);

        List<Ray> rays = new List<Ray>();
        rays.Add(forward);
        rays.Add(back);
        rays.Add(left);
        rays.Add(right);

        Vector3 currentDirection;

        bool directionsChanged = false;
        //Shoot raycasts and check if the viable directions are already in the available directions. If not add them and recalculate moving direction
        foreach(Ray ray in rays)
        {
            if(!Physics.Raycast(ray, 1, movementBlockLayer))
            {
                if (!availableDirections.Contains(ray.direction))
                {
                    availableDirections.Add(ray.direction);
                    directionsChanged = true;
                }
            }
            else
            {
                if (availableDirections.Contains(ray.direction))
                {
                    availableDirections.Remove(ray.direction);
                }
            }
        }

        if (directionsChanged)
        {
            //Randomize available directions if they have changed and also add the opposite vector of the index 0 so if the enemy is against a wall a new directions changed bool will not be launched
            availableDirections = Shuffle(availableDirections);
            if (!availableDirections.Contains(-availableDirections[0]))
            {
                availableDirections.Add(-availableDirections[0]);
            }
        }
        currentDirection = availableDirections[0];
        transform.position = Vector3.MoveTowards(transform.position, transform.position + currentDirection, 1);


    }

    void Chase()
    {
        transform.position = Vector3.MoveTowards(transform.position, chasedBeforeLastKnownPosition, 1);
        if(Vector3.Distance(transform.position, chasedBeforeLastKnownPosition) < 0.05f)
        {
            chasedBeforeLastKnownPosition = Vector3.zero;
            chasedLastKnownPosition = Vector3.zero;
            chasing = false;
        }
    }
    
    List<Vector3> Shuffle(List<Vector3> listToShuffle)
    {
        System.Random _rand = new System.Random();
        for (int i = listToShuffle.Count - 1; i > 0; i--)
        {
            var k = _rand.Next(i + 1);
            var value = listToShuffle[k];
            listToShuffle[k] = listToShuffle[i];
            listToShuffle[i] = value;
        }
        return listToShuffle;
    }

}
