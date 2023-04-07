using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableRitualKnife : MonoBehaviour
{
    [SerializeField] private WorldManager worldManager;
    [SerializeField] private GameObject ritualKnifeUI;

    private void Awake()
    {
        ritualKnifeUI.SetActive(false);
        worldManager.canMoveBetweenWorlds = false;
    }

    public void EnableKnife()
    {
        ritualKnifeUI.SetActive(true);
        worldManager.canMoveBetweenWorlds = true;
    }
}
