using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RustyRing : Accessory, IStatusEffectable
{
    public override string GetItemStats1()
    {
        return string.Format("<color={0}>Slightly increases Max MP \nbut decreases Max HP.</color>", Color.white);
    }

    public override string GetDiscription()
    {
        return base.GetDiscription() + "\n" +
            "Old rusty ring, \n" +
            "might have been more powerfull \n" +
            "a few decades ago.";
    }

    public void AddStatusEffects()
    {
        Player.MyInstance.statusEffects.AddStatusEffect(6);
        Player.MyInstance.statusEffects.AddStatusEffect(12);
    }

    public void RemoveStatusEffects()
    {
        Player.MyInstance.statusEffects.RemoveStatusEffect(6);
        Player.MyInstance.statusEffects.RemoveStatusEffect(12);
    }
}
