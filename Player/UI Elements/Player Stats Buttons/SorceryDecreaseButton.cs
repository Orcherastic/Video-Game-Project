using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SorceryDecreaseButton : StatButton
{
    public override void OnClickButton()
    {
        Player.MyInstance.DowngradeStat("sorcery");
    }
}
