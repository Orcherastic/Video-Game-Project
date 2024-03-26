using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallManaPotion : Item, IUsable
{
    public override string GetDiscription()
    {
        return base.GetDiscription() + 
            "Restores a small amount of Mana.";
    }

    public void TakeItem()
    {
        Player.MyInstance.UseItem("drinking", this);
    }

    public void UseItem()
    {
        Player.MyInstance.GainMana(5);
    }
}
