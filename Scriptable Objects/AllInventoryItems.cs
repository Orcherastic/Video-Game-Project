using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AllInventoryItems", menuName = "ScriptableObjects/AllInventoryItems", order = 1)]
public class AllInventoryItems : ScriptableObject
{
    public List<Item> allItems = new List<Item>();
}
