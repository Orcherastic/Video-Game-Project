using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    [Header("Properties")]
    public Rigidbody2D rb;
    public Animator animator;
    [Header("Variables")]
    public float speed;
    public int poiseDamage;
    public Transform target;
    public float lifeSpan = 3f;
    public float counter = 0f;
    public Vector3 move = Vector3.zero;
    public string spellName;

    void Start()
    {
        move.x = Player.MyInstance.animator.GetFloat("Horizontal");
        move.y = Player.MyInstance.animator.GetFloat("Vertical");
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        //target = GameObject.FindWithTag("Enemy").transform;
    }

    void Update()
    {
        counter += 0.02f;
        if (counter >= lifeSpan)
        {
            SpellEndEffect();
        }
    }

    public virtual void SpellEndEffect()
    {
        
    }
}
