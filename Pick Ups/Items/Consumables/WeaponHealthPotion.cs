using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHealthPotion : Item, IUsable
{
    public override string GetDiscription()
    {
        return base.GetDiscription() + 
            "Completely restores Weapon Health.\n" +
            "Cannot repair a broken weapon.";
    }

    public override bool CanBeUsed()
    {
        if (Player.MyInstance.equipedWeapon && !Player.MyInstance.equipedWeapon.weaponBroken)
            return true;
        else
            return false;
    }

    public void TakeItem()
    {
        Player.MyInstance.UseItem("repairWeapon", this);
    }

    public void UseItem()
    {
        Player.MyInstance.RepairWeapon();
    }
}
