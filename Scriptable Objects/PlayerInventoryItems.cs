using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItems", menuName = "ScriptableObjects/InventoryItems", order = 1)]
public class PlayerInventoryItems : ScriptableObject
{
    [Serializable]
    public class propItemInfo
    {
        public Item item;
        public int numberOf;
    }

    public List<propItemInfo> items = new List<propItemInfo>();
    public List<propItemInfo> quickUseItems = new List<propItemInfo>();
    public Armor helmet;
    public Armor chestplate;
    public Armor leggings;
    public Armor boots;
    public Armor weapon;
    public Armor accessory1;
    public Armor accessory2;
    public Item keyItem;

    public void AddItem(Item item, int numOf)
    {
        if (item == null)
        {
            propItemInfo i = new propItemInfo();
            i.item = null;
            i.numberOf = 0;
            items.Add(i);
        }
            
        else
        {
            foreach (Item i in AllInventoryItemsManager.MyInstance.allInventoryItems.allItems)
            {
                if (i.itemName == item.itemName)
                {
                    propItemInfo it = new propItemInfo();
                    it.item = i;
                    it.numberOf = numOf;
                    items.Add(it);
                }
            }
        }
    }

    public void AddQuickuseItem(Item item, int numOf)
    {
        if (item == null)
        {
            propItemInfo i = new propItemInfo();
            i.item = null;
            i.numberOf = 0;
            quickUseItems.Add(i);
        }

        else
        {
            foreach (Item i in AllInventoryItemsManager.MyInstance.allInventoryItems.allItems)
            {
                if (i.itemName == item.itemName)
                {
                    propItemInfo it = new propItemInfo();
                    it.item = i;
                    it.numberOf = numOf;
                    quickUseItems.Add(it);
                }
            }
        }
    }

    public void AddArmor(Armor armor, ArmorType type)
    {
        if(armor != null)
        {
            foreach (Item i in AllInventoryItemsManager.MyInstance.allInventoryItems.allItems)
            {
                if (i.itemName == armor.itemName)
                {
                    switch (armor.armorType)
                    {
                        case ArmorType.Helmet:
                            helmet = i as Armor;
                            break;
                        case ArmorType.Chestplate:
                            chestplate = i as Armor;
                            break;
                        case ArmorType.Leggings:
                            leggings = i as Armor;
                            break;
                        case ArmorType.Boots:
                            boots = i as Armor;
                            break;
                        case ArmorType.Weapon:
                            weapon = i as Armor;
                            break;
                    }
                }
            }
        }
        else
        {
            switch (type)
            {
                case ArmorType.Helmet:
                    helmet = null;
                    break;
                case ArmorType.Chestplate:
                    chestplate = null;
                    break;
                case ArmorType.Leggings:
                    leggings = null;
                    break;
                case ArmorType.Boots:
                    boots = null;
                    break;
                case ArmorType.Weapon:
                    weapon = null;
                    break;
            }
        }
    }

    public void AddAccessory(Armor armor, int number)
    {
        if (armor != null)
        {
            foreach (Item i in AllInventoryItemsManager.MyInstance.allInventoryItems.allItems)
            {
                if (i.itemName == armor.itemName)
                {
                    if (number == 0)
                        accessory1 = i as Armor;
                    else
                        accessory2 = i as Armor;
                }
            }
        }
        else
        {
            if (number == 0)
                accessory1 = null;
            else
                accessory2 = null;
        }
    }

    public void AddKeyItem(Item key)
    {
        if(key != null)
        {
            foreach (Item i in AllInventoryItemsManager.MyInstance.allInventoryItems.allItems)
            {
                if (i.itemName == key.itemName)
                {
                    keyItem = i;
                }
            }
        }
    }

    public void Reset()
    {
        items = new List<propItemInfo>();
        quickUseItems = new List<propItemInfo>();
        helmet = null;
        chestplate = null;
        leggings = null;
        boots = null;
        weapon = null;
        accessory1 = null;
        accessory2 = null;
        keyItem = null;
    }
}
