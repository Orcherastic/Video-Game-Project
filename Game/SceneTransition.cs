using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    public string sceneName;
    public Vector2 spawnPosition;
    private Animator anim;
    public bool sceneAlreadyLoaded = false;

    bool introWait = true;

    private void Start()
    {
        anim = GetComponent<Animator>();
        Player.MyInstance.sceneTransitioning = true;
        //StartCoroutine(WaitCo());
    }

    private void Update()
    {
        if(introWait)
        {
            introWait = false;
            StartCoroutine(WaitCo());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Player.MyInstance.currentState = PlayerState.interact;
            Player.MyInstance.SaveStats();
            Player.MyInstance.SaveSpawnPosition(spawnPosition);
            Player.MyInstance.SaveSprite();
            Player.MyInstance.SaveInventory();
            Player.MyInstance.SaveQuickUseSlots();
            Player.MyInstance.SaveEquipment();
            Player.MyInstance.SaveUseSpellSlots();
            Player.MyInstance.SaveKeyItem();
            Player.MyInstance.sceneTransitioning = true;
            StartCoroutine(FadeCo());
        }
    }

    private IEnumerator FadeCo()
    {
        anim.SetTrigger("Transition");
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            enemy.currentState = EnemyStates.wait;
        }
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator WaitCo()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            enemy.currentState = EnemyStates.wait;
        }

        Player.MyInstance.currentState = PlayerState.interact;
        yield return new WaitForSeconds(1.5f);

        foreach (Enemy enemy in enemies)
        {
            enemy.currentState = EnemyStates.idle;
        }
        Player.MyInstance.currentState = PlayerState.run;
        Player.MyInstance.sceneTransitioning = false;
    }
}
