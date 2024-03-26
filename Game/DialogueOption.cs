using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueOption : MonoBehaviour
{
    public Image image;
    public Text text;

    public void SelectedOption()
    {
        image.color = Color.white;
    }

    public void UnselectedOption()
    {
        image.color = Color.clear;
    }
}
