using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBookHeavyStrike : Item
{
    public override string GetDiscription()
    {
        return base.GetDiscription() +
            "Book containing knowlage of \n" +
            "Heavy Strike.";
    }
}
