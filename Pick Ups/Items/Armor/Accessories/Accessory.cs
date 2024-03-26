using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accessory : Armor, IDescriptable
{
    
    public override string GetItemType()
    {
        return "Accessory\n";
    }
    public override string GetItemStats1()
    {
        return string.Empty;
    }

    public override string GetItemStats1Amount()
    {
        return string.Empty;
    }

    public override string GetItemStats1Comparison()
    {
        return string.Empty;
    }

    public override string GetItemStats2()
    {
        return string.Empty;
    }

    public override string GetItemStats2Amount()
    {
        return string.Empty;
    }

    public override string GetItemStats2Comparison()
    {
        return string.Empty;
    }
}
