using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EquipSlots : MonoBehaviour, IDropHandler
{
    public Animator animator;
    [SerializeField]
    public Image myItemSprite;
    [SerializeField]
    private InventoryItem invItem;

    [SerializeField]
    public ArmorType armorType;

    [SerializeField]
    private PlayerEquipment playerEquipment;

    private Armor armor;

    public Armor MyArmor
    {
        get
        {
            return armor;
        }
        set
        {
            armor = value;
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

    public PlayerEquipment PlayerEquipment
    {
        get { return playerEquipment; }
    }

    private void Awake()
    {
        PlayerEquipment[] equipments = FindObjectsOfType<PlayerEquipment>();
        foreach (PlayerEquipment eq in equipments)
        {
            if (eq.equipType == armorType)
                playerEquipment = eq;
        }
    }

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Used == Light Gray Background
        if (this.gameObject.GetComponentInChildren<InventoryItem>() == null ||
            this.gameObject.GetComponentInChildren<InventoryItem>().IsEmpty)
            animator.SetBool("Used", false);
        else
            animator.SetBool("Used", true);
    }

    public bool AddItem(Armor armor)
    {
        if (armor == null)
            return false;
        invItem = this.gameObject.GetComponentInChildren<InventoryItem>();
        MyArmor = armor;
        invItem.MyItem = armor;
        invItem.numOfItems++;
        invItem.parent = transform;
        invItem.image.sprite = armor.MySprite;
        invItem.image.color = Color.white;
        playerEquipment.Equip(armor.Animations);
        armor.ChangePlayerStats(false);
        armor.ChangePlayerStats(true);
        if (armor is IStatusEffectable)
        {
            (armor as IStatusEffectable).AddStatusEffects();
        }
        return true;
    }

    public void OnDrop(PointerEventData eventData)
    {
        invItem = this.gameObject.GetComponentInChildren<InventoryItem>();
        if (invItem.shopItem)
            return;
        GameObject dropped = eventData.pointerDrag;
        InventoryItem item = dropped.GetComponent<InventoryItem>();
        // If the Items Armor Type matches that of the Equip Slot
        if (((myItemSprite.sprite != null || item.image.sprite != null) || item.image.sprite != null) && 
            (item.MyItem.itemType == ItemType.Armor && (item.MyItem as Armor).armorType == armorType)
            && !gameObject.GetComponentInChildren<InventoryItem>().shopItem)
        {
            // If this slot is not Empty and another item that isn't already equiped is being equiped 
            // on this slot, reduce this Items buffs from Player Stats first
            if (!item.parent.gameObject.CompareTag("EquipSlot") && !invItem.IsEmpty)
            {
                armor.ChangePlayerStats(false);
                playerEquipment.Dequip();
                // Remove Items Status Effects if it has any
                if (armor is IStatusEffectable)
                    (armor as IStatusEffectable).RemoveStatusEffects();
            }

            if (invItem != null)
            {
                Transform o = transform.GetChild(0);
                o.SetParent(item.parent);
                // If the Item isn't already Equiped, buff Player Stats, otherwise, set the other Equip Slot
                // to be this Item
                if (!item.parent.gameObject.CompareTag("EquipSlot"))
                {
                    (item.MyItem as Armor).ChangePlayerStats(true);
                    // Add Items Status Effects if it has any
                    if (item.MyItem is IStatusEffectable)
                        (item.MyItem as IStatusEffectable).AddStatusEffects();
                }
                else
                {
                    o.parent.gameObject.GetComponent<EquipSlots>().MyArmor =
                        o.gameObject.GetComponentInChildren<InventoryItem>().MyItem as Armor;
                    // Dequip the Item from the other Equip Slot
                    o.parent.gameObject.GetComponent<EquipSlots>().playerEquipment.Dequip();
                }
            }
            item.parent = transform;
            armor = item.MyItem as Armor;
            playerEquipment.Equip((item.MyItem as Armor).Animations);
        }
    }
}
