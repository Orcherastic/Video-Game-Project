using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryTrashCan : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
{
    public Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(Player.MyInstance.interacting)
            animator.SetBool("Opened", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animator.SetBool("Opened", false);
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        InventoryItem item = dropped.GetComponent<InventoryItem>();
        if (!item.shopItem && !item.MyItem.undropable)
        {
            item.OnEndDrag(eventData);
            item.MyItem.gameObject.SetActive(true);
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
