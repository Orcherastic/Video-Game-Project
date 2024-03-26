using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bone : MonsterLoot
{
    public override string GetDiscription()
    {
        return base.GetDiscription() +
            "Ordinary human bone.";
    }
}
