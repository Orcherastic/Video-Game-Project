using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBuildUpLayout : MonoBehaviour
{
    private static WeaponBuildUpLayout instance;

    public static WeaponBuildUpLayout MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<WeaponBuildUpLayout>();
            }
            return instance;
        }
    }

    public GameObject weaponSlotHolder;
    public WeaponBuildUpSlot weaponSlotPrefab;
    public List<WeaponBuildUpSlot> slots = new List<WeaponBuildUpSlot>();

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        PlayerUI.MyInstance.buildUpActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!Player.MyInstance.equipedWeapon || !Player.MyInstance.openInventory)
        {
            PlayerUI.MyInstance.ResetWeaponStatsColor();
            PlayerUI.MyInstance.buildUpActive = false;
            gameObject.SetActive(false);
        }
    }

    public void RemoveWeaponBuildUpSlots()
    {
        foreach(WeaponBuildUpSlot slot in slots)
        {
            Destroy(slot.gameObject);
        }
        slots.Clear();
    }

    public void AddWeaponBuildUpSlots()
    {
        foreach(Weapon weapon in Player.MyInstance.equipedWeapon.buildUpWeaponOptions)
        {
            WeaponBuildUpSlot slot = Instantiate(weaponSlotPrefab, weaponSlotHolder.transform);
            slot.weapon = weapon;
            slot.weaponSprite.sprite = weapon.MySprite;
/*            if(PlayerUI.MyInstance.CompareWithEquipedWeapon(slot.weapon))
                slot.weaponSprite.color = Color.white;
            else
                slot.weaponSprite.color = Color.black;*/
            slots.Add(slot);
        }
    }

    public void SetWeaponSlotsSprites(WeaponBuildUpSlot selectedSlot)
    {
        if (slots.Count <= 1)
            return;
        else
        {
            for(int i=0; i<slots.Count; i++)
            {
                if (slots[i] != selectedSlot)
                    slots[i].thisImage.sprite = slots[i].idleSprite;
            }
        }
    }
}
