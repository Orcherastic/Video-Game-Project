using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SorceryIncreaseButton : StatButton
{
    public override void OnClickButton()
    {
        Player.MyInstance.UpgradeStat("sorcery");
    }
}
