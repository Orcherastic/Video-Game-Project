using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burn : StatusEffect
{
    private int increment;
    private float time = 0f;

    public override string GetDiscription()
    {
        return base.GetDiscription() + "\n" + "Slowly Decreases Health,\nbut causes Staggering \n" + timeText;
    }

    void Update()
    {
        time += Time.deltaTime;
        if (time >= Random.Range(8f + (Player.MyInstance.fireDefence / 10), 12f + (Player.MyInstance.fireDefence / 10)))
        {
            time = 0;
            if (player)
                Player.MyInstance.Knock(0.8f, increment * numberOf, 100);
            else
                transform.root.gameObject.GetComponent<Enemy>()
                    .Knock(transform.root.gameObject.GetComponent<Enemy>().GetComponent<Rigidbody2D>(), 2, increment * numberOf, 100, true);
        }
    }

    public override void OnEffect()
    {
        int maxHP = 0;

        if (transform.parent.gameObject.CompareTag("Player Status Effects"))
        {
            player = true;
            maxHP = Player.MyInstance.maxHealth;
        }
        else if (transform.parent.gameObject.CompareTag("Enemy Status Effects"))
        {
            player = false;
            maxHP = transform.root.gameObject.GetComponent<Enemy>().maxHealth;
        }

        increment = Mathf.RoundToInt((maxHP / 100 * 10)); //=== FORMULA TO CHANGE ===\\\
        if (increment == 0)
            increment = 1;
    }

    public override void OffEffect()
    {

    }
}
