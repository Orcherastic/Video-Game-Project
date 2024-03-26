using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : Enemy
{
    public AnimationCurve animationCurve;
    public float acceleration;

    bool retreat = false;
    int chance = -1;
    float retreatPosOffset = 25f;

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
                currentState = EnemyStates.chase;

                if (chaseSpeed < 3f)
                    chaseSpeed += acceleration;
                else if (chaseSpeed > 3f)
                    chaseSpeed -= acceleration;

                Vector3 temp;
                if (retreat)
                {
                    //temp = Vector3.Slerp(transform.position, target.position, chaseSpeed / 2 * Time.deltaTime);
                    temp = Vector3.MoveTowards(transform.position, target.position, chaseSpeed* Time.deltaTime);
                    temp = Vector3.Slerp(transform.position, CalculateRetreatPosition(target.position, temp - target.position, chance), chaseSpeed / 10 * Time.deltaTime);
                }

                else
                    temp = Vector3.Slerp(transform.position, target.position, chaseSpeed/2 * Time.deltaTime);
                    //temp = Vector3.MoveTowards(transform.position, target.position, chaseSpeed * Time.deltaTime);

                Vector2 direction = temp - target.position;
                if (direction.x > 0)
                    animator.SetFloat("Horizontal", -1f);
                else
                    animator.SetFloat("Horizontal", 1f);

                rb.MovePosition(temp);
                //animator.SetBool("isChasing", true);
                
                if (Vector3.Distance(target.position, transform.position) <= attackRadius && currentState != EnemyStates.stagger)
                {
                    currentState = EnemyStates.chase;
                    chaseSpeed = 4f;

                    if (retreat)
                    {
                        temp = Vector3.MoveTowards(transform.position, target.position, chaseSpeed * Time.deltaTime);
                        temp = Vector3.Slerp(transform.position, CalculateRetreatPosition(target.position, temp - target.position, chance), chaseSpeed / 10 * Time.deltaTime);
                    }

                    else
                        temp = Vector3.MoveTowards(transform.position, target.position, chaseSpeed * Time.deltaTime);

                    direction = temp - target.position;
                    if (direction.x > 0)
                        animator.SetFloat("Horizontal", -1f);
                    else
                        animator.SetFloat("Horizontal", 1f);

                    rb.MovePosition(temp);
                }
            }
        }
        else if (Vector3.Distance(target.position, transform.position) > chaseRadius && !canvas.isActiveAndEnabled)
        {
            //animator.SetBool("isChasing", false);
            currentState = EnemyStates.idle;
            Vector3 temp = Vector3.MoveTowards(transform.position, target.position, chaseSpeed * Time.deltaTime);
            Vector2 direction = temp - target.position;
            if (direction.x > 0)
                animator.SetFloat("Horizontal", -1f);
            else
                animator.SetFloat("Horizontal", 1f);

            rb.MovePosition(temp);
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.velocity = Vector2.zero;
            if (chaseSpeed > 0)
                chaseSpeed -= acceleration;
        }
    }

    public Vector2 CalculateRetreatPosition(Vector2 targetPos, Vector2 direction, int chance)
    {
        Vector2 result = Vector2.zero;
        if (direction.x > 0)
        {
            if (Mathf.Abs(direction.x) >= Mathf.Abs(direction.y))
            {
                // From the Right
                result.x = targetPos.x + retreatPosOffset;

                if (chance > 50)
                    result.y = targetPos.y + retreatPosOffset;
                else
                    result.y = targetPos.y - retreatPosOffset;
                return result;
            }
            else
            {
                if (direction.y > 0)
                {
                    // From Above
                    result.y = targetPos.y + retreatPosOffset;

                    if (chance > 50)
                        result.x = targetPos.x + retreatPosOffset;
                    else
                        result.x = targetPos.x - retreatPosOffset;
                    return result;
                }
                else
                {
                    // From Below
                    result.y = targetPos.y - retreatPosOffset;

                    if (chance > 50)
                        result.x = targetPos.x + retreatPosOffset;
                    else
                        result.x = targetPos.x - retreatPosOffset;
                    return result;
                }
            }
        }
        else if (direction.x < 0)
        {
            if (Mathf.Abs(direction.x) >= Mathf.Abs(direction.y))
            {
                // From the Left
                result.x = targetPos.x - retreatPosOffset;

                if (chance > 50)
                    result.y = targetPos.y + retreatPosOffset;
                else
                    result.y = targetPos.y - retreatPosOffset;
                return result;
            }
            else
            {
                if (direction.y > 0)
                {
                    // From Above
                    result.y = targetPos.y + retreatPosOffset;

                    if (chance > 50)
                        result.x = targetPos.x + retreatPosOffset;
                    else
                        result.x = targetPos.x - retreatPosOffset;
                    return result;
                }
                else
                {
                    // From Below
                    result.y = targetPos.y - retreatPosOffset;

                    if (chance > 50)
                        result.x = targetPos.x + retreatPosOffset;
                    else
                        result.x = targetPos.x - retreatPosOffset;
                    return result;
                }
            }
        }
        else
            return result;
    }

/*    public override void takeDamage(int damage, int poiseDamage, bool isPlayerSpell)
    {
        base.takeDamage(damage, poiseDamage, isPlayerSpell);
        ui.SetHealth(health);
        if (health <= 0)
        {
            Player.MyInstance.GainXP(6);
            if (!isPlayerSpell)
                Player.MyInstance.GainWeaponXP(2);
            Player.MyInstance.counter = 0f;
        }
    }*/

    public override void Knock(Rigidbody2D myRB, float knockTime, int damage, int poiseDamage, bool isPlayerSpell)
    {
        //animator.SetTrigger("Staggering");
        base.Knock(myRB, knockTime, damage, poiseDamage, isPlayerSpell);
        StartCoroutine(HitCo());
        ui.SetHealth(health);
    }

    public override void OnPlayerHit()
    {
        int poisonProb = Random.Range(0, 101);
        if (poisonProb <= 20)
        {
            //Poison
            Player.MyInstance.statusEffects.AddStatusEffectWithTimer(17, 60f - (Player.MyInstance.poisonResistance / 10));
        }
        chance = Random.Range(0, 101);
        StartCoroutine(HitCo());
    }

    private IEnumerator HitCo()
    {
        yield return null;
        retreat = true;
        yield return new WaitForSeconds(2f);
        retreat = false;
    }


    public List<Vector3> EvaluateSlerpPoints(Vector3 start, Vector3 end, float centerOffset)
    {
        List<Vector3> positions = new List<Vector3>();
        var centerPivot = (start + end) * 0.5f;
        centerPivot -= new Vector3(0, -centerOffset);

        var startRelativeCenter = start - centerPivot;
        var endRelativeCenter = end - centerPivot;

        var f = 1f / 10;

        for(var i=0f; i<1+f; i+=f)
        {
            positions.Add(Vector3.Slerp(startRelativeCenter, endRelativeCenter, i) + centerPivot);
        }

        return positions;
    }
}
