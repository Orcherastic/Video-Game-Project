using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryQuickUseSlots : MonoBehaviour
{
    private static InventoryQuickUseSlots instance;

    public static InventoryQuickUseSlots MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InventoryQuickUseSlots>();
            }
            return instance;
        }
    }

    [SerializeField]
    private GameObject slotPrefab;

    public List<QuickUseSlot> slots = new List<QuickUseSlot>();
    public QuickUseSlot slotWithUsedItem;

    private void Awake()
    {
        AddSlots(3);
    }

    void Start()
    {
        
    }

    public void AddSlots(int slotCount)
    {
        for (int i = 0; i < slotCount; i++)
        {
            QuickUseSlot slot = Instantiate(slotPrefab, transform).GetComponent<QuickUseSlot>();
            slots.Add(slot);
        }
    }

    void Update()
    {
        if(!Player.MyInstance.openInventory)
        {
            // Inventory is not active == Quick Slots are up and center on the Screen
            for(int i=0; i<slots.Count; i++)
            {
                slots[i].transform.SetParent(InGameQuickUseSlots.myInstance.transform);
                // Quick Slots are smaller than the ones in the Inventory
                slots[i].animator.SetBool("Shrinked", true);
            }
            // Selected Quick Slot
            UpdateSelectedSlot();
            if (Input.GetAxis("Mouse ScrollWheel") > 0 /*|| Input.GetKeyDown("right")*/)
            {
                NextSelectedSlot(true);
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0 /*|| Input.GetKeyDown("left")*/)
            {
                NextSelectedSlot(false);
            }
            // Use Item
            if(Input.GetButtonDown("Use Item") && 
                (Player.MyInstance.currentState == PlayerState.run || Player.MyInstance.currentState == PlayerState.walk))
            {
                for(int i=0; i<slots.Count; i++)
                {
                    if(slots[i].animator.GetBool("Selected") == true)
                    {
                        slots[i].UseItem();
                    }
                }
            }
        }
        else
        {
            // Inventory is Active == Quick Slots are in the Inventory
            for (int i = 0; i < slots.Count; i++)
            {
                slots[i].transform.SetParent(transform);
                // There is no Slot Selection, Quick Slots are bigger then when outside of the Inventory
                slots[i].animator.SetBool("Shrinked", false);
                slots[i].animator.SetBool("Selected", false);
            }
        }
    }

    public void UpdateSelectedSlot()
    {
        // Only one Quick Slot can be Selected, otherwise all Quick Slots are deselected
        bool skip = false;
        for (int i=0; i<slots.Count; i++)
        {
            // If a Quick Slot has an Item, make it Selected and skip the others
            if (slots[i].animator.GetBool("Selected") == true)
                skip = true;
        }
        if(!skip)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                // Deselect an Empty Quick Slot
                if (slots[i].gameObject.GetComponentInChildren<InventoryItem>() == null ||
                    slots[i].gameObject.GetComponentInChildren<InventoryItem>().IsEmpty)
                    slots[i].animator.SetBool("Selected", false);
                else
                {
                    // Select the Quick Slot
                    slots[i].animator.SetBool("Selected", true);
                    break;
                }
            }
        }
    }

    public void NextSelectedSlot(bool next)
    {
        // next == right, !next == left
        for (int i = 0; i < slots.Count; i++)
        {
            if(slots[i].animator.GetBool("Selected") == true)
            {
                QuickUseSlot temp = slots[i];
                if(next)
                {
                    // Right
                    i += 1;
                    if (i > slots.Count - 1)
                        i = 0;
                }
                else
                {
                    // Left
                    i -= 1;
                    if (i < 0)
                        i = slots.Count - 1;
                }

                if(!slots[i].gameObject.GetComponentInChildren<InventoryItem>().IsEmpty)
                {
                    temp.animator.SetBool("Selected", false);
                    slots[i].animator.SetBool("Selected", true);
                    break;
                }
                else
                {
                    if (next)
                    {
                        i += 1;
                        if (i > slots.Count - 1)
                            i = 0;
                    }
                    else
                    {
                        i -= 1;
                        if (i < 0)
                            i = slots.Count - 1;
                    }
                    if (!slots[i].gameObject.GetComponentInChildren<InventoryItem>().IsEmpty)
                    {
                        temp.animator.SetBool("Selected", false);
                        slots[i].animator.SetBool("Selected", true);
                        break;
                    }
                }
                break;
            }
        }
    }
}
