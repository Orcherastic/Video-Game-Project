using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpellsManager : MonoBehaviour
{
    public PlayerSpells playerSpells;

    public static PlayerSpellsManager MyInstance;

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
