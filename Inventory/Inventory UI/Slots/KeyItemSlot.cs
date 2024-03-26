using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class KeyItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Image myItemSprite;
    private Item myItem;

    private Vector3 off;

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

    public bool AddItem(Item item)
    {
        myItem = item;
        myItemSprite.sprite = item.MySprite;
        myItemSprite.color = Color.white;
        return true;
    }

    public void RemoveItem()
    {
        myItemSprite.sprite = null;
        myItemSprite.color = Color.clear;
        Destroy(MyItem.gameObject);
        myItem = null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (myItem != null && !Player.MyInstance.interacting)
        {
            off = new Vector3(-30, 30, off.z);
            PlayerUI.MyInstance.ActivateTooltip(transform.position + off, myItem);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PlayerUI.MyInstance.DeactivateTooltip();
    }
}
