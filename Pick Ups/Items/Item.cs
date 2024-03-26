using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    KeyItem,
    Consumable,
    MonsterLoot,
    Armor,
    SpellBook
}
public abstract class Item : PickUp, IDescriptable
{
    [SerializeField]
    protected string id;
    [ContextMenu("Generate Guid for ID")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }
    private bool collected = false;
    public bool undropable = false;
    [Header("Item Properties")]
    public string itemName;
    public ItemType itemType;
    [SerializeField]
    private Sprite sprite;
    [SerializeField]
    private int stackSize;
    private ItemSlot slot;

    [Header("Item Shop Prices")]
    public int baseCostPrice;
    public int baseSellPrice;
    public bool canSell = false;

    public Sprite MySprite 
    {
        get { return sprite; }
        set { sprite = value; }
    }

    public int StackSize 
    {
        get { return stackSize; }
    }

    public ItemSlot Slot 
    {
        get { return slot; }
        set { slot = value; }
    }

    void Start()
    {
        base.Start();
        LoadCollected();
    }

    public string GetItemName()
    {
        return itemName;
    }

    public virtual string GetItemType()
    {
        string color = string.Empty;
        string type = string.Empty;
        switch (itemType)
        {
            case ItemType.KeyItem:
                color = "#fbf719";
                type = "Key Item";
                break;
            case ItemType.Consumable:
                color = "#d24e01";
                type = "Consumable";
                break;
            case ItemType.MonsterLoot:
                color = "#8a00c2";
                type = "Monster Loot";
                break;
            case ItemType.Armor:
                color = "#000000";
                type = "Armor";
                break;
            case ItemType.SpellBook:
                color = "#AEF359";
                type = "Spell Book";
                break;
        }

        return string.Format("<color={0}>{1}</color>", color, type) + "\n";
    }

    public virtual string GetItemStats1()
    {
        return string.Empty;
    }

    public virtual string GetItemStats2()
    {
        return string.Empty; ;
    }

    public virtual string GetItemStats1Amount()
    {
        return string.Empty; ;
    }

    public virtual string GetItemStats2Amount()
    {
        return string.Empty; ;
    }

    public virtual string GetItemStats1Comparison()
    {
        return string.Empty; ;
    }

    public virtual string GetItemStats2Comparison()
    {
        return string.Empty; ;
    }

    public virtual string GetDiscription()
    {
        /*string color = string.Empty;
        string type = string.Empty;
        switch(itemType)
        {
            case ItemType.KeyItem:
                color = "#fbf719";
                type = "Key Item";
                break;
            case ItemType.Consumable:
                color = "#d24e01";
                type = "Consumable";
                break;
            case ItemType.MonsterLoot:
                color = "#8a00c2";
                type = "Monster Loot";
                break;
            case ItemType.Armor:
                color = "#000000";
                type = "Armor";
                break;
            case ItemType.SpellBook:
                color = "#AEF359";
                type = "Spell Book";
                break;
        }
        if(canSell)
            return string.Format("<b>{0}\n<color={1}>{2}</color></b>\n{3} $", itemName, color, type, baseSellPrice);
        else
            return string.Format("<b>{0}\n<color={1}>{2}</color></b>\n", itemName, color, type);*/
        return "";
    }

    public override void OnTriggerStay2D(Collider2D other)
    {
        amountValue = 1;
        if (other.CompareTag("Player") && other.isTrigger && magnetize)
        {
            if (itemType == ItemType.KeyItem)
                PlayerUI.MyInstance.dungeonGateKeyObtained(this);
            InventoryItemSlots.MyInstance.AddItem(this);
            collected = true;
            if (ItemSpawnManager.MyInstance != null && id != string.Empty)
                SaveCollected();
        }
        base.OnTriggerStay2D(other);
    }

    public void SaveCollected()
    {
        if(ItemSpawnManager.MyInstance.itemSpawn.itemsColected.ContainsKey(id))
        {
            ItemSpawnManager.MyInstance.itemSpawn.itemsColected.Remove(id);
        }
        ItemSpawnManager.MyInstance.itemSpawn.itemsColected.Add(id, collected);
    }

    public void LoadCollected()
    {
        if (ItemSpawnManager.MyInstance != null && id != string.Empty && ItemSpawnManager.MyInstance.itemSpawn.itemsColected.Count > 0)
        {
            ItemSpawnManager.MyInstance.itemSpawn.itemsColected.TryGetValue(id, out collected);
            if (collected)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public virtual bool CanBeUsed()
    {
        return true;
    }
}
