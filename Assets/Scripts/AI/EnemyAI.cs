using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour
{
    
    [Range(0,1)][SerializeField] private float aggressionLevel;
    
    [SerializeField] LayerMask playerLayer;
    [SerializeField] LayerMask wallLayer;

    [SerializeField] LayerMask movementBlockLayer;

    [SerializeField] GameObject chased;
    [SerializeField] Vector3 chasedLastKnownPosition;

    [SerializeField] bool chasing = false;
    
    [SerializeField] List<Vector3> availableDirections = new List<Vector3>();

    private Vector3 _lastPosition;

    private bool CanMove => availableDirections is { Count: > 0 };

    private EnemyState _enemyState;
    private enum EnemyState
    {
        WANDERING,
        CHASING
    }

    private void Awake()
    {
        _lastPosition = transform.position;
        _enemyState = EnemyState.WANDERING;
    }

    private void OnEnable()
    {
        PlayerMovement.OnMoved += RunAI;
    }

    private void OnDisable()
    {
        PlayerMovement.OnMoved -= RunAI;
    }

    private void RunAI()
    {
        switch (_enemyState)
        {
            case EnemyState.WANDERING:
                Wander();
                break;
            case EnemyState.CHASING:
                Chase();
                break;
        }
    }

    private void Wander()
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
                
        var targetPosition = transform.position + currentDirection;
        var newPosIsOldPos = Vector3.Distance(targetPosition, _lastPosition) <= 0.3f;
        var shouldFindNewPos = CanMove && newPosIsOldPos;
        
        if (shouldFindNewPos)
        {
            Wander();
        }
        else
        {
            MoveToPosition(transform.position + currentDirection);
        }
        
        _lastPosition = targetPosition;
    }

    private void Chase()
    {
        MoveToPosition(chasedLastKnownPosition);
    }
    
    private void MoveToPosition(Vector3 pos)
    {
        transform.DOMove(Vector3Int.RoundToInt(pos), 0.5f).SetEase(Ease.Flash).OnComplete(() =>
        {
            _enemyState = CanSeePlayer() ? EnemyState.CHASING : EnemyState.WANDERING;
            chasedLastKnownPosition = chased.transform.position;
            
            if (aggressionLevel < 1 && Random.Range(0, 1) < aggressionLevel)
            {
                _enemyState = EnemyState.WANDERING;
            }
        });
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

    private bool CanSeePlayer()
    {
        return Vector3.Distance(transform.position, chased.transform.position) <= 1.1f && !Physics.Raycast(transform.position, chased.transform.position, 1,wallLayer);
    }

    // private void InitiateCombat()
    // {
    //     Combat.Instance.StartCombat(GetComponent<IParticipant>());
    // }

}
