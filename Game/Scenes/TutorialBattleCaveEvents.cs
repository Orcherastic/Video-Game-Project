using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBattleCaveEvents : MonoBehaviour
{
    private static TutorialBattleCaveEvents instance;

    public static TutorialBattleCaveEvents MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<TutorialBattleCaveEvents>();
            }
            return instance;
        }
    }

    public GameObject eventTrigger;

    public GameObject LeftGateClosed;
    public GameObject RightGateClosed;
    public GameObject LeftGateOpenedUpper;
    public GameObject LeftGateOpenedLower;
    public GameObject RightGateOpenedUpper;
    public GameObject RightGateOpenedLower;

    public Enemy slime1;
    public Enemy slime2;
    public Enemy slime3;
    public Enemy slime4;
    public Enemy slime5;
    public Enemy slime6;

    public SmallChest chest1;
    public SmallChest chest2;

    public DustEffect dust1;
    public DustEffect dust2;
    public DustEffect dust3;
    public DustEffect dust4;
    public DustEffect dust5;
    public DustEffect dust6;
    public DustEffect dust7;
    public DustEffect dust8;

    private bool slime1Killed = false;
    private bool slimes23Killed = false;
    private bool slimes456Killed = false;

    private bool chest1Opened = false;
    private bool chest2Opened = false;

    private void Update()
    {
        if(!slime1 && Player.MyInstance.currentState == PlayerState.run)
        {
            if (!slime1Killed)
            {
                StartCoroutine(SpawnChest1());
                slime1Killed = true;
            }
        }

        if(chest1.isOpened && Player.MyInstance.currentState != PlayerState.interact)
        {
            if (!chest1Opened)
            {
                StartCoroutine(Spawn2Slimes());
                chest1Opened = true;
            }

        }

        if(!slime2 && !slime3 && Player.MyInstance.currentState == PlayerState.run)
        {
            if (!slimes23Killed)
            {
                StartCoroutine(SpawnChest2());
                slimes23Killed = true;
            }
        }

        if (chest2.isOpened && Player.MyInstance.currentState != PlayerState.interact)
        {
            if (!chest2Opened)
            {
                StartCoroutine(Spawn3Slimes());
                chest2Opened = true;
            }

        }

        if (!slime4 && !slime5 && !slime6 && Player.MyInstance.currentState == PlayerState.run)
        {
            if (!slimes456Killed)
            {
                StartCoroutine(OpenRightGate());
                slimes456Killed = true;
            }
        }
    }

    public IEnumerator CloseGates()
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
        yield return null;
        LeftGateOpenedUpper.gameObject.SetActive(false);
        LeftGateOpenedLower.gameObject.SetActive(false);
        RightGateOpenedUpper.gameObject.SetActive(false);
        RightGateOpenedLower.gameObject.SetActive(false);
        LeftGateClosed.gameObject.SetActive(true);
        RightGateClosed.gameObject.SetActive(true);
        yield return null;
        MainCamera.MyInstance.BigShake();
        yield return new WaitForSeconds(2f);
        Player.MyInstance.animator.SetFloat("Horizontal", -1f);
        foreach (PlayerEquipment pe in Player.MyInstance.equipments)
        {
            pe.SetXAndY(Player.MyInstance.animator.GetFloat("Horizontal"), Player.MyInstance.animator.GetFloat("Vertical"));
        }
        yield return new WaitForSeconds(1f);
        Player.MyInstance.animator.SetFloat("Horizontal", 1f);
        foreach (PlayerEquipment pe in Player.MyInstance.equipments)
        {
            pe.SetXAndY(Player.MyInstance.animator.GetFloat("Horizontal"), Player.MyInstance.animator.GetFloat("Vertical"));
        }
        yield return new WaitForSeconds(1f);
        MainCamera.MyInstance.cutscene = true;
        MainCamera.MyInstance.MoveToPosition(slime1.transform);
        yield return new WaitForSeconds(2f);
        dust1.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        slime1.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        MainCamera.MyInstance.MoveToPosition(Player.MyInstance.transform);
        yield return new WaitForSeconds(2f);
        MainCamera.MyInstance.cutscene = false;
        CombatTextManager.MyInstance.CreateTextWorld(Player.MyInstance.transform.position, "Use your weapon to defeat your enemies.", TextType.levelUp);
        yield return new WaitForSeconds(1f);
        Player.MyInstance.currentState = PlayerState.run;
    }

    public IEnumerator SpawnChest1()
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
        yield return null;
        MainCamera.MyInstance.cutscene = true;
        MainCamera.MyInstance.MoveToPosition(chest1.transform);
        yield return new WaitForSeconds(2f);
        dust2.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        chest1.gameObject.SetActive(true);
        CombatTextManager.MyInstance.CreateTextWorld(chest1.transform.position, "Now claim your reward.", TextType.levelUp);
        yield return new WaitForSeconds(1f);
        MainCamera.MyInstance.MoveToPosition(Player.MyInstance.transform);
        yield return new WaitForSeconds(2f);
        MainCamera.MyInstance.cutscene = false;
        Player.MyInstance.currentState = PlayerState.run;
    }

    public IEnumerator Spawn2Slimes()
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
        yield return null;
        MainCamera.MyInstance.cutscene = true;
        MainCamera.MyInstance.MoveToPosition(slime3.transform);
        yield return new WaitForSeconds(2f);
        dust3.gameObject.SetActive(true);
        dust4.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        slime2.gameObject.SetActive(true);
        slime3.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        MainCamera.MyInstance.MoveToPosition(Player.MyInstance.transform);
        yield return new WaitForSeconds(2f);
        MainCamera.MyInstance.cutscene = false;
        CombatTextManager.MyInstance.CreateTextWorld(Player.MyInstance.transform.position, "Use your newfound power to defeat your enemies.", TextType.levelUp);
        yield return new WaitForSeconds(1f);
        Player.MyInstance.currentState = PlayerState.run;

    }

    public IEnumerator SpawnChest2()
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
        yield return null;
        MainCamera.MyInstance.cutscene = true;
        MainCamera.MyInstance.MoveToPosition(chest2.transform);
        yield return new WaitForSeconds(2f);
        dust5.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        chest2.gameObject.SetActive(true);
        CombatTextManager.MyInstance.CreateTextWorld(chest2.transform.position, "Take it, you earned it.", TextType.levelUp);
        yield return new WaitForSeconds(1f);
        MainCamera.MyInstance.MoveToPosition(Player.MyInstance.transform);
        yield return new WaitForSeconds(2f);
        MainCamera.MyInstance.cutscene = false;
        Player.MyInstance.currentState = PlayerState.run;
    }

    public IEnumerator Spawn3Slimes()
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
        yield return null;
        MainCamera.MyInstance.cutscene = true;
        MainCamera.MyInstance.MoveToPosition(slime5.transform);
        yield return new WaitForSeconds(2f);
        dust6.gameObject.SetActive(true);
        dust7.gameObject.SetActive(true);
        dust8.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        slime4.gameObject.SetActive(true);
        slime5.gameObject.SetActive(true);
        slime6.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        MainCamera.MyInstance.MoveToPosition(Player.MyInstance.transform);
        yield return new WaitForSeconds(2f);
        MainCamera.MyInstance.cutscene = false;
        Player.MyInstance.currentState = PlayerState.run;

    }

    public IEnumerator OpenRightGate()
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
        yield return null;
        MainCamera.MyInstance.cutscene = true;
        MainCamera.MyInstance.MoveToPosition(RightGateClosed.transform);
        yield return new WaitForSeconds(2f);
        RightGateClosed.gameObject.SetActive(false);
        RightGateOpenedUpper.gameObject.SetActive(true);
        RightGateOpenedLower.gameObject.SetActive(true);
        MainCamera.MyInstance.BigShake();
        yield return new WaitForSeconds(1f);
        MainCamera.MyInstance.MoveToPosition(Player.MyInstance.transform);
        yield return new WaitForSeconds(2f);
        MainCamera.MyInstance.cutscene = false;
        CombatTextManager.MyInstance.CreateTextWorld(Player.MyInstance.transform.position, "The Dungeon awaits.", TextType.levelUp);
        yield return new WaitForSeconds(1f);
        Player.MyInstance.currentState = PlayerState.run;
    }
}
