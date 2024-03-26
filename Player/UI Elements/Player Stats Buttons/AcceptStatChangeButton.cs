using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcceptStatChangeButton : StatButton
{
    public override void OnClickButton()
    {
        Player.MyInstance.AcceptUpgrades();
    }
}
