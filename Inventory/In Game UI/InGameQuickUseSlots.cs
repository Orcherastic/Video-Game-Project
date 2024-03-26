using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameQuickUseSlots : MonoBehaviour
{
    private static InGameQuickUseSlots instance;

    public static InGameQuickUseSlots myInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InGameQuickUseSlots>();
            }
            return instance;
        }
    }
}
