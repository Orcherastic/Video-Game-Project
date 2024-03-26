using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenceUpStatusEffect : StatusEffect
{
    float speedDecreaseValue, defenceDecreaseValue, fireDefenceDecreaseValue, iceDefenceDecreaseValue, lightningDefenceDecreaseValue;

    public override string GetDiscription()
    {
        return base.GetDiscription() + "\n" + "Increased Melee and Magic Defence\n" + timeText;
    }

    public override void OnEffect()
    {
        float defence = Player.MyInstance.physicalDefence;
        float fireDefence = Player.MyInstance.fireDefence;
        float iceDefence = Player.MyInstance.iceDefence;
        float lightningDefence = Player.MyInstance.lightningDefence;
        // Defence
        defenceDecreaseValue = (defence / 100) * 30;
        Player.MyInstance.physicalDefence -= Mathf.RoundToInt(defenceDecreaseValue);
        // Fire Defence
        fireDefenceDecreaseValue = (fireDefence / 100) * 30;
        Player.MyInstance.fireDefence -= Mathf.RoundToInt(fireDefenceDecreaseValue);
        // Ice Defence
        iceDefenceDecreaseValue = (iceDefence / 100) * 30;
        Player.MyInstance.iceDefence -= Mathf.RoundToInt(iceDefenceDecreaseValue);
        // Lightning Defence
        lightningDefenceDecreaseValue = (lightningDefence / 100) * 30;
        Player.MyInstance.lightningDefence -= Mathf.RoundToInt(lightningDefenceDecreaseValue);
    }

    public override void OffEffect()
    {
        Player.MyInstance.physicalDefence += Mathf.RoundToInt(defenceDecreaseValue);
        Player.MyInstance.fireDefence += Mathf.RoundToInt(fireDefenceDecreaseValue);
        Player.MyInstance.iceDefence += Mathf.RoundToInt(iceDefenceDecreaseValue);
        Player.MyInstance.lightningDefence += Mathf.RoundToInt(lightningDefenceDecreaseValue);
    }
}
