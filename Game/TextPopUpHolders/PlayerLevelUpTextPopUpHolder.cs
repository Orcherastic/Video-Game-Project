using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevelUpTextPopUpHolder : MonoBehaviour
{
    private static PlayerLevelUpTextPopUpHolder instance;

    public static PlayerLevelUpTextPopUpHolder MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerLevelUpTextPopUpHolder>();
            }
            return instance;
        }
    }
}
