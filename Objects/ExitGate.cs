using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGate : MonoBehaviour
{
    private bool playerInRange = false;

    void Update()
    {
        if(Player.MyInstance.dungeonKeyObtained && Input.GetButtonDown("Interact") && playerInRange)
        {
            PlayerUI.MyInstance.dungeonGateKeyUsed();
            RoomFirstDungeonGenerator.MyInstance.StartCoroutine(OpenCo());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            playerInRange = true;
            Player.MyInstance.inTriggerRange = true;
            Player.MyInstance.ThinkBubble.PopUp();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            playerInRange = false;
            Player.MyInstance.inTriggerRange = false;
            Player.MyInstance.ThinkBubble.PopDown();
        }
    }

    private IEnumerator OpenCo()
    {
        Player.MyInstance.currentState = PlayerState.cutscene;
        Player.MyInstance.animator.SetBool("isRunning", false);
        Player.MyInstance.animator.SetBool("isWalking", false);
        foreach (PlayerEquipment pe in Player.MyInstance.equipments)
        {
            pe.SetXAndY(Player.MyInstance.animator.GetFloat("Horizontal"), Player.MyInstance.animator.GetFloat("Vertical"));
            pe.SetRunWalkBools("isRunning", Player.MyInstance.animator.GetBool("isRunning"),
                               "isWalking", Player.MyInstance.animator.GetBool("isWalking"));
        }
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            enemy.currentState = EnemyStates.wait;
        }
        yield return null;
        RoomFirstDungeonGenerator.MyInstance.exitOpened.SetActive(true);
        gameObject.SetActive(false);
        MainCamera.MyInstance.BigShake();
        yield return new WaitForSeconds(2f);
        foreach (Enemy enemy in enemies)
        {
            enemy.currentState = EnemyStates.idle;
        }
        Player.MyInstance.currentState = PlayerState.run;
    }
}
