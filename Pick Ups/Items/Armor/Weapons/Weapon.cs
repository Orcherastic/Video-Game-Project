using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Armor, IDescriptable
{
    [Header("Weapon Main Stats")]
    public int level;
    public int HP;
    public int maxHP;
    public float XP;
    public float maxXP;
    public int upgradePoints;

    [Header("Passive Stats")]
    public int poiseDamage;

    [Header("Weapon Upgradable Stats")]
    public int attack;
    public int durability;
    public int endurance;
    public int fire;
    public int ice;
    public int lightning;
    public int beast;
    public int flying;
    public int smash;

    [Header("Weapon Stats Threshold")]
    public int maxAttack;
    public int maxDurability;
    public int maxFire;
    public int maxIce;
    public int maxLightning;
    public int maxBeast;
    public int maxFlying;
    public int maxSmash;

    [Header("Broken Weapon Sprite")]
    public Sprite brokenSprite;
    public bool weaponBroken = false;

    [Header("Build Up Options")]
    public List<Weapon> buildUpWeaponOptions = new List<Weapon>();

    public void SaveStats()
    {
        WeaponStatsManager.MyInstance.AddWeapon(this);
    }

    public void LoadStats(int numOfWeapon)
    {
        WeaponStatsContainer stats = new WeaponStatsContainer();
        stats = WeaponStatsManager.MyInstance.weaponStats[numOfWeapon];
        level = stats.level;
        maxXP = stats.maxXP;
        XP = stats.XP;
        maxHP = stats.maxHP;
        HP = stats.HP;
        upgradePoints = stats.upgradePoints;

        attack = stats.attack;
        durability = stats.durability;
        endurance = stats.endurance;
        fire = stats.fire;
        ice = stats.ice;
        lightning = stats.lightning;
        beast = stats.beast;
        flying = stats.flying;
        smash = stats.smash;
    }

    public override string GetItemType()
    {
        return "Weapon\n";
    }
    public override string GetItemStats1()
    {
        string finalString = "";
        finalString += "Attack\n" + "Fire\n" + "Ice\n" + "Lightning";
        return finalString;
    }

    public override string GetItemStats1Amount()
    {
        string finalString = "";
        finalString += attack + "\n" + fire + "\n" + ice + "\n" + lightning;
        return finalString;
    }

    public override string GetItemStats1Comparison()
    {
        string finalString = "";
        string comparison = "";
        string green = "#00FF00", red = "#FF0000", yellow = "#FFFF00";
        string color = string.Empty;

        // ATTACk
        int res = attack - FindEquipedWeaponStat("attack");
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

        // FIRE
        res = fire - FindEquipedWeaponStat("fire");
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

        // ICE
        res = ice - FindEquipedWeaponStat("ice");
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

        // LIGHTNING
        res = lightning - FindEquipedWeaponStat("lightning");
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
        finalString += "Durability\n" + "Beast\n" + "Flying\n" + "Smash";
        return finalString;
    }

    public override string GetItemStats2Amount()
    {
        string finalString = "";
        finalString += durability + "\n" + beast + "\n" + flying + "\n" + smash;
        return finalString;
    }

    public override string GetItemStats2Comparison()
    {
        string finalString = "";
        string comparison = "";
        string green = "#00FF00", red = "#FF0000", yellow = "#FFFF00";
        string color = string.Empty;

        // DuRABILITY
        int res = durability - FindEquipedWeaponStat("durability");
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

        // BEAST
        res = beast - FindEquipedWeaponStat("beast");
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

        // FLYING
        res = flying - FindEquipedWeaponStat("flying");
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

        // SMASH
        res = smash - FindEquipedWeaponStat("smash");
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
        int res;

        stat = "ATK ";
        value = attack.ToString();
        res = attack - FindEquipedWeaponStast("attack");
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

        stat = "DUR ";
        value = durability.ToString();
        res = durability - FindEquipedWeaponStast("durability");
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

        stat = "BST ";
        value = beast.ToString();
        res = beast - FindEquipedWeaponStast("beast");
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

        stat = "FLY ";
        value = flying.ToString();
        res = flying - FindEquipedWeaponStast("flying");
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

        stat = "SMH ";
        value = smash.ToString();
        res = smash - FindEquipedWeaponStast("smash");
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

        stat = "FIRE ";
        value = fire.ToString();
        res = fire - FindEquipedWeaponStast("fire");
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

        stat = "ICE ";
        value = ice.ToString();
        res = ice - FindEquipedWeaponStast("ice");
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

        stat = "LIGH ";
        value = lightning.ToString();
        res = lightning - FindEquipedWeaponStast("lightning");
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

        return base.GetDiscription() + "\n" + statsString;*/
        return "";
    }

    public int FindEquipedWeaponStat(string stat)
    {
        EquipSlots[] slots = FindObjectsOfType<EquipSlots>();
        foreach (EquipSlots slot in slots)
        {
            if (slot.armorType == ArmorType.Weapon)
            {
                if (slot.gameObject.GetComponentInChildren<InventoryItem>().IsEmpty)
                {
                    return 0;
                }
                else
                {
                    Weapon equipedItem =
                        (slot.gameObject.GetComponentInChildren<InventoryItem>().MyItem as Weapon);
                    switch (stat)
                    {
                        case "attack":
                            return equipedItem.attack;
                        case "durability":
                            return equipedItem.durability;
                        case "beast":
                            return equipedItem.beast;
                        case "flying":
                            return equipedItem.flying;
                        case "smash":
                            return equipedItem.smash;
                        case "fire":
                            return equipedItem.fire;
                        case "ice":
                            return equipedItem.ice;
                        case "lightning":
                            return equipedItem.lightning;
                    }
                }
            }
        }

        return 0;
    }
}
