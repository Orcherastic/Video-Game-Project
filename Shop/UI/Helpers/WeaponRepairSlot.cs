using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WeaponRepairSlot : MonoBehaviour, IDropHandler
{
    private static WeaponRepairSlot instance;
    public static WeaponRepairSlot MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<WeaponRepairSlot>();
            }
            return instance;
        }
    }

    [SerializeField]
    private Image myItemSprite;
    [SerializeField]
    private InventoryItem invItem;
    public Item myItem;

    public Text weaponRepairCostText;

    public bool IsEmpty
    {
        get
        {
            return this.gameObject.GetComponentInChildren<InventoryItem>().numOfItems == 0;
        }
    }

    void Update()
    {
        if (gameObject.GetComponentInChildren<InventoryItem>() != null)
        {
            if (gameObject.GetComponentInChildren<InventoryItem>().MyItem != null)
            {
                Player.MyInstance.weaponRepairing = true;
            }

            else
            {
                Player.MyInstance.weaponRepairing = false;
            }

        }
    }

    public void CalculateRepairCost(Weapon brokenWeapon)
    {
        int cost = 0;
        if (brokenWeapon.HP > 0 && brokenWeapon.HP < brokenWeapon.maxHP)
        {
            cost = (brokenWeapon.maxHP - brokenWeapon.HP) * (brokenWeapon.level + 1);
        }
        else
        {
            cost = brokenWeapon.maxHP*2 * (brokenWeapon.level + 1);
        }
        weaponRepairCostText.text = cost.ToString();
    }

    public void RepairWeapon()
    {
        if ((myItem as Weapon).HP == (myItem as Weapon).maxHP)
        {
            Debug.Log("Weapon does not need repairing!");
        }
        else
        {
            if (Player.MyInstance.coins >= int.Parse(weaponRepairCostText.text))
            {
                Player.MyInstance.coins -= int.Parse(weaponRepairCostText.text);
                Player.MyInstance.ui.SetCoins(Player.MyInstance.coins);
                CombatTextManager.MyInstance.CreateText(CoinTextPopUpHolder.MyInstance.transform.position, weaponRepairCostText.text, TextType.loseCoin);
                (myItem as Weapon).HP = (myItem as Weapon).maxHP;
                weaponRepairCostText.text = "0";
            }
            else
                Debug.Log("Not Enough Coin!");
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        invItem = this.gameObject.GetComponentInChildren<InventoryItem>();
        GameObject dropped = eventData.pointerDrag;
        InventoryItem item = dropped.GetComponent<InventoryItem>();
        if ((myItemSprite.sprite != null && item.image.sprite != null) || item.image.sprite != null &&
        (item.MyItem is Weapon))
        {
            if (invItem != null)
            {
                Transform o = transform.GetChild(0);
                o.SetParent(item.parent);
                item.parent = transform;
                myItem = item.MyItem;
                CalculateRepairCost(myItem as Weapon);
            }
            item.parent = transform;
            myItem = item.MyItem;
        }

    }
}
