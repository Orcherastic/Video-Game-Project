using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatFang : MonsterLoot
{
    public override string GetDiscription()
    {
        return base.GetDiscription() + 
            "Sharp fang from the Cave Bat.";
    }
}
