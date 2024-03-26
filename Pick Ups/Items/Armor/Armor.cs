using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ArmorType
{
    Helmet,
    Chestplate,
    Leggings,
    Boots,
    Weapon,
    Accessory
}

public class Armor : Item
{
    [Header("Armor Props")]
    public ArmorType armorType;
    [Header("Stat Boosts")]
    public int health;
    public int mana;
    public int damage;
    public int poise;
    public int magicDamage;
    public int physicalDefence;
    public int fireDefence;
    public int iceDefence;
    public int lightningDefence;
    public int poisonResistance;
    public int decayResistance;
    public int petrifyResistance;
    public int fortune;
    public int luck;

    [Header("Armor Animations")]
    [SerializeField]
    private AnimationClip[] animations;

    public AnimationClip[] Animations
    {
        get { return animations; }
    }

    public override string GetItemStats1()
    {
        string finalString = "";
        finalString += "Phy. Def.\n" + "Fire Def.\n" + "Ice Def.\n" + "Ligh. Def.\n" + "Fortune";
        return finalString;
    }

    public override string GetItemStats1Amount()
    {
        string finalString = "";
        finalString += physicalDefence + "\n" + fireDefence + "\n" + iceDefence + "\n" + lightningDefence + "\n" + fortune;
        return finalString;
    }

    public override string GetItemStats1Comparison()
    {
        string finalString = "";
        string comparison = "";
        string green = "#00FF00", red = "#FF0000", yellow = "#FFFF00";
        string color = string.Empty;

        // PHYSICAL DEFENCE
        int res = physicalDefence - FindEquipedItemStat("physicalDefence");
        if (res > 0)
        {
            comparison = "+" + res.ToString();
            color = green;
        }
        else if (res < 0)
        {
            comparison = res.ToString();
            color = red;
        }
        else
        {
            comparison = string.Empty;
            color = yellow;
        }
        finalString += string.Format("<color={0}>{1}</color>\n", color, comparison);

        // FIRE DEFENCE
        res = fireDefence - FindEquipedItemStat("fireDefence");
        if (res > 0)
        {
            comparison = "+" + res.ToString();
            color = green;
        }
        else if (res < 0)
        {
            comparison = res.ToString();
            color = red;
        }
        else
        {
            comparison = string.Empty;
            color = yellow;
        }
        finalString += string.Format("<color={0}>{1}</color>\n", color, comparison);

        // ICE DEFENCE
        res = iceDefence - FindEquipedItemStat("iceDefence");
        if (res > 0)
        {
            comparison = "+" + res.ToString();
            color = green;
        }
        else if (res < 0)
        {
            comparison = res.ToString();
            color = red;
        }
        else
        {
            comparison = string.Empty;
            color = yellow;
        }
        finalString += string.Format("<color={0}>{1}</color>\n", color, comparison);

        // LIGHTNING DEFENCE
        res = lightningDefence - FindEquipedItemStat("lightningDefence");
        if (res > 0)
        {
            comparison = "+" + res.ToString();
            color = green;
        }
        else if (res < 0)
        {
            comparison = res.ToString();
            color = red;
        }
        else
        {
            comparison = string.Empty;
            color = yellow;
        }
        finalString += string.Format("<color={0}>{1}</color>\n", color, comparison);

        // FORTUNE
        res = fortune - FindEquipedItemStat("fortune");
        if (res > 0)
        {
            comparison = "+" + res.ToString();
            color = green;
        }
        else if (res < 0)
        {
            comparison = res.ToString();
            color = red;
        }
        else
        {
            comparison = string.Empty;
            color = yellow;
        }
        finalString += string.Format("<color={0}>{1}</color>", color, comparison);
        return finalString;
    }

    public override string GetItemStats2()
    {
        string finalString = "";
        finalString += "Poise\n" + "Poison Res.\n" + "Decay Res.\n" + "Petrify Res.\n" + "Luck";
        return finalString;
    }

    public override string GetItemStats2Amount()
    {
        string finalString = "";
        finalString += poise + "\n" + poisonResistance + "\n" + decayResistance + "\n" + petrifyResistance + "\n" + luck;
        return finalString;
    }

