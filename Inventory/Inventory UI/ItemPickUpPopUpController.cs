using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUpPopUpController : MonoBehaviour
{
    private static ItemPickUpPopUpController instance;

    public static ItemPickUpPopUpController MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ItemPickUpPopUpController>();
            }
            return instance;
        }
    }

    public GameObject Instance;
    public List<ItemPickUpPopUp> Instances = new List<ItemPickUpPopUp>();

    public void CreateInstance(Item item)
    {
        if (item == null)
        {
            if (Instances.Count != 0)
            {
                foreach (ItemPickUpPopUp inst in Instances)
                {
                    if (inst.itemName.text == "Inventory Is Full!")
                    {
                        return;
                    }
                }
            }
        }
        else
        {
            if (Instances.Count != 0)
            {
                foreach (ItemPickUpPopUp inst in Instances)
                {
                    if (inst.itemName.text == item.itemName)
                    {
                        inst.UpdateNumOfItems();
                        return;
                    }
                }
            }
        }

        //Instance.numOfItems = 0;
        //Instance.UpdateInstance(item);
        ItemPickUpPopUp instance = Instantiate(Instance, transform).GetComponent<ItemPickUpPopUp>();
        instance.numOfItems = 0;
        instance.UpdateInstance(item);
        Instances.Add(instance);
    }
}
