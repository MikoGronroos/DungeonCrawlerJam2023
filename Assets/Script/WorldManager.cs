using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    [SerializeField] private Color fogColorOverworld;
    [SerializeField] private Color fogColorHell;
    [SerializeField] private GameObject hell;
    [SerializeField] private GameObject overworld;

    public bool canMoveBetweenWorlds;
    
    private void Start()
    {
        RenderSettings.fogColor = fogColorOverworld; 
    }

    private void Update()
    {
        if (canMoveBetweenWorlds)
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                MoveBetweenWorlds();
            }
        }
    }

    public void MoveBetweenWorlds()
    {
        hell.SetActive(!hell.activeSelf);
        overworld.SetActive(!overworld.activeSelf);
        
        RenderSettings.fogColor = hell.activeSelf ? fogColorHell : fogColorOverworld; 
    }

}
