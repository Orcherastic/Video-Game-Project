using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerStat : MonoBehaviour, IDescriptable, IPointerEnterHandler, IPointerExitHandler
{
    public string Description;
    private Vector3 off;
    public string GetDiscription()
    {
        return string.Format("<b>{0}</b>", Description);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        off = new Vector3(-30, 30, off.z);
        PlayerUI.MyInstance.ActivateTooltip2(transform.position + off, this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PlayerUI.MyInstance.tooltip.transform.SetParent(Player.MyInstance.inventory.transform);
        PlayerUI.MyInstance.DeactivateTooltip2();
    }
}
