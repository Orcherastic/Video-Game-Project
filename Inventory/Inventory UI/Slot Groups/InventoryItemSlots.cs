using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemSlots : MonoBehaviour
{
    private static InventoryItemSlots instance;

    public static InventoryItemSlots MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InventoryItemSlots>();
            }
            return instance;
        }
    }

    [SerializeField]
    private GameObject slotPrefab;
    [SerializeField]
    private GameObject unusableSlotPrefab;
    [SerializeField]
    private int numOfUsableSlots = 10;

    public List<ItemSlot> slots = new List<ItemSlot>();
    public List<ItemSlot> usableSlots = new List<ItemSlot>();

    private void Awake()
    {
        AddSlots(25);
    }

    void Start()
    {
        
    }

    public void AddSlots(int slotCount)
    {
        for(int i=0; i<slotCount; i++)
        {
            if(i < numOfUsableSlots)
            {
                ItemSlot slot = Instantiate(slotPrefab, transform).GetComponent<ItemSlot>();
                slots.Add(slot);
                usableSlots.Add(slot);
            }
            else
            {
                ItemSlot slot = Instantiate(unusableSlotPrefab, transform).GetComponent<ItemSlot>();
                slots.Add(slot);
            }
        }
    }

    private void Update()
    {
        if (Player.MyInstance.shopping)
        {
            // Player is shopping == Shop is Activated == Item Slots are in the Shop UI
            for (int i = 0; i < slots.Count; i++)
            {
                slots[i].transform.SetParent(InShopPlayerItemSlots.myInstance.transform);
            }
        }
        else
        {
            if (transform.parent.GetSiblingIndex() < InWeaponStatsPlayerItemSlots.MyInstance.transform.parent.GetSiblingIndex())
            {
                // Item Slots are in Weapon Stats UI
                for (int i = 0; i < slots.Count; i++)
                {
                    slots[i].transform.SetParent(InWeaponStatsPlayerItemSlots.MyInstance.transform);
                }
            }
            else
            {
                // Item Slots are in Player Inventory
                for (int i = 0; i < slots.Count; i++)
                {
                    slots[i].transform.SetParent(transform);
                }
            }
        }
    }

    public bool AddItem(Item item)
    {
        if (item == null)
        {
            foreach (ItemSlot slot in usableSlots)
            {
                if (slot.gameObject.GetComponentInChildren<InventoryItem>().IsEmpty)
                {
                    slot.AddItem(item);
                    return true;
                }
            }
        }

        if (item.itemType == ItemType.SpellBook || item.itemType == ItemType.KeyItem)
            return false;

        // Check if the Item is already in an Item Slot
        foreach (ItemSlot slot in usableSlots)
        {
            if (!slot.gameObject.GetComponentInChildren<InventoryItem>().IsEmpty)
            {
                if (slot.gameObject.GetComponentInChildren<InventoryItem>().MyItem.itemName == item.itemName && !slot.IsSlotFull)
                {
                    slot.UpdateNumOfItems();
                    return true;
                }
            }
        }
        // Check if the Item is in a Quick Use Slot
        foreach (QuickUseSlot slot in InventoryQuickUseSlots.MyInstance.slots)
        {
            if (!slot.gameObject.GetComponentInChildren<InventoryItem>().IsEmpty)
            {
                if (slot.gameObject.GetComponentInChildren<InventoryItem>().MyItem.itemName == item.itemName && !slot.IsSlotFull)
                {
                    slot.UpdateNumOfItems();
                    return true;
                }
            }
        }
        // Check if there are any Empty Slots in Inventory
        foreach (ItemSlot slot in usableSlots)
        {
            if (slot.gameObject.GetComponentInChildren<InventoryItem>().IsEmpty)
            {
                slot.AddItem(item);
                return true;
            }
        }
        // Cannot add Item
        return false;
    }

    public bool InventoryIsFull(Item item)
    {
        foreach(ItemSlot slot in usableSlots)
        {
            if(slot.gameObject.GetComponentInChildren<InventoryItem>() != null)
            {
                if (slot.gameObject.GetComponentInChildren<InventoryItem>().IsEmpty)
                    return false;
                else if (slot.gameObject.GetComponentInChildren<InventoryItem>().MyItem.itemName == item.itemName && 
                    !slot.IsSlotFull)
                    return false;
            }
        }
        // Show ItemPickUpPopUp when an Item is Picked Up
        ItemPickUpPopUpController.MyInstance.CreateInstance(null);
        return true;
    }
}
