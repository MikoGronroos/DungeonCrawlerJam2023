using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dice : MonoBehaviour
{
    //Dice Ground layer
    [SerializeField] LayerMask groundLayer;
    //Dice sides. Game object names need to be the resulting side if the dice turns on that side
    [SerializeField] List<GameObject> sides = new List<GameObject>();
    public bool diceLanded;
    [SerializeField] Rigidbody diceRb;

    public int sideLandedOn;
    

    private void Awake()
    {
        diceRb = GetComponent<Rigidbody>();
        //Add random torque
        diceRb.AddTorque(Random.Range(0, 500), Random.Range(0, 500), Random.Range(0, 500));
        //Add downwards velocity so the first physics frame velocity is not zero causing the dice side to be checked
        diceRb.velocity = Vector3.down;
    }

    void FixedUpdate()
    {
        //Check dice side when still
        if(diceRb.velocity == Vector3.zero && !diceLanded)
        {
            if (!diceLanded)
            {
                Time.timeScale = 1.0f;
            }
            diceLanded = true;
            sideLandedOn = checkDiceSide();
        }
        else
        {
            if (!diceLanded)
            {
                Time.timeScale = 3f;
            }
        }
    }

    int checkDiceSide()
    {
        foreach(GameObject side in sides)
        {
            if(Physics.CheckSphere(side.transform.position, 0.1f, groundLayer))
            {
                return int.Parse(side.gameObject.name);
            }
        }
        //Zero if no side landed 
        return 0;
    }

}
