using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MindIncreaseButton : StatButton
{
    public override void OnClickButton()
    {
        Player.MyInstance.UpgradeStat("mind");
    }
}
