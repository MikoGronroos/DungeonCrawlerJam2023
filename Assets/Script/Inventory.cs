using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    [SerializeField] private Item[] inventoryItems = new Item[8];

    public void AddItem()
    {

    }

    private int FindIndexOfEmptySlot()
    {
        int index = 0;
        for (index = 0; index < inventoryItems.Length; index++)
        {

        }
        return index;
    }

}


public class Item
{

}