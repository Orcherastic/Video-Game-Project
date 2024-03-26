using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemShop : Shop
{
    private void Start()
    {
        base.Start();
        tabs = FindObjectOfType<ItemShopTabArea>();
        talker = "Merchant";
    }

    public override void SetTalkerAndTalking()
    {
        numTalkedTo++;
        talking.Clear();
        if (numTalkedTo == 1)
        {
            talking.Add("Welcome Warrior, and feast your eyes!");
            talking.Add("I am The Coin Loving, Trade Seeking and ever so Humble Merchant!");
            talking.Add("Unfortunately, as you can see, buisness has been rather slow lately.");
            talking.Add("However, now that you're here, things may finally turn for the better!");
            talking.Add("Oh yeah, I almost forgot, I'm suppose to give you this.");
            talking.Add("I don't really like giving things for free you see, it ruins the trade, " +
                "but someone was very persuasive.");
        }
        else
        {
            if (!talkSelected)
            {
                hasOptions = true;
                talking.Add("Welcome Warrior. See anything you like?");
                optionTexts.Add("Shop");
                optionTexts.Add("Talk");
                optionTexts.Add("Leave");
            }
            else
            {
                hasOptions = false;
                if(numOfTalkSelected == 1)
                {
                    talking.Add("You're probably wondering what we are doing here,\n" +
                        "or how we got here, or who we even are.");
                    talking.Add("Well, you see, we are not actually humans.");
                    talking.Add("We just took these forms to appear more friendly.");
                    talking.Add("But don't be alarmed, we mean you no harm,\n" +
                        "we simply like coin, especially me.");
                }
                else if(numOfTalkSelected == 2)
                {
                    talking.Add("This place used to be brimming with Warriors.");
                    talking.Add("One after another, each more determined than the last.");
                    talking.Add("But then it just...");
                    talking.Add("Stopped.");
                    talking.Add("For so long, no one was coming.");
                    talking.Add("So... so... long.");
                }
                else
                {
                    talking.Add("Oh, I'm sorry. I got emotional again.\n" +
                        "Don't mind me.");
                }
            }
        }
    }
}
