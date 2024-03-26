using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    public Transform parent;
    public Image image;
    public int numOfItems = 0;
    public Text numOfItemsTxt;
    public bool shopItem = false;

    public Item item;

    public Item MyItem
    {
        get
        {
            return item;
        }
        set
        {
            item = value;
        }
    }

    private Vector3 off;

    public bool IsEmpty
    {
        get { return numOfItems == 0; } //image.sprite == null; }
    }

    private void Update()
    {
        if (numOfItems < 2)
            numOfItemsTxt.gameObject.SetActive(false);
        else
            numOfItemsTxt.gameObject.SetActive(true);

        if(MyItem != null)
        { 
            if(MyItem is Weapon)
            {
                if ((MyItem as Weapon).HP <= 0 && (MyItem as Weapon).brokenSprite != null)
                {
                    image.sprite = (MyItem as Weapon).brokenSprite;
                    (MyItem as Weapon).weaponBroken = true;
                }
                else
                {
                    image.sprite = (MyItem as Weapon).MySprite;
                    (MyItem as Weapon).weaponBroken = false;
                }

            }
        }
    }

    public void RemoveItem()
    {
        //if(MyItem.itemType == ItemType.Armor )
            //(MyItem as Armor).ChangePlayerStats(false);
        image.sprite = null;
        image.color = Color.clear;
        numOfItems = 0;
        numOfItemsTxt.text = numOfItems.ToString();
        Destroy(MyItem.gameObject);
        MyItem = null;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!IsEmpty && !shopItem)
        {
            parent = transform.parent;
            transform.SetParent(transform.root);
            transform.SetAsLastSibling();
            image.raycastTarget = false;
            Player.MyInstance.interacting = true;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!IsEmpty && !shopItem)
            transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!IsEmpty && !shopItem)
        {
            transform.SetParent(parent);
            image.raycastTarget = true;
            Player.MyInstance.interacting = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!IsEmpty && !Player.MyInstance.interacting)
        {
            if (Player.MyInstance.shopping && parent.gameObject.CompareTag("ItemSlot"))
                MyItem.canSell = true;
            else
                MyItem.canSell = false;
            off = new Vector3(-30, 30, off.z);
            PlayerUI.MyInstance.ActivateTooltip(transform.position + off, item);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PlayerUI.MyInstance.DeactivateTooltip();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (Player.MyInstance.shopping && !Player.MyInstance.interacting && !IsEmpty && MyItem.canSell)
            {
                Player.MyInstance.coins += MyItem.baseSellPrice;
                CombatTextManager.MyInstance.CreateText(CoinTextPopUpHolder.MyInstance.transform.position, MyItem.baseSellPrice.ToString(), TextType.gainCoin);
                PlayerUI.MyInstance.SetCoins(Player.MyInstance.coins);
                numOfItems--;
                numOfItemsTxt.text = numOfItems.ToString();
                if (numOfItems <= 0)
                    RemoveItem();
            }
        }
    }
}
