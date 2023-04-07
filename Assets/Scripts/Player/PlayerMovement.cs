using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Moved event
    public delegate void PlayerMoved();
    public static event PlayerMoved OnMoved;

    public float moveDuration = 0.5f;
    public float transitionRotationSpeed = 500f;

    private Vector3 targetGridPos;
    private Vector3 targetRotation;

    public AudioSource WalkSound;

    [Tooltip("This variable is controlled from input state controller")]
    [field: SerializeField] public bool CanMove { get; set; }
    
    [SerializeField] private int movementMultiplier;

    [SerializeField] private LayerMask wallLayers;
    [SerializeField] private LayerMask enemyLayers;

    public bool _playerStill;

    private bool _rotateRightIsBuffered;
    private bool _rotateLeftIsBuffered;
    private bool _moveForwardIsBuffered;

    private Tween _moveTween;

    private bool HasAlmostFinishedMoving => _moveTween != null && _moveTween.position > moveDuration - moveDuration / 4f;
    private void Start()
    {
        targetGridPos = Vector3Int.RoundToInt(transform.position);
        targetRotation = new Vector3(0, 90, 0);
        _playerStill = true;
    }
    
    private void Update()
    {
        if (!CanMove) return;

        UpdateMoveInput();
        UpdateRotateInput();
        
        
        Debug.Log(CanMoveForward());
    }

    private void LateUpdate()
    {      
        if (!CanMove) return;

        // Constantly update rotation
        if (targetRotation.y > 270f && targetRotation.y < 361f) targetRotation.y = 0f;
        if (targetRotation.y < 0f) targetRotation.y = 270f;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetRotation), Time.deltaTime * transitionRotationSpeed);
    }
    
    private void UpdateRotateInput()
    {
        if (_playerStill) // Rotate
        {
            if (Input.GetKeyDown(KeyCode.D)) RotateRight();
            if (Input.GetKeyDown(KeyCode.A)) RotateLeft();
        }
        else if(HasAlmostFinishedMoving) // Buffer rotation input when moving and almost stopped
        {
            if (!_rotateRightIsBuffered && Input.GetKeyDown(KeyCode.D)) _rotateRightIsBuffered = true;
            if (!_rotateLeftIsBuffered && Input.GetKeyDown(KeyCode.A)) _rotateLeftIsBuffered = true;
        }
    }

    private void UpdateMoveInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            // Dont move if already moving
            if (CanMoveForward())  
            {
                MoveForward();
            }
            else if (HasAlmostFinishedMoving)
            {
                _moveForwardIsBuffered = true;
            }

        }
    }

    private bool CanMoveForward()
    {
        if (ObstacleIsInFront()) return false; // Dont move if wall or enemy in front
        if(transform.eulerAngles != targetRotation) return false; // Dont move while rotating (when player presses rotate at the same time as move)
        if (!_playerStill) return false;

        return true;
    }

    private void MoveForward()
    {
        _playerStill = false;
        if (WalkSound) WalkSound.Play();
        _moveTween = transform.DOMove(transform.position + transform.forward, moveDuration).SetEase(Ease.Flash).OnComplete(
            () =>
            {
                OnMoved?.Invoke();
                CheckTile(targetGridPos);

                if (_moveForwardIsBuffered)
                {
                    _moveForwardIsBuffered = false;

                    if (CanMoveForward())
                    {
                        MoveForward();
                    }
                    else
                    {
                        _playerStill = true;
                    }
                }
                else
                {
                    _playerStill = true;
                    RotateIfBuffered();
                }
            });
    }

    private void RotateIfBuffered()
    {
        if (_rotateRightIsBuffered)
        {
            RotateRight();
            _rotateRightIsBuffered = false;
        }
        
        if (_rotateLeftIsBuffered)
        {
            RotateLeft();
            _rotateLeftIsBuffered = false;
        }
    }
    
    public void LookAtTheEnemy(Transform target)
    {
        Vector3 dir = target.position - transform.position;
        StartCoroutine(LerpLookAt(dir, 1));
    }
    public IEnumerator LerpLookAt(Vector3 dir, float duration)
    {
        float time = 0;
        while (time < duration)
        {
            Quaternion rot = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), time / duration);
            time += Time.deltaTime;
            rot.x = 0;
            rot.z = 0;
            transform.rotation = rot;
            yield return null;
        }
    }
    
    private bool ObstacleIsInFront()
    {
        var isWall = Physics.Raycast(transform.position, transform.forward, 1, wallLayers);
        var isEnemy = Physics.Raycast(transform.position, transform.forward, 2, enemyLayers);

        return isWall || isEnemy;
    }

    public void RotateLeft() { targetRotation -= Vector3.up * 90f; }
    public void RotateRight() { targetRotation += Vector3.up * 90f; }

    private void CheckTile(Vector3 targetGridPos)
    {
        int x = (int)Mathf.Ceil(targetGridPos.x);
        int z = (int)Mathf.Ceil(targetGridPos.z);
        // Debug.Log(x + " " + z);
        if (Grid.GridCells.ContainsKey(new Vector2(x, z)))
        {
            Grid.GridCells[new Vector2(x, z)].OnStepped();
        }
    }
}