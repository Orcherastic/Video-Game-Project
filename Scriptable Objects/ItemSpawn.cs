using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemSpawn", menuName = "ScriptableObjects/ItemSpawn", order = 1)]
public class ItemSpawn : ScriptableObject
{
    public Dictionary<string, bool> itemsColected = new Dictionary<string, bool>();
}
