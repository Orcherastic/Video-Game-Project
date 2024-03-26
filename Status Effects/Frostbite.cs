using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frostbite : StatusEffect
{
    Enemy enemy;
    float speedDecreaseValue, defenceDecreaseValue, fireDefenceDecreaseValue, iceDefenceDecreaseValue, lightningDefenceDecreaseValue;

    public override string GetDiscription()
    {
        return base.GetDiscription() + "\n" + "Decreased Melee and Magic Defence.\nSlightly decreased Movement Speed\n" + timeText;
    }

    public override void OnEffect()
    {
        if (transform.parent.gameObject.CompareTag("Player Status Effects"))
        {
            player = true;
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
            // Movement Speed
            speedDecreaseValue = (Player.MyInstance.playerSpeed / 100) * 15;
            Player.MyInstance.playerSpeed -= speedDecreaseValue;
        }
        else if (transform.parent.gameObject.CompareTag("Enemy Status Effects"))
        {
            player = false;
            enemy = transform.root.GetComponent<Enemy>();
            // Defence
            defenceDecreaseValue = (enemy.defence / 100) * 30;
            enemy.defence -= Mathf.RoundToInt(defenceDecreaseValue);
            // Fire Defence
            fireDefenceDecreaseValue = (enemy.fireDefence / 100) * 30;
            Player.MyInstance.fireDefence -= Mathf.RoundToInt(fireDefenceDecreaseValue);
            // Ice Defence
            iceDefenceDecreaseValue = (enemy.iceDefence / 100) * 30;
            Player.MyInstance.iceDefence -= Mathf.RoundToInt(iceDefenceDecreaseValue);
            // Lightning Defence
            lightningDefenceDecreaseValue = (enemy.lightningDefence / 100) * 30;
            Player.MyInstance.lightningDefence -= Mathf.RoundToInt(lightningDefenceDecreaseValue);
            // Movement Speed
            speedDecreaseValue = (enemy.chaseSpeed / 100) * 10;
            enemy.chaseSpeed -= speedDecreaseValue;
        }
    }

    public override void OffEffect()
    {
        if (player)
        {
            Player.MyInstance.physicalDefence += Mathf.RoundToInt(defenceDecreaseValue);
            Player.MyInstance.fireDefence += Mathf.RoundToInt(fireDefenceDecreaseValue);
            Player.MyInstance.iceDefence += Mathf.RoundToInt(iceDefenceDecreaseValue);
            Player.MyInstance.lightningDefence += Mathf.RoundToInt(lightningDefenceDecreaseValue);
            Player.MyInstance.playerSpeed += speedDecreaseValue;
        }
        else
        {
            enemy = transform.root.GetComponent<Enemy>();
            enemy.defence += Mathf.RoundToInt(defenceDecreaseValue);
            enemy.fireDefence += Mathf.RoundToInt(fireDefenceDecreaseValue);
            enemy.iceDefence += Mathf.RoundToInt(iceDefenceDecreaseValue);
            enemy.lightningDefence += Mathf.RoundToInt(lightningDefenceDecreaseValue);
            enemy.chaseSpeed += speedDecreaseValue;
        }
    }
}
