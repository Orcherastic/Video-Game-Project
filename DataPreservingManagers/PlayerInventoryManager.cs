using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour
{
    public PlayerInventoryItems playerInventoryItems;

    public static PlayerInventoryManager MyInstance;

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
