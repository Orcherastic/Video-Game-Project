using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBookDefenceUp : Item
{
    public override string GetDiscription()
    {
        return base.GetDiscription() +
            "Book containing knowlage of\n " +
            "Defence Up spell.";
    }
}
