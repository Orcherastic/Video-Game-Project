using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class QuickUseSlot : MonoBehaviour, IDropHandler
{
    public Animator animator;
    [SerializeField]
    public Image myItemSprite;
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

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Used == Light Gray background
        if (this.gameObject.GetComponentInChildren<InventoryItem>() == null ||
            this.gameObject.GetComponentInChildren<InventoryItem>().IsEmpty)
            animator.SetBool("Used", false);
        else
            animator.SetBool("Used", true);
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

    public void UseItem()
    {
        invItem = this.gameObject.GetComponentInChildren<InventoryItem>();
        // If the Item is Usable
        if (invItem.MyItem is IUsable && invItem.MyItem.CanBeUsed())
        {
            // Activate the Usable Items Use Function
            (invItem.MyItem as IUsable).TakeItem();
            InventoryQuickUseSlots.MyInstance.slotWithUsedItem = this;
        }
    }

    public void DecreceItemNumber()
    {
        invItem = this.gameObject.GetComponentInChildren<InventoryItem>();
        // Decrease Item number, if 0 remove Item
        invItem.numOfItems -= 1;

        if (invItem.numOfItems <= 0)
        {
            invItem.RemoveItem();
            // Deselect the Slot, it is no longer light gray
            animator.SetBool("Selected", false);
            animator.SetBool("Used", false);
        }
        else
        {
            invItem.numOfItemsTxt.text = invItem.numOfItems.ToString();
        }
    }

    public void UpdateNumOfItems()
    {
        invItem = this.gameObject.GetComponentInChildren<InventoryItem>();
        invItem.numOfItems++;
        if (invItem.numOfItems > invItem.MyItem.StackSize)
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
        // If the item being dropped is a Consumable
        if ((myItemSprite.sprite != null || item.image.sprite != null) && item.MyItem.itemType == ItemType.Consumable)
        {
            if (invItem != null)
            {
                Transform o = transform.GetChild(0);
                o.SetParent(item.parent);
            }
            item.parent = transform;
        }
    }
}
