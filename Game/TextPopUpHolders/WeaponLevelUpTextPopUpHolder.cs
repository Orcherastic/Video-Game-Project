using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLevelUpTextPopUpHolder : MonoBehaviour
{
    private static WeaponLevelUpTextPopUpHolder instance;

    public static WeaponLevelUpTextPopUpHolder MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<WeaponLevelUpTextPopUpHolder>();
            }
            return instance;
        }
    }
}
