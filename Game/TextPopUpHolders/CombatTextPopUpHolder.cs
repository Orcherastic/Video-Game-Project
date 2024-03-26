using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTextPopUpHolder : MonoBehaviour
{
    private static CombatTextPopUpHolder instance;

    public static CombatTextPopUpHolder MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CombatTextPopUpHolder>();
            }
            return instance;
        }
    }
}
