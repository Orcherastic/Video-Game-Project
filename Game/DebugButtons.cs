using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugButtons : MonoBehaviour
{
    public static DebugButtons MyInstance;

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

    public TutorialPopUpsSaver popUps;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            PlayerInventoryManager.MyInstance.playerInventoryItems.Reset();
            PlayerSpawnManager.MyInstance.playerSpawn.Reset();
            PlayerSpawnSpriteManager.MyInstance.playerSpriteInfo.Reset();
            PlayerSpellsManager.MyInstance.playerSpells.Reset();
            PlayerStatsManager.MyInstance.playerStats.Reset();
            popUps.Reset();
            SceneManager.LoadScene(0);
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            PlayerInventoryManager.MyInstance.playerInventoryItems.Reset();
            PlayerSpawnManager.MyInstance.playerSpawn.Reset();
            PlayerSpawnSpriteManager.MyInstance.playerSpriteInfo.Reset();
            PlayerSpellsManager.MyInstance.playerSpells.Reset();
            PlayerStatsManager.MyInstance.playerStats.Reset();
            popUps.Reset();
            SceneManager.LoadScene(4);
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            popUps.Reset();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
