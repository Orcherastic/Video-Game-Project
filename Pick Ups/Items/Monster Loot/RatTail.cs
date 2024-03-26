using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatTail : MonsterLoot
{
    public override string GetDiscription()
    {
        return base.GetDiscription() +
            "Tail of a big Cave Rat.";
    }
}
