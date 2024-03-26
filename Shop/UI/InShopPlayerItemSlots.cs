using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InShopPlayerItemSlots : MonoBehaviour
{
    private static InShopPlayerItemSlots instance;

    public static InShopPlayerItemSlots myInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InShopPlayerItemSlots>();
            }
            return instance;
        }
    }
}
