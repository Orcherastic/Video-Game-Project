using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MagicSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IDescriptable
{
    public Image background;
    public Image myMagicSprite;
    public string myMagicName;
    public string myMagicDescription;
    public bool selected = false;
    public bool unlocked = false;

    private Vector3 off;

    void Update()
    {
        if (unlocked)
            myMagicSprite.color = Color.white;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (unlocked)
            {
                if (InventoryMagicUseSlots.MyInstance.AddMagicToSlot(myMagicSprite.sprite, myMagicName))
                    selected = true;
                else
                    selected = false;
            }
        }
    }

    public string GetDiscription()
    {
        return myMagicName + "\n\n" + myMagicDescription;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(unlocked)
        {
            off = new Vector3(-36, 36, off.z);
            PlayerUI.MyInstance.ActivateTooltip2(transform.position + off, this);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PlayerUI.MyInstance.DeactivateTooltip2();
    }
}
