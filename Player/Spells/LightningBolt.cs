using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBolt : Spell
{
    public override void SpellEndEffect()
    {
        StartCoroutine(EndCo());
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().statusEffects.AddStatusEffectWithTimer(23, 3f);
        }
    }

    private IEnumerator EndCo()
    {
        yield return null;
        this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }
}
