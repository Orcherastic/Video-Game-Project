using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryMagicSlots : MonoBehaviour
{
    private static InventoryMagicSlots instance;

    public static InventoryMagicSlots MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InventoryMagicSlots>();
            }
            return instance;
        }
    }

    [SerializeField]
    private GameObject slotPrefab;
    public Sprite emptyMagicSlot;
    public Sprite unlockedMagicSlot;
    public Sprite selectedMagicSlot;

    public List<MagicSlot> slots = new List<MagicSlot>();

    public List<Sprite> magicSprites = new List<Sprite>();
    public List<string> magicNames = new List<string>();
    public List<string> magicDescriptions = new List<string>();
    private int numOfSlots = 18;

    void Awake()
    {
        magicNames.Add("Heavy Strike");
        magicNames.Add("Fireball");
        magicNames.Add("Ice Shards");
        magicNames.Add("Lightning Bolt");
        magicNames.Add("Poisonous Touch");
        magicNames.Add("Defence Up");

/*        magicNames.Add("Wave Slash");
        magicNames.Add("Blaze");
        magicNames.Add("Ice Wall");
        magicNames.Add("Lightning Ray");
        magicNames.Add("Decay");
        magicNames.Add("Heal");

        magicNames.Add("Spinning Attack");
        magicNames.Add("Scorch");
        magicNames.Add("Frostbite");
        magicNames.Add("Thunder Storm");
        magicNames.Add("Petrify");
        magicNames.Add("Cure");*/

        magicDescriptions.Add("Strong attack that deals\nbig damage to a target.");
        magicDescriptions.Add("Throw condensed fire in the shape of a ball\nthat deals fire damage to a target.\nSmall chance of inflicting burn.\n\nCosts 2MP");
        magicDescriptions.Add("Throw many small ice shards\nthat deal ice damage to a target.\nSmall chance of inflicting frostbite.\n\nCosts 3MP");
        magicDescriptions.Add("Hit a target with a single lightning bolt\nand deal lightning damage.\nSmall chance of inflicting shock.\n\nCosts 4MP");
        magicDescriptions.Add("Throw a ball of poison\nthat spreads and inflicts poison to targets.\n\nCosts 4MP");
        magicDescriptions.Add("Boost defence against melee,\naswell against magic attacks for a time.\n\nCosts 5MP");

        AddSlots();
    }

    void Start()
    {
    }

    void Update()
    {
        foreach(MagicSlot slot in slots)
        {
            if (slot.unlocked && !slot.selected)
                slot.background.sprite = unlockedMagicSlot;
            else if (slot.selected)
                slot.background.sprite = selectedMagicSlot;
            else
                slot.background.sprite = emptyMagicSlot;
        }
    }

    public void AddSlots()
    {
        for (int i = 0; i < numOfSlots; i++)
        {
            MagicSlot slot = Instantiate(slotPrefab, transform).GetComponent<MagicSlot>();
            slots.Add(slot);
            if (i<magicSprites.Count)
            {
                //slots[i].myMagicSprite.color = Color.white;
                slots[i].myMagicSprite.color = Color.clear;
                slots[i].myMagicSprite.sprite = magicSprites[i];
                slots[i].myMagicName = magicNames[i];
                slots[i].myMagicDescription = magicDescriptions[i];
            }
            //else
                //slots[i].myMagicSprite.color = Color.clear;
        }
    }
}
