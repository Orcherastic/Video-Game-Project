using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopperChestplate : Armor
{
    public override string GetDiscription()
    {
        return base.GetDiscription() + "\n" +
            "Little rusty, \n" +
            "but still better than nothing.";
    }
}
