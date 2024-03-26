using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaOrb : PickUp
{
    public override void OnTriggerStay2D(Collider2D other)
    {
        amountValue = 1;
        if (other.CompareTag("Player") && other.isTrigger && magnetize)
        {
            player.GetComponent<Player>().GainMana(amountValue);
        }
        base.OnTriggerStay2D(other);
    }
}
