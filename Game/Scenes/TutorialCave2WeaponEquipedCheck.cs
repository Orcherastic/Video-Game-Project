using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCave2WeaponEquipedCheck : MonoBehaviour
{
    public TutorialPopUpsSaver saver;
    private bool popedUp = false;

    void Start()
    {
        LoadPoped();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !Player.MyInstance.equipedWeapon && !popedUp)
        {
            CombatTextManager.MyInstance.CreateTextWorld(Player.MyInstance.transform.position, "You can't fight without a weapon.", TextType.levelUp);
            popedUp = true;
            SavePoped();
            Destroy(this.gameObject);
        }
    }

    public void SavePoped()
    {
        saver.weaponNotEquiped = popedUp;
    }

    public void LoadPoped()
    {
        popedUp = saver.weaponNotEquiped;
    }
}
