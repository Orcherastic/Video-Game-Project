using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllInventoryItemsManager : MonoBehaviour
{
    public AllInventoryItems allInventoryItems;

    public static AllInventoryItemsManager MyInstance;

    void Awake()
    {
        if (MyInstance == null)
        {
            MyInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
