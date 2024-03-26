using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatWing : MonsterLoot
{
    public override string GetDiscription()
    {
        return base.GetDiscription() + 
            "Wing of the Cave Bat.";
    }
}
