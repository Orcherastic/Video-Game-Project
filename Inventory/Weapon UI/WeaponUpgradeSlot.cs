using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WeaponUpgradeSlot : MonoBehaviour, IDropHandler
{
    private static WeaponUpgradeSlot instance;
    public static WeaponUpgradeSlot MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<WeaponUpgradeSlot>();
            }
            return instance;
        }
    }

    [SerializeField]
    private Image myItemSprite;
    [SerializeField]
    private InventoryItem invItem;
    public Item myItem;

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

    void Update()
    {
        if(gameObject.GetComponentInChildren<InventoryItem>() != null)
        {
            if (gameObject.GetComponentInChildren<InventoryItem>().MyItem != null)
            {
                Player.MyInstance.weaponUpgrading = true;
            }

            else
            {
                Player.MyInstance.weaponUpgrading = false;
            }

        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        invItem = this.gameObject.GetComponentInChildren<InventoryItem>();
        GameObject dropped = eventData.pointerDrag;
        InventoryItem item = dropped.GetComponent<InventoryItem>();
        if(Player.MyInstance.equipedWeapon)
        {
            if ((myItemSprite.sprite != null && item.image.sprite != null) || item.image.sprite != null &&
            (item.MyItem.itemType == ItemType.MonsterLoot))
            {
                if (!item.parent.gameObject.CompareTag("EquipSlot") && !invItem.IsEmpty)
                {
                    Player.MyInstance.DowngradeWeaponStats(invItem.MyItem as MonsterLoot);
                }

                if (invItem != null)
                {
                    Transform o = transform.GetChild(0);
                    o.SetParent(item.parent);
                    item.parent = transform;
                    myItem = item.MyItem;
                    Player.MyInstance.UpgradeWeaponStats(item.MyItem as MonsterLoot);
                }
                item.parent = transform;
                myItem = item.MyItem;

                EquipSlots[] slots = FindObjectsOfType<EquipSlots>();
                foreach (EquipSlots slot in slots)
                {
                    if (slot.gameObject.GetComponentInChildren<InventoryItem>() != null)
                    {
                        if ((slot.gameObject.GetComponentInChildren<InventoryItem>().MyItem as Weapon) != null)
                        {
                            slot.gameObject.GetComponentInChildren<InventoryItem>().shopItem = true;
                        }
                    }
                }
            }
        }
        
    }
}
