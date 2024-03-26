using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairWeaponButton : StatButton
{
    public override void OnClickButton()
    {
        WeaponRepairSlot.MyInstance.RepairWeapon();
    }
}
