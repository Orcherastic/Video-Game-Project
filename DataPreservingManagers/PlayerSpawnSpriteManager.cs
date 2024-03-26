using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnSpriteManager : MonoBehaviour
{
    public PlayerSpawnSprite playerSpriteInfo;

    public static PlayerSpawnSpriteManager MyInstance;

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
