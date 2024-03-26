using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnManager : MonoBehaviour
{
    public ItemSpawn itemSpawn;

    private static ItemSpawnManager instance;

    public static ItemSpawnManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ItemSpawnManager>();
            }
            return instance;
        }
    }
}
