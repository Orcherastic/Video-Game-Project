using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Antidote : Item, IUsable
{
    public override string GetDiscription()
    {
        return base.GetDiscription() + "Potion filled with antitoxins.\nRemoves Poison abnormality.";
    }

    public override bool CanBeUsed()
    {
        foreach(StatusEffect se in Player.MyInstance.statusEffects.ActiveStatusEffects)
        {
            if (se.statusEffectName == "Poison")
                return true;
        }
        return false;
    }

    public void TakeItem()
    {
        Player.MyInstance.UseItem("drinking", this);
    }

    public void UseItem()
    {
        Player.MyInstance.statusEffects.RemoveStatusEffectCompletely("Poison");
    }
}
