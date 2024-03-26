using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TutorialType
{
    move,
    walk,
    inventory,
    attack
}

public class TutorialPopUps : MonoBehaviour
{
    private static TutorialPopUps instance;

    public static TutorialPopUps MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<TutorialPopUps>();
            }
            return instance;
        }
    }

    public TutorialPopUpsSaver saver;
    public TutorialType type;

    private Animator anim;
    public float popUpTime;
    public float popBackTime;

    private bool popedUp = false;
    private bool popedBack = false;

    public bool finished = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        LoadPopUps();
    }

    // Update is called once per frame
    void Update()
    {
        if(!finished)
        {
            if (popUpTime > 0)
                popUpTime -= Time.deltaTime;
            else
            {
                if (!popedUp)
                {
                    popedUp = true;
                    anim.SetTrigger("PopUp");
                }
            }

            if (popedUp)
            {
                if (popBackTime > 0)
                    popBackTime -= Time.deltaTime;
                else
                {
                    if (!popedBack)
                    {
                        popedBack = true;
                        anim.SetTrigger("PopBack");
                        finished = true;
                        SavePopUps();
                    }
                }
            }
        }
    }

    public void SavePopUps()
    {
        switch(type)
        {
            case TutorialType.move:
                saver.moveCommands = finished;
                break;
            case TutorialType.walk:
                saver.walkCommand = finished;
                break;
            case TutorialType.inventory:
                saver.inventoryCommand = finished;
                break;
        }
    }

    public void LoadPopUps()
    {
        switch (type)
        {
            case TutorialType.move:
                finished = saver.moveCommands;
                break;
            case TutorialType.walk:
                finished = saver.walkCommand;
                break;
            case TutorialType.inventory:
                finished = saver.inventoryCommand;
                break;
        }
    }
}
