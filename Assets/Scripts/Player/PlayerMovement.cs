using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Moved event
    public delegate void PlayerMoved();
    public static event PlayerMoved OnMoved;

    public bool playerStill;

    public bool smoothTransition = false;
    public float transitionSpeed = 10f;
    public float transitionRotationSpeed = 500f;

    public Vector3 targetGridPos;
    public Vector3 prevTargetGridPos;
    public Vector3 targetRotation;

    [SerializeField] private int movementMultiplyer;

    private void Start()
    {
        targetGridPos = Vector3Int.RoundToInt(transform.position);
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.W)) MoveForward();
        if (Input.GetKeyDown(KeyCode.S)) MoveBackward();
        if (Input.GetKeyDown(KeyCode.D)) RotateRight();
        if (Input.GetKeyDown(KeyCode.A)) RotateLeft();

        if (canMove())
        {
            Move();
        }
    }

    private void Move()
    {
        if (true)
        {
            prevTargetGridPos = targetGridPos;

            Vector3 targetPosition = targetGridPos;

            if (targetRotation.y > 270f && targetRotation.y < 361f) targetRotation.y = 0f;
            if (targetRotation.y < 0f) targetRotation.y = 270f;

            if (!smoothTransition)
            {
                transform.position = targetPosition;
                transform.rotation = Quaternion.Euler(targetRotation);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * transitionSpeed);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetRotation), Time.deltaTime * transitionRotationSpeed);
            }

            //Fire OnMoved Event
            if((Vector3.Distance(transform.position, targetGridPos) < 0.05) && !playerStill) //Player has now reached the goal so call the OnMoved function and set player status to still
            {
                playerStill = true;
                OnMoved();
            }else if(!(Vector3.Distance(transform.position, targetGridPos) < 0.05)) //If player has not reached the goal set playerStill to false
            {
                playerStill = false;
            }
        }
    }

    private bool canMove()
    {

        if (Physics.Raycast(transform.position, (new Vector3(targetGridPos.x, transform.position.y, targetGridPos.z) - transform.position).normalized, Vector3.Distance(targetGridPos, transform.position)))
        {
            targetGridPos = prevTargetGridPos;
            return false;
        }
        else
        {
            return true;
        }
        
    }

    public void RotateLeft() { if (AtRest) targetRotation -= Vector3.up * 90f; }
    public void RotateRight() { if (AtRest) targetRotation += Vector3.up * 90f; }
    public void MoveForward() { if (AtRest) { 
            targetGridPos += transform.forward * movementMultiplyer;
            CheckTile(targetGridPos);
        } }

    private void CheckTile(Vector3 targetGridPos)
    {
        int x = (int)targetGridPos.x;
        int z = (int)targetGridPos.z;
        if (Grid.GridCells.ContainsKey(new Vector2(x, z)))
        {
            Grid.GridCells[new Vector2(x, z)].OnStepped();
        }
    }

    public void MoveBackward() { if (AtRest) { 
            targetGridPos -= transform.forward * movementMultiplyer;
            CheckTile(targetGridPos);
        } }
    public void MoveLeft() { if (AtRest) targetGridPos -= transform.right; }
    public void MoveRight() { if (AtRest) targetGridPos += transform.right; }

    bool AtRest
    {
        get
        {
            if ((Vector3.Distance(transform.position, targetGridPos) < 0.05) && (Vector3.Distance(transform.eulerAngles, targetRotation) < 0.05f))
                return true;
            else
                return false;
        }
    }
}