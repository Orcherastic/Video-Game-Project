using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerSpawnManager : MonoBehaviour
{
    public PlayerSpawnPosition playerSpawn;

    public static PlayerSpawnManager MyInstance;

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
