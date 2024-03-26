using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemSlots : MonoBehaviour
{
    private static ShopItemSlots instance;

    public static ShopItemSlots MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ShopItemSlots>();
            }
            return instance;
        }
    }

    [SerializeField]
    private GameObject slotPrefab;

    private RectTransform rt;

    public List<ShopItemSlot> slots = new List<ShopItemSlot>();

    public List<Item> availableItemsInStore = new List<Item>();

    void Start()
    {
        rt = GetComponent<RectTransform>();
        AddShopItems();
    }

    void Update()
    {
        if(slots.Count >= 4)
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, (75 * slots.Count)+((slots.Count-1)*3.3f));    
        else
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, (75 * 4) + (3 * 3.3f));
    }

    public void AddShopItems()
    {
        foreach(Item i in availableItemsInStore)
        {
            ShopItemSlot slot = Instantiate(slotPrefab, transform).GetComponent<ShopItemSlot>();
            slot.invItem.parent = transform;
            slot.itemSprite.sprite = i.MySprite;
            slot.invItem.MyItem = i;
            slot.itemName.text = i.itemName;
            slot.itemPrice.text = i.baseCostPrice.ToString();
            if(i.itemType == ItemType.Armor || i.itemType == ItemType.SpellBook)
                slot.invItem.numOfItems = 1;
            else
                slot.invItem.numOfItems = Random.Range(0, 10);
            slot.invItem.numOfItemsTxt.text = slot.invItem.numOfItems.ToString();
            slots.Add(slot);
        }
    }
}
