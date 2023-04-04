using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{

    [SerializeField] private GameObject hell;
    [SerializeField] private GameObject overworld;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            MoveBetweenWorlds();
        }
    }

    public void MoveBetweenWorlds()
    {
        hell.SetActive(!hell.activeSelf);
        overworld.SetActive(!overworld.activeSelf);
    }

}
