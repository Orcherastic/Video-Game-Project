using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBookLightningBolt : Item
{
    public override string GetDiscription()
    {
        return base.GetDiscription() +
            "Book containing knowlage of \n" +
            "Lightning Bolt spell.";
    }
}
