using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bread : Item, IUsable
{
    public override string GetDiscription()
    {
        return base.GetDiscription() + "Heals a small amount of Health.";
    }

    public void TakeItem()
    {
        Player.MyInstance.UseItem("eating", this);
    }

    public void UseItem()
    {
        Player.MyInstance.HealHealth(10);
    }
}
