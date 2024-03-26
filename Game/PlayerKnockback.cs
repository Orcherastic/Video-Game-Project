using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnockback : MonoBehaviour
{
    public float thrust;
    public float knockTime;
    public bool isPlayerSpell = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Player"))
        {
            Rigidbody2D hit = other.GetComponent<Rigidbody2D>();
            if (hit != null)
            {
                if ((other.gameObject.CompareTag("Player") && !isPlayerSpell) || other.gameObject.CompareTag("Enemy"))
                {
                    Vector2 difference = hit.transform.position - transform.position;
                    difference = difference.normalized * thrust;
                    hit.AddForce(difference, ForceMode2D.Impulse);
                }

/*                if (other.gameObject.CompareTag("Player") && !isPlayerSpell)
                {
                    //hit.GetComponent<Player>().currentState = PlayerState.stagger;
                    //other.GetComponent<Player>().Knock(knockTime, Enemy.MyInstance.damage);
                }*/
            }
        }
    }
}
