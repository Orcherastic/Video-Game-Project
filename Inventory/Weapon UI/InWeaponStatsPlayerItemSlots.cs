using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InWeaponStatsPlayerItemSlots : MonoBehaviour
{
    private static InWeaponStatsPlayerItemSlots instance;

    public static InWeaponStatsPlayerItemSlots MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InWeaponStatsPlayerItemSlots>();
            }
            return instance;
        }
    }
}
