using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenSword : Weapon
{
    public override string GetDiscription()
    {
        return base.GetDiscription() + "\n" +
            "A standard wooden sword.\n" +
            "Very commonly used in sword training.";
    }
}
