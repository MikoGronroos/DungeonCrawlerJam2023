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
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ConsumeItem(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ConsumeItem(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ConsumeItem(2);
        }
    }

    private void ConsumeItem(int index)
    {

        if (inventoryItems[index] == null)
        {
            return;
        }

        if (inventoryItems[index].IsConsumable)
        {
            Player.Instance.AddHealth((inventoryItems[index] as ItemPotion).HealthAddon);
            RemoveItem(inventoryItems[index]);
        }

        if (inventoryItems[index] as ItemKey)
        {
            Player.Instance.TryToOpenDoor(); 
            //No need to remove key because door.cs does it for you
        }
    }

    public bool TryToAddItem(Item item, GridCell cell)
    {
        if(item as ItemKey && !inventoryItems[1])
        {
            inventoryItems[1] = item;
        }else if(item as ItemKey && inventoryItems[1])
        {
            
        }

        if(item as ItemPotion && !inventoryItems[0])
        {
            inventoryItems[0] = item;
        }else if(item as ItemPotion && !inventoryItems[0])
        {

        }

        /*
        bool addedItem = false;
        var index = FindIndexOfEmptySlot();

        if (index <= inventoryMaxSize - 1)
        {
            inventoryItems[index] = item;
            addedItem = true;
        }
        else
        {
            Debug.Log("Couldn't find an empty slot.");
        }
        */
        SetupSlots();
        return true;
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
        foreach (var item in inventoryItems)
        {
            if (inventoryItems[index] == null)
            {
                noneIsEmpty = false;
                break;
            }
            index++;
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
    public bool IsConsumable;

    [ContextMenu("Generate new id")]
    public void GenerateId() => ItemId = Guid.NewGuid().ToString();


}
