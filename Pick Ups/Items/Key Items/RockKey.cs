using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockKey : Item
{
    public override string GetDiscription()
    {
        return base.GetDiscription() + "Unlocks the Rock Gate.";
    }
}
