using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonousTouch : Spell
{
    void FixedUpdate()
    {
        rb.velocity = move.normalized * speed;

        float angle = Mathf.Atan2(move.y, move.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public override void SpellEndEffect()
    {
        StartCoroutine(PuffCo());
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.gameObject.CompareTag("Enemy") && other.isTrigger) || other.gameObject.CompareTag("Breakable") || !other.isTrigger)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                other.GetComponent<Enemy>().statusEffects.AddStatusEffectWithTimer(17, 30f);
            }

            StartCoroutine(PuffCo());
        }
    }

    private IEnumerator PuffCo()
    {
        speed = 0;
        animator.SetTrigger("Puff");
        yield return new WaitForSeconds(0.5f);
        this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }
}
