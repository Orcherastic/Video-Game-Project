using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritDecreaseButton : StatButton
{
    public override void OnClickButton()
    {
        Player.MyInstance.DowngradeStat("spirit");
    }
}
