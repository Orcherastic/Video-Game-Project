using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinTextPopUpHolder : MonoBehaviour
{
    private static CoinTextPopUpHolder instance;

    public static CoinTextPopUpHolder MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CoinTextPopUpHolder>();
            }
            return instance;
        }
    }
}
