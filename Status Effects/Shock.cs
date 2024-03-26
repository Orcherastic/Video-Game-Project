using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shock : StatusEffect
{
    float speed;
    Enemy enemy;
    public override string GetDiscription()
    {
        return base.GetDiscription() + "\n" + "Unables any movement\n" + timeText;
    }

    void Update()
    {
        if(numberOf > 1)
        {
            numberOf = 0;
            timeRemaining = 0.1f;
        }
    }

    public override void OnEffect()
    {
        if (transform.parent.gameObject.CompareTag("Player Status Effects"))
        {
            player = true;
            speed = Player.MyInstance.playerSpeed;
            Player.MyInstance.playerSpeed -= speed;
            
        }
        else if (transform.parent.gameObject.CompareTag("Enemy Status Effects"))
        {
            player = false; 
            enemy = transform.root.GetComponent<Enemy>();
            speed = enemy.chaseSpeed;
            enemy.chaseSpeed -= speed;
        }
    }

    public override void OffEffect()
    {
        if (player)
        {
            Player.MyInstance.playerSpeed += speed;
            Player.MyInstance.currentState = PlayerState.run;
        }
        else
        {
            enemy.chaseSpeed += speed;
        }
    }
}
