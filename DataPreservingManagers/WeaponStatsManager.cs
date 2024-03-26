using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStatsManager : MonoBehaviour
{
    public List<WeaponStatsContainer> weaponStats = new List<WeaponStatsContainer>();

    public static WeaponStatsManager MyInstance;

    void Awake()
    {
        if (MyInstance == null)
        {
            MyInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddWeapon(Weapon weapon)
    {
        WeaponStatsContainer stats = new WeaponStatsContainer();
        // MAIN STATS
        stats.level = weapon.level;
        stats.maxXP = weapon.maxXP;
        stats.XP = weapon.XP;
        stats.maxHP = weapon.maxHP;
        stats.HP = weapon.HP;
        stats.upgradePoints = weapon.upgradePoints;
        // UPGRADABLE STATS
        stats.attack = weapon.attack;
        stats.durability = weapon.durability;
        stats.endurance = weapon.endurance;
        stats.fire = weapon.fire;
        stats.ice = weapon.ice;
        stats.lightning = weapon.lightning;
        stats.beast = weapon.beast;
        stats.flying = weapon.flying;
        stats.smash = weapon.smash;

        weaponStats.Add(stats);
    }
}
