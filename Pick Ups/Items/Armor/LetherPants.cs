using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetherPants : Armor
{
    public override string GetDiscription()
    {
        return base.GetDiscription() + "\n" +
            "Made from cow leather, \n" +
            "quite comfortable.";
    }
}
