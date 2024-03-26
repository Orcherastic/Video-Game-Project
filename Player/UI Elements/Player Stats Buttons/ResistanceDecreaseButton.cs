using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResistanceDecreaseButton : StatButton
{
    public override void OnClickButton()
    {
        Player.MyInstance.DowngradeStat("resistance");
    }
}
