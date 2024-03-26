using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMagicUseSlots : MonoBehaviour
{
    private static InventoryMagicUseSlots instance;

    public static InventoryMagicUseSlots MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InventoryMagicUseSlots>();
            }
            return instance;
        }
    }

    [SerializeField]
    private GameObject slotPrefab;
    public Sprite emptyMagicUseSlot;
    public Sprite activeMagicUseSlot;

    public List<MagicUseSlot> slots = new List<MagicUseSlot>();
    public List<Sprite> slotNumbers = new List<Sprite>();

    void Awake()
    {
        AddSlots(4);
    }

    void Start()
    {

    }
    void Update()
    {
        if (!Player.MyInstance.openInventory)
        {
            // Inventory is not active == Magic Use Slots are down and left on the screen
            for (int i = 0; i < slots.Count; i++)
            {
                slots[i].transform.SetParent(InGameMagicUseSlots.myInstance.transform);
                // Activate small images
                slots[i].slotNumberSmall.color = Color.white;
                slots[i].myMagicSpriteSmall.color = Color.white;
                // Swap big with small images
                slots[i].slotNumberSmall.sprite = slots[i].slotNumberBig.sprite;
                if (slots[i].myMagicSpriteBig.sprite != null)
                    slots[i].myMagicSpriteSmall.sprite = slots[i].myMagicSpriteBig.sprite;
                else
                    slots[i].myMagicSpriteSmall.color = Color.clear;
                // Deactivate big images
                slots[i].slotNumberBig.color = Color.clear;
                slots[i].myMagicSpriteBig.color = Color.clear;
            }
        }
        else
        {
            // Inventory is Active == Magic Use Slots are in the Inventory
            for (int i = 0; i < slots.Count; i++)
            {
                slots[i].transform.SetParent(transform);
                // Activate big images
                slots[i].slotNumberBig.color = Color.white;
                if(slots[i].myMagicSpriteBig.sprite != null)
                    slots[i].myMagicSpriteBig.color = Color.white;
                else
                    slots[i].myMagicSpriteBig.color = Color.clear;
                // Swap small with big images
                slots[i].slotNumberBig.sprite = slots[i].slotNumberSmall.sprite;
                //if (slots[i].myMagicSpriteSmall.sprite != null)
                    //slots[i].myMagicSpriteBig.sprite = slots[i].myMagicSpriteSmall.sprite;
                // Deactivate small images
                slots[i].slotNumberSmall.color = Color.clear;
                slots[i].myMagicSpriteSmall.color = Color.clear;
            }
        }

        foreach (MagicUseSlot slot in slots)
        {
            if (slot.myMagicSpriteBig.sprite != null)
            {
                slot.background.sprite = activeMagicUseSlot;
            }
            else
            {
                slot.background.sprite = emptyMagicUseSlot;
            }

        }
    }

    public void AddSlots(int slotCount)
    {
        for (int i = 0; i < slotCount; i++)
        {
            MagicUseSlot slot = Instantiate(slotPrefab, transform).GetComponent<MagicUseSlot>();
            slots.Add(slot);
            slots[i].slotNumberBig.sprite = slotNumbers[i];
            slots[i].useNumber = (i+1).ToString();
            slots[i].cooldownSmall.fillAmount = 0;
            slots[i].cooldownBig.fillAmount = 0;
        }
    }

    public bool AddMagicToSlot(Sprite sprite, string magicName)
    {
        for(int i=0; i<slots.Count; i++)
        {
            // If the spell is already in a Magic Use Slot, move it to the next one and remove it from this one
            if (slots[i].myMagicSpriteBig.sprite == sprite)
            {
                slots[i].myMagicSpriteBig.sprite = null;
                slots[i].mySpellName = string.Empty;
                int j = i;
                i++;
                if (i >= slots.Count)
                    i = 0;
                while (slots[i].isCooldown)
                {
                    i++;
                    if (i >= slots.Count)
                        i = 0;
                }
                if(slots[i].myMagicSpriteBig != null)
                {
                    foreach (MagicSlot slot in InventoryMagicSlots.MyInstance.slots)
                    {
                        if (slot.myMagicSprite.sprite == slots[i].myMagicSpriteBig.sprite)
                            slot.selected = false;
                    }
                }
                slots[i].myMagicSpriteBig.sprite = sprite;
                slots[i].mySpellName = magicName;
                slots[j].SwitchCooldown(slots[i]);
                return true;
            }
        }
        for (int i = 0; i < slots.Count; i++)
        {
            // If the Magic Use Slot is empty, add the spell to this slot
            if (slots[i].myMagicSpriteBig.sprite == null)
            {
                slots[i].myMagicSpriteBig.sprite = sprite;
                slots[i].mySpellName = magicName;
                return true;
            }
        }

        return false;
    }

    public void AddMagicToSpecificSlot(Sprite sprite, string magicName, int slotNumber)
    {
        slots[slotNumber].myMagicSpriteBig.sprite = sprite;
        slots[slotNumber].mySpellName = magicName;
    }
}
