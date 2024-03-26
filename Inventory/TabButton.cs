using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TabButton : MonoBehaviour, IPointerClickHandler
{
    public TabGroup group;
    public Image background;

    void Start()
    {
        background = GetComponent<Image>();
        group.Subscribe(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        group.OnTabSelected(this);
    }
}
