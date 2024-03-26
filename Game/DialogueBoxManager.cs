using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBoxManager : MonoBehaviour
{
    private static DialogueBoxManager instance;

    public static DialogueBoxManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DialogueBoxManager>();
            }
            return instance;
        }
    }

    public DialogueBox dialogueBox;

    private void Start()
    {
        dialogueBox = FindObjectOfType<DialogueBox>();
        dialogueBox.gameObject.SetActive(false);
    }

    public void Activate()
    {
        dialogueBox.gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        dialogueBox.gameObject.SetActive(false);    
    }
}
