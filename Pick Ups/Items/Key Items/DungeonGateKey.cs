using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGateKey : Item
{
    public override string GetDiscription()
    {
        return base.GetDiscription() + "Unlocks the gate to the next dungeon.";
    }
}
