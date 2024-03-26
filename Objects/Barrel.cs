using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : BreakableObject
{
    public override void Break(int damage)
    {
        base.Break(damage);
        if (health <= 0)
            anim.SetTrigger("Destroyed");
    }
}
