using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBookPoisonousTouch : Item
{
    public override string GetDiscription()
    {
        return base.GetDiscription() +
            "Book containing knowlage of \n" +
            "Poisonous Touch spell.";
    }
}
