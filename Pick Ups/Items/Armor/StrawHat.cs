using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrawHat : Armor
{
    public override string GetDiscription()
    {
        return base.GetDiscription() + "\n" +
            "Ordinart straw hat, \n" +
            "not really suited for combat, but it  \n" +
            "strangly makes you feel like a pirate.";
    }
}
