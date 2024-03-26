using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum OutsidePosition
{
    Bottom,
    Left,
    Right,
    Top
}

public class InventoryOutsides : MonoBehaviour, IDropHandler
{
    private Vector3 off;
    public OutsidePosition outsidePosition;
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        InventoryItem item = dropped.GetComponent<InventoryItem>();
        if(item.MyItem != null && !item.shopItem && !item.MyItem.undropable)
        {
            item.OnEndDrag(eventData);
            item.MyItem.gameObject.SetActive(true);
            switch (outsidePosition)
            {
                case OutsidePosition.Bottom:
                    off = new Vector3(off.x, Random.Range(-4, -3), off.z);
                    break;
                case OutsidePosition.Left:
                    off = new Vector3(Random.Range(-4, -3), off.y, off.z);
                    break;
                case OutsidePosition.Right:
                    off = new Vector3(Random.Range(3, 4), off.y, off.z);
                    break;
                case OutsidePosition.Top:
                    off = new Vector3(off.x, Random.Range(3, 4), off.z);
                    break;
            }
            Item spawnedItem;
            for (int i = 0; i < item.numOfItems; i++)
            {
                spawnedItem = Instantiate(item.MyItem, Player.MyInstance.transform.position, Quaternion.identity);
                spawnedItem.magnetize = false;
                spawnedItem.rb.MovePosition(Player.MyInstance.transform.position + off * 2f * Time.deltaTime);
            }

            if (item.parent.gameObject.CompareTag("EquipSlot"))
            {
                EquipSlots slot = item.parent.gameObject.GetComponent<EquipSlots>();
                (item.MyItem as Armor).ChangePlayerStats(false);
                slot.PlayerEquipment.Dequip();
                // Remove Items Status Effects if it has any
                if (item.MyItem is IStatusEffectable)
                    (item.MyItem as IStatusEffectable).RemoveStatusEffects();
            }
            if (item.parent.gameObject.CompareTag("Weapon Upgrade Slot"))
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
            item.RemoveItem();
            //Destroy(item.MyItem.gameObject);
        }

    }
}
