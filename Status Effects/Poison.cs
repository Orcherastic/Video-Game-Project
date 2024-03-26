using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : StatusEffect
{
    private int increment;
    private float time = 0f;

    public override string GetDiscription()
    {
        return base.GetDiscription() + "\n" + "Slowly Decreases Health \n" + timeText;
    }

    void Update()
    {
        time += Time.deltaTime;
        if(time >= Random.Range(4f + (Player.MyInstance.poisonResistance / 10), 8f + (Player.MyInstance.poisonResistance / 10)))
        {
            time = 0;
            if (player)
                Player.MyInstance.Knock(0, increment * numberOf, -1);
            else
                transform.root.gameObject.GetComponent<Enemy>().takeDamage(increment * numberOf, 0, true);
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

        increment = Mathf.RoundToInt((maxHP / 100 * 5)); //=== FORMULA TO CHANGE ===\\\
        if (increment == 0)
            increment = 1;
    }

    public override void OffEffect()
    {
        
    }
}
