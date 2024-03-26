using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    public List<TabButton> tabs;
    public Sprite tabIdle;
    public Sprite tabSelected;
    public List<GameObject> pages;

    void Update()
    {
        if(!Player.MyInstance.openInventory && !Player.MyInstance.shopping)
        {
            SetFirstTab();
        }
    }

    public void Subscribe(TabButton button)
    {
        if(tabs == null)
        {
            tabs = new List<TabButton>();
        }

        tabs.Add(button);
    }
    public void OnTabSelected(TabButton button)
    {
        ResetTabs();
        button.background.sprite = tabSelected;
        int index = button.transform.GetSiblingIndex();
        for(int i=0; i<pages.Count; i++)
        {
            if (i == index)
                pages[i].transform.SetAsLastSibling();
        }
    }
    public void ResetTabs()
    {
        foreach (TabButton tab in tabs)
            tab.background.sprite = tabIdle;
    }

    public void SetFirstTab()
    {
        TabButton firstTab = transform.GetChild(0).GetComponent<TabButton>();
        OnTabSelected(firstTab);
    }
}
