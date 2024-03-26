using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMagicUseSlots : MonoBehaviour
{
    private static InGameMagicUseSlots instance;

    public static InGameMagicUseSlots myInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InGameMagicUseSlots>();
            }
            return instance;
        }
    }
}
