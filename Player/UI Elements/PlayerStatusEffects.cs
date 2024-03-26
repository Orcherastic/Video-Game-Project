using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusEffects : MonoBehaviour
{
    public List<StatusEffect> StatusEffects = new List<StatusEffect>();
    // 0 = Damage Increase
    // 1 = Defence Increase
    // 2 = Health Increase
    // 3 = Luck Increase
    // 4 = Magic Defence Increase
    // 5 = Magic Increase
    // 6 = Mana Increase
    // 7 = Speed Increase
    // 8 = Bleed
    // 9 = Damage Decrease
    // 10 = Defence Decrease
    // 11 = Goo
    // 12 = Health Decrease
    // 13 = Luck Decrease
    // 14 = Magic Decrease
    // 15 = Magic Defence Decrease
    // 16 = Mana Decrease
    // 17 = Poison
    // 18 = Speed Decrease
    // 19 = Burn
    // 20 = Decay
    // 21 = Frostbite
    // 22 = Petrify
    // 23 = Shock
    // 24 = Defence Up Status Effect

    public List<StatusEffect> ActiveStatusEffects = new List<StatusEffect>();

    private Vector3 off;

    public void AddStatusEffect(int index)
    {
        foreach(StatusEffect se in ActiveStatusEffects)
        {
            if (se.statusEffectName == StatusEffects[index].statusEffectName)
            {
                // Goo does not stack
                if(se.statusEffectName != "Goo")
                {
                    se.numberOf++;
                    if(se.numberOfText != null)
                        se.numberOfText.text = se.numberOf.ToString();
                }
                return;
            }
        }
        StatusEffect statusEffect = Instantiate(StatusEffects[index], transform).GetComponent<StatusEffect>();
        statusEffect.numberOf++;
        statusEffect.OnEffect();
        ActiveStatusEffects.Add(statusEffect);
    }

    public void AddStatusEffectWithTimer(int index, float timer)
    {
        foreach (StatusEffect se in ActiveStatusEffects)
        {
            if (se.statusEffectName == StatusEffects[index].statusEffectName)
            {
                // If the Status Effect is Stackable
                if (se.numberOfText != null)
                {
                    se.numberOf++;
                    if(se.numberOf < 4)
                        se.numberOfText.text = se.numberOf.ToString();
                }
                return;
            }
        }
        StatusEffect statusEffect = Instantiate(StatusEffects[index], transform).GetComponent<StatusEffect>();
        statusEffect.numberOf++;
        statusEffect.ActivateTimer(timer);
        statusEffect.OnEffect();
        ActiveStatusEffects.Add(statusEffect);
    }

    public void RemoveStatusEffect(int index)
    {
        foreach (StatusEffect se in ActiveStatusEffects)
        {
            if (se.statusEffectName == StatusEffects[index].statusEffectName)
            {
                se.numberOf--;
                if(se.numberOfText != null)
                    se.numberOfText.text = se.numberOf.ToString();
                if (se.numberOf <= 0)
                {
                    ActiveStatusEffects.Remove(se);
                    se.OffEffect();
                    se.timeRemaining = 0;
                    Destroy(se.gameObject);
                }
                return;
            }
        }
    }

    public void RemoveStatusEffectWithName(string name)
    {
        for(int i=0; i<ActiveStatusEffects.Count; i++)
        {
            if(ActiveStatusEffects[i].statusEffectName == name)
            {
                StatusEffect se = ActiveStatusEffects[i];
                se.numberOf--;
                if (se.numberOfText != null)
                    se.numberOfText.text = se.numberOf.ToString();
                if (se.numberOf <= 0)
                {
                    ActiveStatusEffects.Remove(se);
                    se.OffEffect();
                    se.timeRemaining = 0;
                    se.OnPointerExit(se.ped);
                    Destroy(se.gameObject);
                }
                return;
            }
        }
    }

    public void RemoveStatusEffectCompletely(string name)
    {
        for (int i = 0; i < ActiveStatusEffects.Count; i++)
        {
            if (ActiveStatusEffects[i].statusEffectName == name)
            {
                StatusEffect se = ActiveStatusEffects[i];
                ActiveStatusEffects.Remove(se);
                se.OffEffect();
                se.timeRemaining = 0;
                se.OnPointerExit(se.ped);
                Destroy(se.gameObject);
                return;
            }
        }
    }
}
