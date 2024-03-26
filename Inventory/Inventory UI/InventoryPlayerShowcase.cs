using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryPlayerShowcase : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!Player.MyInstance.interacting)
        {
            //PlayerUI.MyInstance.ActivatePlayerStats();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //PlayerUI.MyInstance.DeactivatePlayerStats();
    }
}
