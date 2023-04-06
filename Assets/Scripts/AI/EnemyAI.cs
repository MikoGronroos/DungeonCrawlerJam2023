using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] LayerMask playerLayer;
    [SerializeField] LayerMask wallLayer;

    [SerializeField] LayerMask movementBlockLayer;

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
        //If player is not next to enemy move and check again and if true start combat. Otherwise start combat
        if (!checkForPlayer())
        {
            Wander();
            if (checkForPlayer())
            {
                InitiateCombat();
            }
        }
        else
        {
            InitiateCombat();
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
        foreach (Ray ray in rays)
        {
            if (!Physics.Raycast(ray, 1, movementBlockLayer))
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

    private bool checkForPlayer()
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
        RaycastHit raycastHit;
        foreach (Ray ray in rays)
        {
            if (!Physics.Raycast(ray, 1, wallLayer))
            {
                if (Physics.Raycast(ray.origin, ray.direction, out raycastHit, 1.2f, playerLayer))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void InitiateCombat()
    {
        Combat.Instance.StartCombat(GetComponent<IParticipant>());
    }

}
