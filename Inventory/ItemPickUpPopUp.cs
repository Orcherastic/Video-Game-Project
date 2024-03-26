using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPickUpPopUp : MonoBehaviour
{
    public Image itemSprite;
    public Text itemName;
    public Text numOfItemsText;
    public int numOfItems;

    public float lifespan = 3;

    private void Update()
    {
        lifespan -= 0.01f;
        if (lifespan <= 0)
        {
            ItemPickUpPopUpController.MyInstance.Instances.Remove(this);
            Destroy(this.gameObject);
        }
    }

    public void UpdateInstance(Item item)
    {
        if(item == null)
        {
            itemSprite.sprite = null;
            itemSprite.color = Color.gray;
            itemName.text = "Inventory Is Full!";
            numOfItemsText.gameObject.SetActive(false);
        }
        else
        {
            itemSprite.sprite = item.MySprite;
            itemName.text = item.itemName;
            numOfItems++;
            numOfItemsText.text = numOfItems.ToString();
        }
    }

    public void UpdateNumOfItems()
    {
        lifespan = 3;
        numOfItems++;
        numOfItemsText.text = numOfItems.ToString();
    }
}
