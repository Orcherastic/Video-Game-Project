using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat : Enemy
{
    private float standingAttackRadius = 1f;
    bool colliding = false;
    void Update()
    {
        if (health < maxHealth)
            canvas.gameObject.SetActive(true);

        if (currentState != EnemyStates.death || currentState != EnemyStates.wait)
            CheckDistance();
        else
        {
            if (currentState != EnemyStates.death)
                animator.SetBool("isDead", true);
            animator.SetBool("isChasing", false);
            chaseSpeed = 0;
        }

        if (currentState != EnemyStates.stagger && !colliding)
            rb.bodyType = RigidbodyType2D.Kinematic;
        else
            rb.bodyType = RigidbodyType2D.Dynamic;
    }

    public override void CheckDistance()
    {
        if ((Vector3.Distance(target.position, transform.position) <= chaseRadius &&
        (Vector3.Distance(target.position, transform.position) > attackRadius)))
        {
            if (currentState == EnemyStates.idle || currentState == EnemyStates.chase)
            {
                Vector3 temp = Vector3.MoveTowards(transform.position, target.position, chaseSpeed * Time.deltaTime);
                rb.MovePosition(temp);
                changeAnim(-(temp - target.position));
                currentState = EnemyStates.chase;
                animator.SetBool("Retreat", true);
            }
        }
        else if (Vector3.Distance(target.position, transform.position) <= attackRadius &&
            Vector3.Distance(target.position, transform.position) > standingAttackRadius)
        {
            if (currentState == EnemyStates.idle || currentState == EnemyStates.chase)
            {
                animator.SetBool("Retreat", false);
                chaseSpeed = 0;
                Vector3 temp = Vector3.MoveTowards(transform.position, -target.position, chaseSpeed * Time.deltaTime);
                changeAnim(-(temp - target.position));
                StopAllCoroutines();
                StartCoroutine(AttackCo());
            }
        }
        else if (Vector3.Distance(target.position, transform.position) < standingAttackRadius)
        {
            if (currentState == EnemyStates.idle || currentState == EnemyStates.chase)
            {
                animator.SetBool("Retreat", false);
                chaseSpeed = 0;
                Vector3 temp = Vector3.MoveTowards(transform.position, -target.position, chaseSpeed * Time.deltaTime);
                changeAnim(-(temp - target.position));
                StopAllCoroutines();
                StartCoroutine(AttackCloseCo());
            }
        }
        else if (Vector3.Distance(target.position, transform.position) > chaseRadius)
        {
            animator.SetBool("Retreat", false);
            currentState = EnemyStates.idle;
            rb.velocity = Vector2.zero;
        }
        
    }

    private void setAnimFloat(Vector2 setVector)
    {
        animator.SetFloat("Horizontal", setVector.x);
        animator.SetFloat("Vertical", setVector.y);
    }

    private void changeAnim(Vector2 direction)
    {
        if(direction.x < 0)
        {
            if (Mathf.Abs(direction.x) >= Mathf.Abs(direction.y))
            {
                setAnimFloat(Vector2.left);
            }
            else
            {
                if(direction.y > 0)
                    setAnimFloat(Vector2.up);
                else
                    setAnimFloat(Vector2.down);
            }
        }
        else if(direction.x > 0)
        {
            if (Mathf.Abs(direction.x) >= Mathf.Abs(direction.y))
            {
                setAnimFloat(Vector2.right);
            }
            else
            {
                if (direction.y > 0)
                    setAnimFloat(Vector2.up);
                else
                    setAnimFloat(Vector2.down);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.gameObject.CompareTag("Breakable") || other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Small Chest")) 
            && !other.isTrigger)
            colliding = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if ((other.gameObject.CompareTag("Breakable") || other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Small Chest")) 
            && !other.isTrigger)
            colliding = false;
    }

/*    public override void takeDamage(int damage, int poiseDamage, bool isPlayerSpell)
    {
        base.takeDamage(damage, poiseDamage, isPlayerSpell);
        ui.SetHealth(health);
        if (health <= 0)
        {
            Player.MyInstance.GainXP(8);
            if (!isPlayerSpell)
                Player.MyInstance.GainWeaponXP(3);
            Player.MyInstance.counter = 0f;
        }
    }*/

    public override void Knock(Rigidbody2D myRB, float knockTime, int damage, int poiseDamage, bool isPlayerSpell)
    {
        //animator.SetTrigger("Staggering");
        base.Knock(myRB, knockTime, damage, poiseDamage, isPlayerSpell);
        ui.SetHealth(health);
    }

    private IEnumerator AttackCo()
    {
        animator.SetTrigger("Attack");
        currentState = EnemyStates.attack;
        yield return new WaitForSeconds(0.5f);
        Vector2 directiontToPlayer = target.position - transform.position;
        Vector3 move = Vector3.zero;
        if (currentState != EnemyStates.stagger)
        {
            move.x = directiontToPlayer.x;
            move.y = directiontToPlayer.y;

            transform.position += move / 3;
        }
        yield return new WaitForSeconds(0.4f);
        currentState = EnemyStates.idle;
        chaseSpeed = -1.5f;
    }

    private IEnumerator AttackCloseCo()
    {
        animator.SetTrigger("Attack");
        currentState = EnemyStates.attack2;
        yield return new WaitForSeconds(0.9f);
        currentState = EnemyStates.idle;
        chaseSpeed = -1.5f;
    }
}
