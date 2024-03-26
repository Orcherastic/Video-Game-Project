using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBoxOptions : MonoBehaviour
{
    public DialogueOption dialogueOption;
    public string selectedText = string.Empty;
    public List<DialogueOption> options;
    int selectedOption = 0;

    private void Update()
    {
        if(options.Count > 0)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                options[selectedOption].UnselectedOption();
                selectedOption++;
                if (selectedOption > options.Count - 1)
                    selectedOption = 0;
            }
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                options[selectedOption].UnselectedOption();
                selectedOption--;
                if (selectedOption < 0)
                    selectedOption = options.Count - 1;
            }

            options[selectedOption].SelectedOption();
            selectedText = options[selectedOption].text.text;
        }
    }

    public void AddOption(string text)
    {
        DialogueOption option = Instantiate(dialogueOption, transform);
        option.text.text = text;
        options.Add(option);
    }

    public void RemoveOptions()
    {
        foreach(DialogueOption option in options)
        {
            Destroy(option.gameObject);
        }
        options.Clear();
        selectedOption = 0;
    }
}
