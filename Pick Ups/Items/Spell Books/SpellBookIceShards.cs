using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBookIceShards : Item
{
    public override string GetDiscription()
    {
        return base.GetDiscription() +
            "Book containing knowlage of \n" +
            "Ice Shards spell.";
    }
}
