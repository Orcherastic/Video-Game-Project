using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    public Animator anim;
    public GameObject pickUpPrefab;
    public int health = 1;

    public virtual void Break(int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            float num = Mathf.Floor(Random.Range(1, 3));
            if(pickUpPrefab != null)
                for(int i=0; i<num; i++)
                    Instantiate(pickUpPrefab, transform.position, Quaternion.identity);
            StartCoroutine(BreakCo());
        }
    }

    IEnumerator BreakCo()
    {
        yield return new WaitForSeconds(0.4f);
        this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }

}