    public override string GetItemStats2Comparison()
    {
        string finalString = "";
        string comparison = "";
        string green = "#00FF00", red = "#FF0000", yellow = "#FFFF00";
        string color = string.Empty;

        // POISE
        int res = poise - FindEquipedItemStat("poise");
        if (res > 0)
        {
            comparison = "+" + res.ToString();
            color = green;
        }
        else if (res < 0)
        {
            comparison = res.ToString();
            color = red;
        }
        else
        {
            comparison = string.Empty;
            color = yellow;
        }
        finalString += string.Format("<color={0}>{1}</color>\n", color, comparison);

        // POISON RESISTANCE
        res = poisonResistance - FindEquipedItemStat("poisonResistance");
        if (res > 0)
        {
            comparison = "+" + res.ToString();
            color = green;
        }
        else if (res < 0)
        {
            comparison = res.ToString();
            color = red;
        }
        else
        {
            comparison = string.Empty;
            color = yellow;
        }
        finalString += string.Format("<color={0}>{1}</color>\n", color, comparison);

        // DECAY RESISTANCE
        res = decayResistance - FindEquipedItemStat("decayResistance");
        if (res > 0)
        {
            comparison = "+" + res.ToString();
            color = green;
        }
        else if (res < 0)
        {
            comparison = res.ToString();
            color = red;
        }
        else
        {
            comparison = string.Empty;
            color = yellow;
        }
        finalString += string.Format("<color={0}>{1}</color>\n", color, comparison);

        // PETRIFY RESISTANCE
        res = petrifyResistance - FindEquipedItemStat("petrifyResistance");
        if (res > 0)
        {
            comparison = "+" + res.ToString();
            color = green;
        }
        else if (res < 0)
        {
            comparison = res.ToString();
            color = red;
        }
        else
        {
            comparison = string.Empty;
            color = yellow;
        }
        finalString += string.Format("<color={0}>{1}</color>\n", color, comparison);

        // LUCK
        res = luck - FindEquipedItemStat("luck");
        if (res > 0)
        {
            comparison = "+" + res.ToString();
            color = green;
        }
        else if (res < 0)
        {
            comparison = res.ToString();
            color = red;
        }
        else
        {
            comparison = string.Empty;
            color = yellow;
        }
        finalString += string.Format("<color={0}>{1}</color>", color, comparison);
        return finalString;
    }

