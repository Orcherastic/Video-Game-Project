using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenSlimeBlob : MonsterLoot
{
    public override string GetDiscription()
    {
        return base.GetDiscription() + "Very gooey, and yet solid.";
    }
}
