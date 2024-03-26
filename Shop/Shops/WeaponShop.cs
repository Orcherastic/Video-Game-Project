using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShop : Shop
{
    private void Start()
    {
        base.Start();
        tabs = FindObjectOfType<WeaponShopTabArea>();
        magicShopItems.gameObject.SetActive(false);
        itemShopItems.gameObject.SetActive(false);
        weaponShopItems.gameObject.SetActive(false);
        talker = "Blacksmith";
    }

    public override void SetTalkerAndTalking()
    {
        numTalkedTo++;
        talking.Clear();
        if (numTalkedTo == 1)
        {
            talking.Add("Hello Warrior.");
            talking.Add("I am The Blacksmith.");
            talking.Add("This is for you.");
        }
        else
        {
            if (!talkSelected)
            {
                hasOptions = true;
                talking.Add("Hello Warrior.");
                optionTexts.Add("Shop");
                optionTexts.Add("Talk");
                optionTexts.Add("Leave");
            }
            else
            {
                hasOptions = false;
                if (numOfTalkSelected == 1)
                {
                    talking.Add("Umm...");
                    talking.Add("Be careful not to break your weapon.");
                    talking.Add("And if you do, come to me. I'll fix it for you.");
                }
                else
                {
                    talking.Add("Err...");
                }
            }
        }
    }
}
