using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MagicUseSlot : MonoBehaviour, IPointerClickHandler
{
    [Header("Background, Magic, Slot Number")]
    public Image background;
    public Image slotNumberBig;
    public Image slotNumberSmall;
    public Image myMagicSpriteBig;
    public Image myMagicSpriteSmall;
    public string mySpellName;
    [Header("Cooldown")]
    public Image cooldownSmall;
    public Image cooldownBig;
    public bool isCooldown = false;
    public float cooldownTime = 5f;
    [Header("Input Value")]
    public string useNumber;

    void Update()
    {
        if (transform.parent == InGameMagicUseSlots.myInstance.transform && myMagicSpriteBig.sprite != null)
        {
            Player player = Player.MyInstance;

            //if (Input.GetKeyDown(useNumber) && !isCooldown && player.currentState != PlayerState.interact
            //    && player.currentState != PlayerState.magicAttack && player.currentState != PlayerState.stagger
            //    && player.currentState != PlayerState.attack1 && player.currentState != PlayerState.attack2
            //    && player.currentState != PlayerState.attack3)

            if (Input.GetKeyDown(useNumber) && !isCooldown && 
                (player.currentState == PlayerState.run || player.currentState == PlayerState.walk))
            {
                player.CastSpell(mySpellName);
                if (player.currentState == PlayerState.fireAttack || player.currentState == PlayerState.iceAttack ||
                    player.currentState == PlayerState.lightningAttack || player.currentState == PlayerState.poisonAttack ||
                    player.currentState == PlayerState.decayAttack || player.currentState == PlayerState.petrifyAttack ||
                    player.currentState == PlayerState.buffAttack)
                {
                    isCooldown = true;
                    cooldownSmall.fillAmount = 1;
                    cooldownBig.fillAmount = 1;
                }
                else
                    StartCoroutine(EmptyMpBarCo());
            }
        }

        if (isCooldown)
        {
            cooldownSmall.fillAmount -= 1f / cooldownTime * Time.deltaTime;
            cooldownBig.fillAmount -= 1f / cooldownTime * Time.deltaTime;
            if (cooldownSmall.fillAmount <= 0)
            {
                cooldownSmall.fillAmount = 0;
                isCooldown = false;
            }
            if (cooldownBig.fillAmount <= 0)
            {
                cooldownBig.fillAmount = 0;
                isCooldown = false;
            }
            if(transform.parent == InGameMagicUseSlots.myInstance.transform && myMagicSpriteSmall.sprite != null)
            {
                cooldownBig.color = Color.clear;
                cooldownSmall.color = Color.grey;
            }
            else if(transform.parent == InventoryMagicUseSlots.MyInstance.transform && myMagicSpriteBig.sprite != null)
            {
                cooldownBig.color = Color.grey;
                cooldownSmall.color = Color.clear;
            }
            else
            {
                cooldownSmall.color = Color.clear;
                cooldownBig.color = Color.clear;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (myMagicSpriteBig.sprite != null && !isCooldown)
            {
                foreach(MagicSlot slot in InventoryMagicSlots.MyInstance.slots)
                {
                    if (slot.myMagicSprite.sprite == myMagicSpriteBig.sprite)
                        slot.selected = false;
                }
                myMagicSpriteBig.sprite = null;
                myMagicSpriteSmall.sprite = null;
                mySpellName = string.Empty;
            }
        }
    }

    public void SwitchCooldown(MagicUseSlot slot)
    {
        slot.isCooldown = true;
        isCooldown = false;
        slot.cooldownSmall.fillAmount = cooldownSmall.fillAmount;
        slot.cooldownBig.fillAmount = cooldownBig.fillAmount;
        cooldownSmall.fillAmount = 0;
        cooldownBig.fillAmount = 0;
    }

    private IEnumerator EmptyMpBarCo()
    {
        if(!Player.MyInstance.mpBar.GetComponent<Animator>().GetBool("Empty"))
            Player.MyInstance.mpBar.GetComponent<Animator>().SetBool("Empty", true);
        yield return new WaitForSeconds(0.2f);
        Player.MyInstance.mpBar.GetComponent<Animator>().SetBool("Empty", false);
    }
}
