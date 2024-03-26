using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPTextPopUpHolder : MonoBehaviour
{
    private static XPTextPopUpHolder instance;

    public static XPTextPopUpHolder MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<XPTextPopUpHolder>();
            }
            return instance;
        }
    }
}
