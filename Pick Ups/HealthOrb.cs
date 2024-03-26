using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthOrb : PickUp
{
    public override void OnTriggerStay2D(Collider2D other)
    {
        amountValue = 2;
        if (other.CompareTag("Player") && other.isTrigger && magnetize)
        {
            player.GetComponent<Player>().HealHealth(amountValue);
        }
        base.OnTriggerStay2D(other);
    }
}
