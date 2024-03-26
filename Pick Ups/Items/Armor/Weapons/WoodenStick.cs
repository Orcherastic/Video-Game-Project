using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenStick : Weapon
{
    public override string GetDiscription()
    {
        return base.GetDiscription() + "\n" +
            "A stick made of wood, \n" +
            "not much to see here.";
    }
}
