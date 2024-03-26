using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WeaponBuildUpSlot : MonoBehaviour, IPointerClickHandler
{
    public Image thisImage;
    public Weapon weapon;
    public Image weaponSprite;

    public Sprite idleSprite;
    public Sprite selectedSprite;
    public Sprite buildUpAvailableSprite;

    private void Start()
    {
        thisImage.sprite = idleSprite;
        weaponSprite.color = Color.black;
    }

    private void Update()
    {
        if(thisImage.sprite == buildUpAvailableSprite)
            weaponSprite.color = Color.white;
        else
            weaponSprite.color = Color.black;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (thisImage.sprite == idleSprite)
            {
                PlayerUI.MyInstance.ResetWeaponStatsColor();

                if (PlayerUI.MyInstance.CompareWithEquipedWeapon(this.weapon))
                    thisImage.sprite = buildUpAvailableSprite;
                else
                    thisImage.sprite = selectedSprite;

                WeaponBuildUpLayout.MyInstance.SetWeaponSlotsSprites(this);
            }
                
            else if(thisImage.sprite == selectedSprite)
            {
                thisImage.sprite = idleSprite;
                PlayerUI.MyInstance.ResetWeaponStatsColor();
            }
        }
    }
}
