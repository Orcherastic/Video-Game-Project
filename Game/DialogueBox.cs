using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour
{
    public bool fullSentence = false;
    public bool typing = false;
    public bool finish = false;
    public bool hasOptions = false;
    public Text theTalker;
    public Text theTalking;
    public Text continueText;
    public DialogueBoxOptions dialogueOptions;

    void Update()
    {
        if (fullSentence)
        {
            if(!hasOptions)
                continueText.gameObject.SetActive(true);
            typing = false;
        }

        else
            continueText.gameObject.SetActive(false);
    }

    public void SetDialogue(string talker, string talking)
    {
        theTalker.text = talker;
        StopAllCoroutines();
        StartCoroutine(TypeSentenceCo(talking));
    }

    public void SetDialogueWithOptions(string talker, string talking, List<string> optionTexts)
    {
        theTalker.text = talker;
        StopAllCoroutines();
        StartCoroutine(TypeSentenceAndOptionsCo(talking, optionTexts));
    }

    public void ResetDialogue()
    {
        theTalker.text = null;
        theTalking.text = null;
        fullSentence = false;
        dialogueOptions.RemoveOptions();
    }

    public void RemoveOptions()
    {
        dialogueOptions.RemoveOptions();
    }

    private IEnumerator TypeSentenceCo(string sentence)
    {
        hasOptions = false;
        finish = false;
        fullSentence = false;
        typing = true;
        theTalking.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            theTalking.text += letter;
            if(finish)
            {
                theTalking.text = sentence;
                break;
            }
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);
        fullSentence = true;
    }

    private IEnumerator TypeSentenceAndOptionsCo(string sentence, List<string> optionTexts)
    {
        hasOptions = true;
        finish = false;
        fullSentence = false;
        typing = true;
        theTalking.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            theTalking.text += letter;
            if (finish)
            {
                theTalking.text = sentence;
                break;
            }
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);
        fullSentence = true;
        foreach(string text in optionTexts)
        {
            dialogueOptions.AddOption(text);
        }
    }
}
