using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPouch : PickUp
{
    public override void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.isTrigger && magnetize)
        {
            player.GetComponent<Player>().GainCoins(amountValue);
        }
        base.OnTriggerStay2D(other);
    }
}
