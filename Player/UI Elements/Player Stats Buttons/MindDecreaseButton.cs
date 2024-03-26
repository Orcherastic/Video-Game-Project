using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MindDecreaseButton : StatButton
{
    public override void OnClickButton()
    {
        Player.MyInstance.DowngradeStat("mind");
    }
}
