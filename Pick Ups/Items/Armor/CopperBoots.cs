using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopperBoots : Armor
{
    public override string GetDiscription()
    {
        return base.GetDiscription() + "\n" +
            "Decent enough boots for combat, \n" +
            "not very durable though.";
    }
}
