using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponStats", menuName = "ScriptableObjects/WeaponStats", order = 1)]
public class WeaponStatsContainer : ScriptableObject
{
    [Header("Weapon Main Stats")]
    public int level = 0;
    public int HP = 40;
    public int maxHP = 40;
    public float XP = 0;
    public float maxXP = 20;
    public int upgradePoints = 0;

    [Header("Weapon Upgradable Stats")]
    public int attack = 1;
    public int durability = 1;
    public int endurance = 1;
    public int fire = 0;
    public int ice = 0;
    public int lightning = 0;
    public int beast = 0;
    public int flying = 0;
    public int smash = 0;
}
