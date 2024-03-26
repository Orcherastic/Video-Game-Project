using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TutorialPopUpsSaver", menuName = "ScriptableObjects/TutorialPopUpsSaver", order = 1)]
public class TutorialPopUpsSaver : ScriptableObject
{
    public bool moveCommands;
    public bool walkCommand;
    public bool pickItUpTrigger;
    public bool invisibleWall;
    public bool inventoryCommand;
    public bool weaponNotEquiped;

    public void Reset()
    {
        moveCommands = false;
        walkCommand = false;
        pickItUpTrigger = false;
        invisibleWall = false;
        inventoryCommand = true;
        weaponNotEquiped = false;
    }
}
