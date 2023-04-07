using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ChasingAI : MonoBehaviour
{

    [SerializeField] private Image deathUIImage;
    [SerializeField] private TextMeshProUGUI deathTextUGUI;
    [SerializeField] private GameManager gameManager;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] LayerMask wallLayer;

    [SerializeField] LayerMask movementBlockLayer;
    [SerializeField] GameObject chased;
    [SerializeField] Vector3 chasedLastKnownPosition;

    [SerializeField] bool chasing = false;

    [SerializeField] List<Vector3> availableDirections = new List<Vector3>();

    private bool _canRunAI;

    private void Awake()
    {
        deathUIImage.fillAmount = 0;
        deathUIImage.gameObject.SetActive(false);
        _canRunAI = true;
        deathTextUGUI.DOFade(0, 0);
    }

    // private void OnEnable()
    // {
    //     PlayerMovement.OnMoved += RunAI;
    // }
    //
    // private void OnDisable()
    // {
    //     PlayerMovement.OnMoved -= RunAI;
    // }

    private void Update()
    {
        if (_canRunAI)
        {
            RunAI();
        }
    }

    void RunAI()
    {

        //Shoot raycast from chaser to chased and if it collides with any walls the chased is not visible setting chasing to false
        //If the raycast finds the player start chasing
        if (!findPlayerRaycast() && !chasing)
        {
            if (!checkForPlayer())
            {
                Wander();
                if (checkForPlayer())
                {
                    InitiateCombat();
                }
            }else
            {
                InitiateCombat();
            }
            
        }else if(!findPlayerRaycast() && chasing)
        {
            if (!checkForPlayer())
            {
                Chase(); // continue to last known position
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
        else
        {
            chasedLastKnownPosition = Vector3Int.RoundToInt(chased.transform.position);
            chasing = true;
            if (!checkForPlayer())
            {
                Chase();
                if (checkForPlayer())
                {
                    KillPlayer();
                    // InitiateCombat();
                }
            }
            else
            {
                KillPlayer();
                // InitiateCombat();
            }
        }
    }

    private void KillPlayer()
    {
        Debug.Log("Kill player");
        _canRunAI = false;
        StartCoroutine(KillPlayerSequence());

        IEnumerator KillPlayerSequence()
        {
            chased.GetComponent<PlayerMovement>().CanMove = false;
            var targetDirection = transform.position - chased.transform.position; // Get the direction to the target
            targetDirection.y = 0; // Make sure the billboard is only rotated on the y-axis

            if (targetDirection != Vector3.zero)
            {
                chased.transform.DORotate(Quaternion.LookRotation(targetDirection).eulerAngles, 0.2f, RotateMode.Fast);
                // chased.transform.rotation = Quaternion.LookRotation(-targetDirection); // Rotate the billboard to face the opposite direction of the target
            }

            yield return new WaitForSeconds(0.2f);
            
            transform.DOMove(chased.transform.position + Vector3.down * 0.25f, 0.2f).SetEase(Ease.Flash);

            yield return new WaitForSeconds(0.2f);
            deathUIImage.gameObject.SetActive(true);
            deathUIImage.DOFillAmount(1, 0.2f);
            deathTextUGUI.DOFade(1, 0.2f).SetDelay(0.1f);
            yield return new WaitForSeconds(3f);
            deathUIImage.fillOrigin = 0;
            deathUIImage.DOFillAmount(0, 0.2f);
            yield return new WaitForSeconds(0.2f);
            gameManager.LoseGame();
        }
    }

    private bool findPlayerRaycast()
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
        foreach(Ray ray in rays)
        {
            if(Physics.Raycast(ray.origin, ray.direction, out raycastHit, float.MaxValue, movementBlockLayer))
            {
                if(((1<<raycastHit.collider.gameObject.layer) & playerLayer) != 0)
                {
                    return true;
                }
            }
        }
        return false;
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
        transform.position = Vector3.MoveTowards(transform.position, Vector3Int.RoundToInt(transform.position + currentDirection), Time.deltaTime);


    }

    void Chase()
    {
        
        transform.position = Vector3.MoveTowards(transform.position, Vector3Int.RoundToInt(chasedLastKnownPosition), Time.deltaTime);
        if(Vector3.Distance(transform.position, chasedLastKnownPosition) < 0.05f && !findPlayerRaycast())
        {
            chasedLastKnownPosition = Vector3.zero;
            chasing = false;
        }else if(Vector3.Distance(transform.position, chasedLastKnownPosition) < 0.05f && findPlayerRaycast())
        {
            chasedLastKnownPosition = Vector3Int.RoundToInt(chased.transform.position);
        }
    }
    
    List<Vector3> Shuffle(List<Vector3> listToShuffle)
    {
        System.Random _rand = new System.Random();
        for (int i = listToShuffle.Count - 1; i > 0; i--)
        {
            var k = _rand.Next(i + 1);
            (listToShuffle[k], listToShuffle[i]) = (listToShuffle[i], listToShuffle[k]);
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
                if (Physics.Raycast(ray.origin, ray.direction, out raycastHit, 0.5f, playerLayer))
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
