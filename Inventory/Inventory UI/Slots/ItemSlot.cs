using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    [SerializeField]
    private Image myItemSprite;
    [SerializeField]
    private InventoryItem invItem;
    private Item myItem;

    public Item MyItem
    {
        get
        {
            return myItem;
        }
        set
        {
            myItem = value;
        }
    }

    public bool IsEmpty
    {
        get
        {
            return this.gameObject.GetComponentInChildren<InventoryItem>().numOfItems == 0;
        }
    }

    public bool IsSlotFull
    {
        get
        {
            return this.gameObject.GetComponentInChildren<InventoryItem>().numOfItems >=
                 this.gameObject.GetComponentInChildren<InventoryItem>().MyItem.StackSize;
        }
    }

    public bool AddItem(Item item)
    {
        if (item == null)
            return false;
        invItem = this.gameObject.GetComponentInChildren<InventoryItem>();
        myItem = item;
        invItem.MyItem = item;
        invItem.parent = transform;
        invItem.image.sprite = item.MySprite;
        invItem.image.color = Color.white;
        UpdateNumOfItems();
        return true;
    }

    public void UpdateNumOfItems()
    {
        invItem = this.gameObject.GetComponentInChildren<InventoryItem>();
        invItem.numOfItems++;
        if(invItem.numOfItems > invItem.MyItem.StackSize)
        {
            invItem.numOfItems = invItem.MyItem.StackSize;
        }
        invItem.numOfItemsTxt.text = invItem.numOfItems.ToString();
    }

    public void OnDrop(PointerEventData eventData)
    {
        invItem = this.gameObject.GetComponentInChildren<InventoryItem>();
        GameObject dropped = eventData.pointerDrag;
        InventoryItem item = dropped.GetComponent<InventoryItem>();
        if(!gameObject.CompareTag("UnusableItemSlot") && !item.shopItem)
        {
            // Slots with no items can't be clicked or dragged 
            if ((myItemSprite.sprite != null && item.image.sprite != null) || item.image.sprite != null)
            {
                // If the item is from an Equip Slot and the Item Slot is Empty, 
                // reduce Item Buffs on Player Stats
                if (item.parent.gameObject.CompareTag("EquipSlot") && invItem.IsEmpty)
                {
                    EquipSlots slot = item.parent.gameObject.GetComponent<EquipSlots>();
                    (item.MyItem as Armor).ChangePlayerStats(false);
                    slot.PlayerEquipment.Dequip();
                    // Remove Items Status Effects if it has any
                    if (item.MyItem is IStatusEffectable)
                        (item.MyItem as IStatusEffectable).RemoveStatusEffects();

                }
                // If the item is from Weapon Upgrade Slot and the Item Slot is Empty, 
                // reduce Item Buffs on Weapon Stats
                if (item.parent.gameObject.CompareTag("Weapon Upgrade Slot") && invItem.IsEmpty)
                {
                    Player.MyInstance.DowngradeWeaponStats(item.MyItem as MonsterLoot);

                    EquipSlots[] slots = FindObjectsOfType<EquipSlots>();
                    foreach (EquipSlots slot in slots)
                    {
                        if (slot.gameObject.GetComponentInChildren<InventoryItem>() != null)
                        {
                            if ((slot.gameObject.GetComponentInChildren<InventoryItem>().MyItem as Weapon) != null)
                            {
                                slot.gameObject.GetComponentInChildren<InventoryItem>().shopItem = false;
                            }
                        }
                    }
                }
                if(item.parent.gameObject.CompareTag("Weapon Repair Slot") && invItem.IsEmpty)
                {
                    WeaponRepairSlot.MyInstance.weaponRepairCostText.text = "0";
                }
                if(item.parent.gameObject.CompareTag("Weapon Repair Slot") && !invItem.IsEmpty && invItem.MyItem is Weapon)
                {
                    WeaponRepairSlot.MyInstance.CalculateRepairCost(invItem.MyItem as Weapon);
                }
                // If an Item Slot item is dropped on another Item Slot or if the item comes from a Quick Slot and
                // the Item Slot is Empty or it has a Consumable item or if the item comes from an Equip Slot and
                // the Item Slot is Empty or it has an Armor item of the same Armor Type as the item being dragged
                if (item.parent.gameObject.CompareTag("ItemSlot") || invItem.IsEmpty ||
                    (item.parent.gameObject.CompareTag("QuickSlot") && invItem.MyItem.itemType == ItemType.Consumable) ||
                    (item.parent.gameObject.CompareTag("EquipSlot") && (invItem.MyItem as Armor).armorType == (item.MyItem as Armor).armorType) ||
                    (item.parent.gameObject.CompareTag("Weapon Upgrade Slot") && invItem.MyItem.itemType == ItemType.MonsterLoot))
                {
                    // If both items are the same Armor Type, add this items buffs and remove the other ones
                    if (item.parent.gameObject.CompareTag("EquipSlot") && invItem.MyItem is Armor)
                    {
                        EquipSlots slot = item.parent.gameObject.GetComponent<EquipSlots>();
                        (invItem.MyItem as Armor).ChangePlayerStats(true);
                        // Add this Items Status Effects if it has any
                        if (invItem.MyItem is IStatusEffectable)
                            (invItem.MyItem as IStatusEffectable).AddStatusEffects();

                        (item.MyItem as Armor).ChangePlayerStats(false);
                        // Remove the other Items Status Effects if it has any
                        if (item.MyItem is IStatusEffectable)
                            (item.MyItem as IStatusEffectable).RemoveStatusEffects();

                        slot.PlayerEquipment.Dequip();
                        slot.PlayerEquipment.Equip((invItem.MyItem as Armor).Animations);
                    }
                    // If both Items are Monster Loot add this Items Buffs to the Weapon and remove the other ones buffs
                    if (item.parent.gameObject.CompareTag("Weapon Upgrade Slot") && invItem.MyItem is MonsterLoot)
                    {
                        Player.MyInstance.DowngradeWeaponStats(item.MyItem as MonsterLoot);
                        Player.MyInstance.UpgradeWeaponStats(invItem.MyItem as MonsterLoot);
                    }
                    // Switch items
                    // If the item is not dropped on the same Item Slot
                    if (invItem != null)
                    {
                        Transform o = transform.GetChild(0);
                        o.SetParent(item.parent);
                        // Set the Equip Slots Armor to be this Item
                        if (o.parent.gameObject.CompareTag("EquipSlot"))
                            o.parent.gameObject.GetComponent<EquipSlots>().MyArmor =
                                o.gameObject.GetComponentInChildren<InventoryItem>().MyItem as Armor;
                    }
                    item.parent = transform;
                    //this.gameObject.GetComponentInChildren<InventoryItem>().MyItem = item.MyItem;
                }
            }
        }

    }
}
