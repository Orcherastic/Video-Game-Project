using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Animator anim;
    public Image itemSprite;
    public InventoryItem invItem;
    public Text itemName;
    public Text itemPrice;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        anim.SetBool("Selected", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        anim.SetBool("Selected", false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(anim.GetBool("Selected") && !Player.MyInstance.interacting)
        {
            if (InventoryItemSlots.MyInstance.InventoryIsFull(invItem.MyItem))
            {
                ItemPickUpPopUpController.MyInstance.CreateInstance(null);
                return;
            }
            if (Player.MyInstance.coins >= int.Parse(itemPrice.text))
            {
                Player.MyInstance.coins -= int.Parse(itemPrice.text);
                CombatTextManager.MyInstance.CreateText(CoinTextPopUpHolder.MyInstance.transform.position, itemPrice.text, TextType.loseCoin);
                invItem.numOfItems--;
                invItem.numOfItemsTxt.text = invItem.numOfItems.ToString();
                if (invItem.numOfItems <= 0)
                {
                    gameObject.GetComponentInParent<ShopItemSlots>().slots.Remove(this);
                    Destroy(this.gameObject);
                }
                PlayerUI.MyInstance.SetCoins(Player.MyInstance.coins);
                if(invItem.MyItem.itemType == ItemType.SpellBook)
                {
                    PlayerSpellsManager.MyInstance.playerSpells.spellBooks.Add(invItem.MyItem);
                    foreach(MagicSlot slot in InventoryMagicSlots.MyInstance.slots)
                    {
                        if (invItem.MyItem.itemName == slot.myMagicName)
                        {
                            slot.unlocked = true;
                            break;
                        }
                    }
                }
                else
                {
                    Item item = Instantiate(invItem.MyItem, transform.position, Quaternion.identity);
                    item.gameObject.SetActive(false);
                    InventoryItemSlots.MyInstance.AddItem(item);
                }

            }
            else
            {
                Debug.Log("Not Enough Coin!");
            }
        }
    }
}
