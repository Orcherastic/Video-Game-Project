using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    void Update()
    {
        if (health < maxHealth)
            canvas.gameObject.SetActive(true);

        if (currentState != EnemyStates.death || currentState != EnemyStates.wait)
            CheckDistance();
        else
        {
            animator.SetBool("isChasing", false);
            chaseSpeed = 0;
        }

        if (playerHit)
        {
            OnPlayerHit();
            playerHit = false;
        }
    }

    public override void CheckDistance()
    {
        if ((Vector3.Distance(target.position, transform.position) <= chaseRadius && currentState != EnemyStates.stagger) || canvas.isActiveAndEnabled)
        {
            if (currentState == EnemyStates.idle || currentState == EnemyStates.chase)
            {
                // Calculate direction to the player
                Vector2 directionToPlayer = target.position - transform.position;

                // Check for obstacles in the path
                RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer.normalized, avoidanceDistance, obstacleLayer);

                // If there's an obstacle, adjust the direction
                if (hit.collider != null && !hit.collider.CompareTag("Enemy"))
                {
                    // Calculate perpendicular direction to the obstacle
                    Vector2 avoidanceDirection = new Vector2(-hit.normal.y, hit.normal.x);

                    // Normalize the resulting direction
                    directionToPlayer += avoidanceDirection * avoidanceDistance;
                }

                // Move towards the player
                transform.Translate(directionToPlayer.normalized * chaseSpeed * Time.deltaTime);

                currentState = EnemyStates.chase;
                animator.SetBool("isChasing", true);
            }
        }
        else if (Vector3.Distance(target.position, transform.position) > chaseRadius)
        {
            animator.SetBool("isChasing", false);
            currentState = EnemyStates.idle;
            rb.velocity = Vector2.zero;
        }
    }

/*    public override void takeDamage(int damage, int poiseDamage, bool isPlayerSpell)
    {
        base.takeDamage(damage, poiseDamage, isPlayerSpell);
        ui.SetHealth(health);
        if(health <= 0)
        {
            Player.MyInstance.GainXP(5);
            if(!isPlayerSpell)
                Player.MyInstance.GainWeaponXP(2);
            Player.MyInstance.counter = 0f;
        }
    }*/

    public override void Knock(Rigidbody2D myRB, float knockTime, int damage, int poiseDamage, bool isPlayerSpell)
    {
        //animator.SetTrigger("Staggering");
        base.Knock(myRB, knockTime, damage, poiseDamage, isPlayerSpell);
        ui.SetHealth(health);
    }

    public override void OnPlayerHit()
    {
        StartCoroutine(WaitCo());
        int gooProb = Random.Range(0, 101);
        if (gooProb <= 10)
        {
            //Goo
            Player.MyInstance.statusEffects.AddStatusEffectWithTimer(11, 10f);
        }
    }

    private IEnumerator WaitCo()
    {
        chaseSpeed = 0f;
        yield return new WaitForSeconds(0.8f);
        chaseSpeed = 1.5f;
    }
}
