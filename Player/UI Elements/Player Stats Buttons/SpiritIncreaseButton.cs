using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpiritIncreaseButton : StatButton
{
    public override void OnClickButton()
    {
        Player.MyInstance.UpgradeStat("spirit");
    }
}
