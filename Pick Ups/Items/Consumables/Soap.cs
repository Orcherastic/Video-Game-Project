using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soap : Item, IUsable
{
    public override string GetDiscription()
    {
        return base.GetDiscription() + "Cleans any filth or goo.";
    }

    public override bool CanBeUsed()
    {
        foreach (StatusEffect se in Player.MyInstance.statusEffects.ActiveStatusEffects)
        {
            if (se.statusEffectName == "Goo")
            {
                return true;
            }
        }
        return false;
    }

    public void TakeItem()
    {
        Player.MyInstance.UseItem("eating", this);
    }

    public void UseItem()
    {
        Player.MyInstance.statusEffects.RemoveStatusEffect(11);
    }
}
