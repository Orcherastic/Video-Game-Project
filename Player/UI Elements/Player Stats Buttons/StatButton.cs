using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatButton : MonoBehaviour
{
    protected Button thisButton;
    // Start is called before the first frame update
    void Start()
    {
        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(OnClickButton);
    }

    public virtual void OnClickButton()
    {
        
    }
}