    public override string GetDiscription()
    {
        /*string green = "#00FF00", red = "#FF0000", yellow = "#FFFF00";
        string stat = string.Empty;         // Shortened name of the Stat
        string value = string.Empty;        // Amount gained from Armor
        string comparison = string.Empty;   // Difference between this and equiped armor
        string color = string.Empty;        // Color of the comparison text
        string statsString = string.Empty;  // All of the above in 1 string
        if (armorType != ArmorType.Weapon)
        {
            if (health != 0)
            {
                stat = "HP ";
                if (health > 0)
                {
                    value = "+" + health.ToString();
                    color = green;
                }
                else
                {
                    value = health.ToString();
                    color = red;
                }
                statsString += string.Format("{0} <color={1}>{2}</color> \n", stat, color, value);
            }
            if (mana != 0)
            {
                stat = "MP ";
                if (mana > 0)
                {
                    value = "+" + mana.ToString();
                    color = green;
                }
                else
                {
                    value = mana.ToString();
                    color = red;
                }
                statsString += string.Format("{0} <color={1}>{2}</color> \n", stat, color, value);
            }
            if (damage != 0)
            {
                stat = "DMG ";
                value = damage.ToString();
                int res = damage - FindEquipedItemStat("damage");
                if (res > 0)
                {
                    comparison = "+" + res.ToString();
                    color = green;
                }
                else if (res < 0)
                {
                    comparison = res.ToString();
                    color = red;
                }
                else
                    comparison = string.Empty;
                statsString += string.Format("{0} <color={1}>{2}</color> <color={3}>{4}</color> \n", stat, yellow, value, color, comparison);
            }
            if (physicalDefence != 0)
            {
                stat = "DEF ";
                value = physicalDefence.ToString();
                int res = physicalDefence - FindEquipedItemStat("physicalDefence");
                if (res > 0)
                {
                    comparison = "+" + res.ToString();
                    color = green;
                }
                else if (res < 0)
                {
                    comparison = res.ToString();
                    color = red;
                }
                else
                    comparison = string.Empty;
                statsString += string.Format("{0} <color={1}>{2}</color> <color={3}>{4}</color> \n", stat, yellow, value, color, comparison);
            }
            if (magicDamage != 0)
            {
                stat = "MPDMG ";
                value = magicDamage.ToString();
                int res = magicDamage - FindEquipedItemStat("magicDamage");
                if (res > 0)
                {
                    comparison = "+" + res.ToString();
                    color = green;
                }
                else if (res < 0)
                {
                    comparison = res.ToString();
                    color = red;
                }
                else
                    comparison = string.Empty;
                statsString += string.Format("{0} <color={1}>{2}</color> <color={3}>{4}</color> \n", stat, yellow, value, color, comparison);
            }
            if (fireDefence != 0)
            {
                stat = "FIREDEF ";
                value = fireDefence.ToString();
                int res = fireDefence - FindEquipedItemStat("fireDefence");
                if (res > 0)
                {
                    comparison = "+" + res.ToString();
                    color = green;
                }
                else if (res < 0)
                {
                    comparison = res.ToString();
                    color = red;
                }
                else
                    comparison = string.Empty;
                statsString += string.Format("{0} <color={1}>{2}</color> <color={3}>{4}</color> \n", stat, yellow, value, color, comparison);
            }
            if (iceDefence != 0)
            {
                stat = "ICEDEF ";
                value = iceDefence.ToString();
                int res = iceDefence - FindEquipedItemStat("iceDefence");
                if (res > 0)
                {
                    comparison = "+" + res.ToString();
                    color = green;
                }
                else if (res < 0)
                {
                    comparison = res.ToString();
                    color = red;
                }
                else
                    comparison = string.Empty;
                statsString += string.Format("{0} <color={1}>{2}</color> <color={3}>{4}</color> \n", stat, yellow, value, color, comparison);
            }
            if (lightningDefence != 0)
            {
                stat = "LIGHDEF ";
                value = lightningDefence.ToString();
                int res = lightningDefence - FindEquipedItemStat("lightningDefence");
                if (res > 0)
                {
                    comparison = "+" + res.ToString();
                    color = green;
                }
                else if (res < 0)
                {
                    comparison = res.ToString();
                    color = red;
                }
                else
                    comparison = string.Empty;
                statsString += string.Format("{0} <color={1}>{2}</color> <color={3}>{4}</color> \n", stat, yellow, value, color, comparison);
            }
            if (poisonResistance != 0)
            {
                stat = "PSN";
                value = poisonResistance.ToString();
                int res = poisonResistance - FindEquipedItemStat("poisonResistance");
                if (res > 0)
                {
                    comparison = "+" + res.ToString();
                    color = green;
                }
                else if (res < 0)
                {
                    comparison = res.ToString();
                    color = red;
                }
                else
                    comparison = string.Empty;
                statsString += string.Format("{0} <color={1}>{2}</color> <color={3}>{4}</color> \n", stat, yellow, value, color, comparison);
            }
            if (decayResistance != 0)
            {
                stat = "DCY";
                value = decayResistance.ToString();
                int res = decayResistance - FindEquipedItemStat("decayResistance");
                if (res > 0)
                {
                    comparison = "+" + res.ToString();
                    color = green;
                }
                else if (res < 0)
                {
                    comparison = res.ToString();
                    color = red;
                }
                else
                    comparison = string.Empty;
                statsString += string.Format("{0} <color={1}>{2}</color> <color={3}>{4}</color> \n", stat, yellow, value, color, comparison);
            }
            if (petrifyResistance != 0)
            {
                stat = "PRY";
                value = petrifyResistance.ToString();
                int res = petrifyResistance - FindEquipedItemStat("petrifyResistance");
                if (res > 0)
                {
                    comparison = "+" + res.ToString();
                    color = green;
                }
                else if (res < 0)
                {
                    comparison = res.ToString();
                    color = red;
                }
                else
                    comparison = string.Empty;
                statsString += string.Format("{0} <color={1}>{2}</color> <color={3}>{4}</color> \n", stat, yellow, value, color, comparison);
            }
            if (fortune != 0)
            {
                stat = "FOR";
                value = fortune.ToString();
                int res = fortune - FindEquipedItemStat("fortune");
                if (res > 0)
                {
                    comparison = "+" + res.ToString();
                    color = green;
                }
                else if (res < 0)
                {
                    comparison = res.ToString();
                    color = red;
                }
                else
                    comparison = string.Empty;
                statsString += string.Format("{0} <color={1}>{2}</color> <color={3}>{4}</color> \n", stat, yellow, value, color, comparison);
            }
            if (luck != 0)
            {
                stat = "LCK";
                value = luck.ToString();
                int res = luck - FindEquipedItemStat("luck");
                if (res > 0)
                {
                    comparison = "+" + res.ToString();
                    color = green;
                }
                else if (res < 0)
                {
                    comparison = res.ToString();
                    color = red;
                }
                else
                    comparison = string.Empty;
                statsString += string.Format("{0} <color={1}>{2}</color> <color={3}>{4}</color> \n", stat, yellow, value, color, comparison);
            }*/
        /*}

        return base.GetDiscription() + " / " + armorType + "\n" +
            statsString;*/

        return "";
    }

