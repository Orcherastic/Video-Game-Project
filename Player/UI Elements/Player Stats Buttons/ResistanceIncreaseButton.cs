using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResistanceIncreaseButton : StatButton
{
    public override void OnClickButton()
    {
        Player.MyInstance.UpgradeStat("resistance");
    }
}
