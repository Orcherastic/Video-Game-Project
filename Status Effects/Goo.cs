using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goo : StatusEffect
{
    float speedDecreaseValue;
    public override string GetDiscription()
    {
        return base.GetDiscription() + "\n" + "Speed Decreased \n" + timeText;
    }

    public override void OnEffect()
    {
        speedDecreaseValue = (Player.MyInstance.playerSpeed / 100) * 50;
        Player.MyInstance.playerSpeed -= speedDecreaseValue;
    }

    public override void OffEffect()
    {
        Player.MyInstance.playerSpeed += speedDecreaseValue;
    }
}
