using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceShard : Spell
{
    void FixedUpdate()
    {
        rb.velocity = move.normalized * speed;

        float angle = Mathf.Atan2(move.y, move.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public override void SpellEndEffect()
    {
        StartCoroutine(BreakCo());
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.gameObject.CompareTag("Enemy") && other.isTrigger) || other.gameObject.CompareTag("Breakable") || !other.isTrigger)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                other.GetComponent<Enemy>().statusEffects.AddStatusEffectWithTimer(21, 15f);
            }
            StartCoroutine(BreakCo());
        }
    }

    private IEnumerator BreakCo()
    {
        speed = 0;
        animator.SetTrigger("Break");
        yield return new WaitForSeconds(0.3f);
        this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }
}
