using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy
{
    public int comboProb = -1;
    private bool colliding = false;

    void Update()
    {
        if (health < maxHealth)
            canvas.gameObject.SetActive(true);

        if (currentState != EnemyStates.death || currentState != EnemyStates.wait)
        {
            CheckDistance();
            UpdatePoise();
        }
        else
        {
            if (currentState != EnemyStates.death)
                animator.SetBool("isDead", true);
            animator.SetBool("isChasing", false);
            chaseSpeed = 0;
        }

        if (currentState != EnemyStates.stagger && !colliding && !hit)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.velocity = Vector2.zero;
        }
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
                animator.SetBool("isChasing", true);
                chaseSpeed = 1f;
            }
        }
        else if (Vector3.Distance(target.position, transform.position) <= attackRadius)
        {
            if (currentState == EnemyStates.idle || currentState == EnemyStates.chase)
            {
                animator.SetBool("isChasing", false);
                chaseSpeed = 0;

                Vector3 temp = Vector3.MoveTowards(transform.position, target.position, chaseSpeed * Time.deltaTime);
                changeAnim(-(temp - target.position));

                if(comboProb == -1)
                {
                    comboProb = Random.Range(0, 101);
                    if (comboProb <= 25)
                    {
                        StartCoroutine(AttackComboCo());
                    }
                    else
                    {
                        StartCoroutine(Attack1Co());
                    }
                }
            }
        }
        else if (Vector3.Distance(target.position, transform.position) > chaseRadius)
        {
            animator.SetBool("isChasing", false);
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
        if (direction.x < 0)
        {
            if (Mathf.Abs(direction.x) >= Mathf.Abs(direction.y))
            {
                setAnimFloat(Vector2.left);
            }
            else
            {
                if (direction.y > 0)
                    setAnimFloat(Vector2.up);
                else
                    setAnimFloat(Vector2.down);
            }
        }
        else if (direction.x > 0)
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
        comboProb = -1;
        if (health <= 0)
        {
            Player.MyInstance.GainXP(10);
            if (!isPlayerSpell)
                Player.MyInstance.GainWeaponXP(4);
            Player.MyInstance.counter = 0f;
        }
    }*/

    public override void Knock(Rigidbody2D myRB, float knockTime, int damage, int poiseDamage, bool isPlayerSpell)
    {
        //animator.SetTrigger("Staggering");
        base.Knock(myRB, knockTime, damage, poiseDamage, isPlayerSpell);
        ui.SetHealth(health);
    }

    private IEnumerator Attack1Co()
    {
        animator.SetTrigger("Attack1");
        yield return null;
        currentState = EnemyStates.attack;

        Vector2 directiontToPlayer = target.position - transform.position;
        Vector3 move = Vector3.zero;
        if (currentState != EnemyStates.stagger)
        {
            move.x = directiontToPlayer.x;
            move.y = directiontToPlayer.y;

            transform.position += move / 5;
        }

        yield return new WaitForSeconds(1.2f);
        int comboProb2 = Random.Range(0, 101);
        if (comboProb2 <= 25)
        {
            if (currentState != EnemyStates.attack2)
                StartCoroutine(Attack2Co());
        }
        else
        {
            currentState = EnemyStates.idle;
            chaseSpeed = 1f;
            comboProb = -1;
        }
    }

    private IEnumerator Attack2Co()
    {
        animator.SetTrigger("Attack2");
        //yield return null;
        currentState = EnemyStates.attack2;

        yield return new WaitForSeconds(1f);
        currentState = EnemyStates.idle;
        chaseSpeed = 1f;
        comboProb = -1;
    }

    private IEnumerator AttackComboCo()
    {
        animator.SetTrigger("AttackCombo");
        yield return null;
        currentState = EnemyStates.attackCombo;

        Vector3 move = Vector3.zero;
        if (currentState != EnemyStates.stagger)
        {
            move.x = animator.GetFloat("Horizontal");
            move.y = animator.GetFloat("Vertical");

            transform.position += move / 5;
        }

        yield return new WaitForSeconds(2.1f);
        currentState = EnemyStates.idle;
        chaseSpeed = 1f;
        comboProb = -1;
    }
}