    public int FindEquipedItemStat(string stat)
    {
        EquipSlots[] slots = FindObjectsOfType<EquipSlots>();
        foreach(EquipSlots slot in slots)
        {
            if (slot.armorType == armorType)
            {
                if (slot.gameObject.GetComponentInChildren<InventoryItem>().IsEmpty)
                {
                    return 0;
                }
                else
                {
                    Armor equipedItem = 
                        (slot.gameObject.GetComponentInChildren<InventoryItem>().MyItem as Armor);
                    switch (stat)
                    {
                        case "health":
                            return equipedItem.health;
                        case "mana":
                            return equipedItem.mana;
                        case "damage":
                            return equipedItem.damage;
                        case "magicDamage":
                            return equipedItem.magicDamage;
                        case "physicalDefence":
                            return equipedItem.physicalDefence;
                        case "fireDefence":
                            return equipedItem.fireDefence;
                        case "iceDefence":
                            return equipedItem.iceDefence;
                        case "lightningDefence":
                            return equipedItem.lightningDefence;
                        case "poise":
                            return equipedItem.poise;
                        case "poisonResistance":
                            return equipedItem.poisonResistance;
                        case "decayResistance":
                            return equipedItem.decayResistance;
                        case "petrifyResistance":
                            return equipedItem.petrifyResistance;
                        case "fortune":
                            return equipedItem.fortune;
                        case "luck":
                            return equipedItem.luck;
                    }
                }
            }
        }

        return 0;
    }

    public void ChangePlayerStats(bool equiped)
    {
        Player player = Player.MyInstance;
        if(equiped)
        {
            // Add Stat Buffs to Player Stats
            player.maxHealth            += health;
            player.maxMana              += mana;
            player.damage               += damage;
            player.maxPoise             += poise;
            player.currentPoise         = player.maxPoise;
            player.magicDamage          += magicDamage;
            player.physicalDefence      += physicalDefence;
            player.fireDefence          += fireDefence;
            player.iceDefence           += iceDefence;
            player.lightningDefence     += lightningDefence;
            player.poisonResistance     += poisonResistance;
            player.decayResistance      += decayResistance;
            player.petrifyResistance    += petrifyResistance;
            player.fortune              += fortune;
            player.luck                 += luck;

            player.baseMaxHealth            += health;
            player.baseMaxMana              += mana;
            player.baseMagicDamage          += magicDamage;
            player.basePhysicalDefence      += physicalDefence;
            player.baseFireDefence          += fireDefence;
            player.baseIceDefence           += iceDefence;
            player.baseLightningDefence     += lightningDefence;
            player.basePoisonResistance     += poisonResistance;
            player.baseDecayResistance      += decayResistance;
            player.basePetrifyResistance    += petrifyResistance;
            player.baseFortune              += fortune;
            player.baseLuck                 += luck;
        }
        else
        {
            // Subtract Stat Buffs from Player Stats
            player.maxHealth -= health;
            player.maxMana -= mana;
            player.damage -= damage;
            player.maxPoise -= poise;
            player.currentPoise = player.maxPoise;
            player.magicDamage -= magicDamage;
            player.physicalDefence -= physicalDefence;
            player.fireDefence -= fireDefence;
            player.iceDefence -= iceDefence;
            player.lightningDefence -= lightningDefence;
            player.poisonResistance -= poisonResistance;
            player.decayResistance -= decayResistance;
            player.petrifyResistance -= petrifyResistance;
            player.fortune -= fortune;
            player.luck -= luck;

            player.baseMaxHealth        -= health;
            player.baseMaxMana          -= mana;
            player.baseMagicDamage      -= magicDamage;
            player.basePhysicalDefence  -= physicalDefence;
            player.baseFireDefence      -= fireDefence;
            player.baseIceDefence       -= iceDefence;
            player.baseLightningDefence -= lightningDefence;
            player.basePoisonResistance -= poisonResistance;
            player.baseDecayResistance  -= decayResistance;
            player.basePetrifyResistance-= petrifyResistance;
            player.baseFortune          -= fortune;
            player.baseLuck             -= luck;
        }

        // Change Players Health, if max health is lower than current health, lower current health
        player.ui.SetMaxHealth(player.maxHealth);
        if (player.maxHealth < player.health)
        {
            player.health = player.maxHealth;
            player.ui.SetHealth(player.health);
        }

        // Change Players Mana, if max mana is lower than current mana, lower current mana
        player.ui.SetMaxMana(player.maxMana);
        if (player.maxMana < player.mana)
        {
            player.mana = player.maxMana;
            player.ui.SetMana(player.mana);
        }
    }
}
