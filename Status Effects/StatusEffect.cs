using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StatusEffect : MonoBehaviour, IDescriptable, IPointerEnterHandler, IPointerExitHandler
{
    public string statusEffectName;
    public int numberOf = 0;
    private bool showTooltip = false;
    public Text numberOfText = null;
    [Header("Timer")]
    public float timeRemaining = 0f;
    public bool timeIsRunning = false;
    public string timeText;
    public PointerEventData ped = null;
    [Header("Entity")]
    public bool player = false;

    private Vector3 off;

    void FixedUpdate()
    {
        if(showTooltip)
            PlayerUI.MyInstance.ActivateTooltip2(transform.position + off, this);

        if (timeIsRunning)
        {
            if(timeRemaining >= 0)
            {
                DisplayTime(timeRemaining);
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                timeIsRunning = false;
                RemoveFromParent();
            }
        }

        if (numberOf > 3)
            numberOf = 3;

        if (numberOfText != null)
            if (numberOf > 1)
                numberOfText.gameObject.SetActive(true);
            else
                numberOfText.gameObject.SetActive(false);
    }

    public virtual void OnEffect()
    {

    }
    public virtual void OffEffect()
    {

    }

    public void ActivateTimer(float time)
    {
        timeRemaining = time;
        timeIsRunning = true;
    }

    public void DisplayTime(float timeToDisplay)
    {
        //timeToDisplay -= 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText = string.Format("{0:00} : {1:00}", minutes, seconds);
    }

    public virtual string GetDiscription()
    {
        string color = string.Empty;
        switch (statusEffectName)
        {
            case "Goo":
                color = "#8a00c2";
                break;
            case "Poison":
                color = "#354A21";
                break;
            case "Burn":
                color = "#fe2020";
                break;
            default:
                color = "#000000";
                break;
        }
        return string.Format("<b><color={1}>{0}</color></b>", statusEffectName, color);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ped = eventData;
        //if (Player.MyInstance.currentState != PlayerState.interact)
        //{
            PlayerUI.MyInstance.tooltip2.transform.SetParent(transform);
            off = new Vector3(190, -90, off.z);
            showTooltip = true;
        //}
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        showTooltip = false;
        PlayerUI.MyInstance.tooltip2.transform.SetParent(Player.MyInstance.inventory.transform);
        PlayerUI.MyInstance.DeactivateTooltip2();
    }

    public void RemoveFromParent()
    {
        if(transform.parent.gameObject.CompareTag("Player Status Effects"))
        {
            Player.MyInstance.statusEffects.RemoveStatusEffectCompletely(statusEffectName);
        }
        else if (transform.parent.gameObject.CompareTag("Enemy Status Effects"))
        {
            transform.root.gameObject.GetComponent<Enemy>().statusEffects.RemoveStatusEffectCompletely(statusEffectName);
        }
    }
}
