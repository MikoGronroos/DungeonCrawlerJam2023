using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    [SerializeField] private int inventoryMaxSize;

    [SerializeField] private Item[] inventoryItems = new Item[4];

    [SerializeField] private Slot[] inventorySlots = new Slot[4];

    #region Singleton

    private static Inventory instance;

    public static Inventory Instance
    {
        get { return instance; }
    }

    #endregion

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SetupSlots();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X)) SetupSlots();
    }

    public void AddItem(Item item)
    {
        var index = FindIndexOfEmptySlot();

        if (index <= inventoryMaxSize)
        {
            inventoryItems[index] = item;
        }
        else
        {
            Debug.Log("Couldn't find an empty slot.");
        }

    }

    public bool HasItem(Item item)
    {
        bool hasItem = false;
        foreach (var inventoryItem in inventoryItems)
        {

            if (inventoryItem == null)
            {
                continue;
            }

            if (item.ItemId == inventoryItem.ItemId)
            {
                hasItem = true;
                break;
            }
        }
        return hasItem;
    }

    public void RemoveItem(Item item)
    {
        int index = FindIndexOfItemsSlot(item);
        inventoryItems[index] = null;
        SetupSlots();
    }

    private int FindIndexOfItemsSlot(Item item)
    {
        int index = 0;
        for (index = 0; index < inventoryItems.Length; index++)
        {

            if (inventoryItems[index] == null)
            {
                continue;
            }

            if (inventoryItems[index].ItemId == item.ItemId)
            {
                break;
            }
        }
        return index;
    }

    private int FindIndexOfEmptySlot()
    {
        int index = 0;
        bool noneIsEmpty = true;
        for (index = 0; index < inventoryItems.Length; index++)
        {
            if (inventoryItems[index] != null)
            {
                noneIsEmpty = false;
                break;
            }
        }
        if (noneIsEmpty)
        {
            index = index + 1;
        }
        return index;
    }

    private void SetupSlots()
    {
        for (int i = 0; i < inventoryItems.Length; i++)
        {
            var slot = inventorySlots[i];
            if (inventoryItems[i] == null)
            {
                slot.Setup(null);
                continue;
            }
            slot.Setup(inventoryItems[i].ItemIcon);
        }
    }
}

public abstract class Item : ScriptableObject
{
    public string ItemName;
    public string ItemId;
    public Sprite ItemIcon;

    [ContextMenu("Generate new id")]
    public void GenerateId() => ItemId = Guid.NewGuid().ToString();


}
