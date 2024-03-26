using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicShop : Shop
{
    private void Awake()
    {
        base.Awake();
        tabs = FindObjectOfType<MagicShopTabArea>();
        talker = "Wizard";
    }

    public override void SetTalkerAndTalking()
    {
        numTalkedTo++;
        talking.Clear();
        optionTexts.Clear();
        if (numTalkedTo == 1)
        {
            hasOptions = false;
            optionTexts.Clear();
            talking.Add("Greetings Warrior, and welcome to our humble abode.");
            talking.Add("I am The Wizard of this Dungeon, and as such,\n" +
                "I deal with the art of Spells and Potion Brewing.") ;
            talking.Add("Speaking of which, I believe a reward for you is in order,\n" +
                "for your strength and valour.");
        }
        else
        {
            if (!talkSelected)
            {
                hasOptions = true;
                talking.Add("Greetings Warrior. How may I be of service?");
                optionTexts.Add("Shop");
                optionTexts.Add("Talk");
                optionTexts.Add("Leave");
            }
            else
            {
                hasOptions = false;
                talking.Add("For as long as you remain here, you will be referred to as 'Warrior'.");
                talking.Add("Your real name, your family name, your age, wealth\n" +
                    "where you come from, none of that is of relevance.");
                talking.Add("Here, you are a Warrior and nothing more.");
            }
        }
    }
}
