using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TextType
{
    none,
    damage,
    gainXP,
    levelUp,
    wpLevelUp,
    healHP,
    healMP,
    gainCoin,
    loseCoin
}

public class CombatTextManager : MonoBehaviour
{
    private static CombatTextManager instance;

    public static CombatTextManager MyInstance
    {
        get 
        {
            if(instance == null)
            {
                instance = FindObjectOfType<CombatTextManager>();
            }
            return instance;
        }
    }

    [SerializeField]
    private GameObject combatTextPrefab;
    [SerializeField]
    private GameObject combatTextPrefabWorld;

    private Canvas canvas;

    private void Start()
    {
        canvas = GetComponent<Canvas>();
    }

    public void CreateText(Vector2 position, string text, TextType type)
    {
        if (text != "0")
        {
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            Text t = Instantiate(combatTextPrefab, transform).GetComponent<Text>();
            t.transform.position = position;
            string operation = string.Empty;
            string after = string.Empty;

            switch (type)
            {
                case TextType.none:
                    t.color = Color.white;
                    break;
                case TextType.damage:
                    operation += "-";
                    t.color = Color.red;
                    break;
                case TextType.gainXP:
                    operation += "+";
                    t.color = Color.yellow;
                    after += "XP";
                    break;
                case TextType.levelUp:
                    operation += "";
                    t.color = Color.white;
                    after += "";
                    break;
                case TextType.wpLevelUp:
                    operation += "";
                    t.color = Color.magenta;
                    after += "";
                    break;
                case TextType.healHP:
                    operation += "+";
                    t.color = Color.green;
                    break;
                case TextType.healMP:
                    operation += "+";
                    t.color = Color.cyan;
                    break;
                case TextType.gainCoin:
                    operation += "+";
                    t.color = Color.yellow;
                    after += "G";
                    break;
                case TextType.loseCoin:
                    operation += "-";
                    t.color = Color.yellow;
                    after += "G";
                    break;
            }

            t.text = operation + text + after;
        }
    }

    public void CreateTextWorld(Vector2 position, string text, TextType type)
    {
        if (text != "0")
        {
            canvas.renderMode = RenderMode.WorldSpace;
            Text t = Instantiate(combatTextPrefabWorld, transform).GetComponent<Text>();
            t.transform.position = position;
            string operation = string.Empty;
            string after = string.Empty;

            switch (type)
            {
                case TextType.damage:
                    operation += "-";
                    t.color = Color.red;
                    break;
                case TextType.gainXP:
                    operation += "+";
                    t.color = Color.yellow;
                    after += "XP";
                    break;
                case TextType.levelUp:
                    operation += "";
                    t.color = Color.white;
                    after += "";
                    break;
                case TextType.wpLevelUp:
                    operation += "";
                    t.color = Color.magenta;
                    after += "";
                    break;
                case TextType.healHP:
                    operation += "+";
                    t.color = Color.green;
                    break;
                case TextType.healMP:
                    operation += "+";
                    t.color = Color.cyan;
                    break;
                case TextType.gainCoin:
                    operation += "+";
                    t.color = Color.yellow;
                    after += "G";
                    break;
                case TextType.loseCoin:
                    operation += "-";
                    t.color = Color.yellow;
                    after += "G";
                    break;
            }

            t.text = operation + text + after;
        }
    }
}
