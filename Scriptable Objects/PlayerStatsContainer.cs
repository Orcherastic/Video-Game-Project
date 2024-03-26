using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "ScriptableObjects/PlayerStatsContainer", order = 1)]
public class PlayerStatsContainer : ScriptableObject
{
    [Header("Level Stats")]
    public int level = 0;
    public float maxXP = 30;
    public float XP = 0;
    public int upgratablePoints = 0;

    [Header("Main Stats")]
    public int spirit = 5;
    public int mind = 5;
    public int resistance = 5;
    public int sorcery = 5;

    [Header("Equipment Stats")]
    public int damage = 0;
    public int maxPoise = 0;

    [Header("Upgratable Stats")]
    public int maxHealth = 20;
    public int health = 20;
    public int maxMana = 5;
    public int mana = 5;
    public int magicDamage = 10;
    public int physicalDefence = 10;
    public int fireDefence = 10;
    public int iceDefence = 10;
    public int lightningDefence = 10;
    public int poisonResistance = 10;
    public int decayResistance = 10;
    public int petrifyResistance = 10;
    public int fortune = 0;
    public int luck = 0;

    [Header("Coins")]
    public int coins = 0;

    public void Reset()
    {
        level = 0;
        maxXP = 30;
        XP = 0;
        upgratablePoints = 0;

        spirit = 5;
        mind = 5;
        resistance = 5;
        sorcery = 5;

        damage = 0;
        maxPoise = 0;

        maxHealth = 20;
        health = 20;
        maxMana = 5;
        mana = 5;
        magicDamage = 10;
        physicalDefence = 10;
        fireDefence = 10;
        iceDefence = 10;
        lightningDefence = 10;
        poisonResistance = 10;
        decayResistance = 10;
        petrifyResistance = 10;
        fortune = 0;
        luck = 0;

        coins = 0;
    }
}
