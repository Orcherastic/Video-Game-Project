using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    private static PlayerUI instance;

    public static PlayerUI MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerUI>();
            }
            return instance;
        }
    }

    [Header("XP")]
    public Slider XPSlider;
    public Text level;
    public Text currentXP;
    public Text maxXP;
    public Text upgradePoints;

    [Header("HP")]
    public Slider healthSlider;
    public Text currentHP;
    public Text maxHP;

    [Header("MP")]
    public Slider manaSlider;
    public Text currentMP;
    public Text maxMP;

    [Header("Weapon Bar Sprite")]
    public Image weaponUISprite;

    [Header("WEAPON XP")]
    public Slider WXPSlider;
    public Text wpUpgradePoints;
    //public Text WLevel;
    //public Text currentWXP;
    //public Text maxWXP;

    [Header("WEAPON HP")]
    public Slider WHealthSlider;
    public Text currentWHP;
    public Text maxWHP;

    [Header("COINS")]
    public GameObject playerCoins;
    public Text currentCoins;

    [Header("TOOLTIP")]
    public GameObject tooltip;
    public GameObject tooltip2;
    public Text tooltipText;
    public Text tooltipItemNameText;
    public Text tooltipItemTypeText;
    public Text tooltipItemStats1Text;
    public Text tooltipItemStats1AmountText;
    public Text tooltipItemStats1ComparisonText;
    public Text tooltipItemStats2Text;
    public Text tooltipItemStats2AmountText;
    public Text tooltipItemStats2ComparisonText;
    public Text tooltipItemDescription;

    [Header("Player Level Stats")]
    public Text playerLevel;
    public Text playerMaxXP;
    public Text playerCurrentXP;
    public Text playerUpgradePoints;

    [Header("Player Main Stats")]
    public Text playerSpirit;
    public Text playerMind;
    public Text playerResistance;
    public Text playerSorcery;

    [Header("Player Upgratable Stats")]
    public GameObject playerStats;
    public Text playerHealth;
    public Text playerDamage;
    public Text playerPoise;
    public Text playerMagicDamage;
    public Text playerMana;
    public Text playerDefence;
    public Text playerFireDefence;
    public Text playerIceDefence;
    public Text playerLightningDefence;
    public Text playerPoisonResistance;
    public Text playerDecayResistance;
    public Text playerPetrifyResistance;
    public Text playerFortune;
    public Text playerLuck;

    [Header("Weapon Showcase")]
    public Image weaponSprite;

    [Header("Weapon Level Stats")]
    public Text weaponLevel;
    public Text weaponMaxXP;
    public Text weaponCurrentXP;
    public Text weaponMaxHP;
    public Text weaponCurrentHP;
    public Text weaponUpgradePoints;

    [Header("Weapon Upgradable Stats")]
    public Text weaponAttack;
    public Text weaponDurability;
    public Text weaponFire;
    public Text weaponBeast;
    public Text weaponIce;
    public Text weaponFlying;
    public Text weaponLightning;
    public Text weaponSmash;

    [Header("Weapon Build Up Layout")]
    public WeaponBuildUpLayout weaponBuildUpLayout;
    public bool buildUpActive = false;

    [Header("Player Death")]
    public GameObject gameOverText;

    [Header("Key Items")]
    public KeyItemSlot keyItemSlot;

    // XP
    public void SetLevel(int lv)
    {
        level.text = lv.ToString();
    }
    public void SetMaxXP(float xp)
    {
        maxXP.text = xp.ToString();
        XPSlider.maxValue = xp;
    }
    public void SetXP(float xp)
    {
        currentXP.text = xp.ToString();
        XPSlider.value = xp;
    }

    // HP
    public void SetMaxHealth(int health)
    {
        maxHP.text = health.ToString();
        healthSlider.maxValue = health;
        //healthSlider.value = health;
    }
    public void SetHealth(int health)
    {
        currentHP.text = health.ToString();
        healthSlider.value = health;
    }

    // MP
    public void SetMaxMana(int mana)
    {
        maxMP.text = mana.ToString();
        manaSlider.maxValue = mana;
        //manaSlider.value = mana;
    }
    public void SetMana(int mana)
    {
        currentMP.text = mana.ToString();
        manaSlider.value = mana;
    }

    // WEAPON XP
    public void SetMaxWeaponXP(float xp)
    {
        //maxWXP.text = xp.ToString();
        WXPSlider.maxValue = xp;
    }
    public void SetWeaponXP(float xp)
    {
        //currentWXP.text = xp.ToString();
        WXPSlider.value = xp;
    }

    // WEAPON HP
    public void SetMaxWeaponHealth(int health)
    {
        maxWHP.text = health.ToString();
        WHealthSlider.maxValue = health;
        //WHealthSlider.value = health;
    }
    public void SetWeaponHealth(int health)
    {
        currentWHP.text = health.ToString();
        WHealthSlider.value = health;
    }

    //WEAPON BUILD UP
    public void SetWeaponBuildUpLayout()
    {
        if(Player.MyInstance.equipedWeapon)
        {
            buildUpActive = !buildUpActive;
            if (buildUpActive)
            {
                weaponBuildUpLayout.RemoveWeaponBuildUpSlots();
                weaponBuildUpLayout.AddWeaponBuildUpSlots();
            }
            else
            {
                weaponBuildUpLayout.RemoveWeaponBuildUpSlots();
                ResetWeaponStatsColor();
            }

            weaponBuildUpLayout.gameObject.SetActive(buildUpActive);
        }
    }
    public bool CompareWithEquipedWeapon(Weapon weapon)
    {
        bool buildUpAvailable = true;
        Weapon playerWeapon = Player.MyInstance.equipedWeapon;
        if (weapon.attack > playerWeapon.attack)
        {
            weaponAttack.color = Color.red;
            buildUpAvailable = false;
        }
        if (weapon.durability > playerWeapon.durability)
        {
            weaponDurability.color = Color.red;
            buildUpAvailable = false;
        }
        if (weapon.fire > playerWeapon.fire)
        {
            weaponFire.color = Color.red;
            buildUpAvailable = false;
        }
        if (weapon.ice > playerWeapon.ice)
        {
            weaponIce.color = Color.red;
            buildUpAvailable = false;
        }
        if (weapon.lightning > playerWeapon.lightning)
        {
            weaponLightning.color = Color.red;
            buildUpAvailable = false;
        }
        if (weapon.beast > playerWeapon.beast)
        {
            weaponBeast.color = Color.red;
            buildUpAvailable = false;
        }
        if (weapon.flying > playerWeapon.flying)
        {
            weaponFlying.color = Color.red;
            buildUpAvailable = false;
        }
        if (weapon.smash > playerWeapon.smash)
        {
            weaponSmash.color = Color.red;
            buildUpAvailable = false;
        }

        return buildUpAvailable;
    }
    public void ResetWeaponStatsColor()
    {
        weaponAttack.color =        Color.black;
        weaponDurability.color =    Color.black;
        weaponFire.color =          Color.black;
        weaponIce.color =           Color.black;
        weaponLightning.color =     Color.black;
        weaponBeast.color =         Color.black;
        weaponFlying.color =        Color.black;
        weaponSmash.color =         Color.black;

        if(WeaponUpgradeSlot.MyInstance.myItem != null)
        {
            Player.MyInstance.DowngradeWeaponStats(WeaponUpgradeSlot.MyInstance.myItem as MonsterLoot);
            Player.MyInstance.UpgradeWeaponStats(WeaponUpgradeSlot.MyInstance.myItem as MonsterLoot);
        }
    }
    public void BuildUpWeapon()
    {
        foreach(WeaponBuildUpSlot slot in WeaponBuildUpLayout.MyInstance.slots)
        {
            if(slot.thisImage.sprite == slot.buildUpAvailableSprite)
            {
                Weapon playerWeapon = Player.MyInstance.equipedWeapon;
                Weapon buildUpWeapon = Instantiate(slot.weapon, Player.MyInstance.transform);
                // STAT BOOST
                if (buildUpWeapon.attack > 0)
                {
                    playerWeapon.attack += 1;
                }
                if (buildUpWeapon.durability > 0)
                {
                    playerWeapon.durability += 1;
                }
                if (buildUpWeapon.fire > 0)
                {
                    playerWeapon.fire += 1;
                }
                if (buildUpWeapon.ice > 0)
                {
                    playerWeapon.ice += 1;
                }
                if (buildUpWeapon.lightning > 0)
                {
                    playerWeapon.lightning += 1;
                }
                if (buildUpWeapon.beast > 0)
                {
                    playerWeapon.beast += 1;
                }
                if (buildUpWeapon.flying > 0)
                {
                    playerWeapon.flying += 1;
                }
                if (buildUpWeapon.smash > 0)
                {
                    playerWeapon.smash += 1;
                }

                buildUpWeapon.attack = playerWeapon.attack;
                buildUpWeapon.durability = playerWeapon.durability;
                buildUpWeapon.fire = playerWeapon.fire;
                buildUpWeapon.ice = playerWeapon.ice;
                buildUpWeapon.lightning = playerWeapon.lightning;
                buildUpWeapon.beast = playerWeapon.beast;
                buildUpWeapon.flying = playerWeapon.flying;
                buildUpWeapon.smash = playerWeapon.smash;

                EquipSlots[] slots = FindObjectsOfType<EquipSlots>();
                foreach (EquipSlots s in slots)
                {
                    if(s.armorType == ArmorType.Weapon)
                    {
                        if ((s.gameObject.GetComponentInChildren<InventoryItem>().MyItem as Weapon) != null)
                        {
                            s.gameObject.GetComponentInChildren<InventoryItem>().RemoveItem();
                            s.AddItem(buildUpWeapon);
                            buildUpWeapon.gameObject.SetActive(false);
                            break;
                        }
                    }
                }

                WeaponBuildUpLayout.MyInstance.RemoveWeaponBuildUpSlots();
                WeaponBuildUpLayout.MyInstance.gameObject.SetActive(false);
                buildUpActive = false;
            }
        }
    }

    // COINS
    public void SetCoins(int coins)
    {
        currentCoins.text = coins.ToString();
    }

    // TOOLTIP
    public void ActivateTooltip(Vector3 position, Item item)
    {
        tooltip.SetActive(true);
        tooltip.transform.position = position;
        tooltipItemNameText.text = item.GetItemName();
        tooltipItemTypeText.text = item.GetItemType();
        tooltipItemStats1Text.text = item.GetItemStats1();
        tooltipItemStats1AmountText.text = item.GetItemStats1Amount();
        tooltipItemStats1ComparisonText.text = item.GetItemStats1Comparison();
        tooltipItemStats2Text.text = item.GetItemStats2();
        tooltipItemStats2AmountText.text = item.GetItemStats2Amount();
        tooltipItemStats2ComparisonText.text = item.GetItemStats2Comparison();
        tooltipItemDescription.text = item.GetDiscription();
    }

    public void ActivateTooltip2(Vector3 position, IDescriptable description)
    {
        tooltip2.SetActive(true);
        tooltip2.transform.position = position;
        tooltipText.text = description.GetDiscription();
    }
    public void DeactivateTooltip()
    {
        tooltip.SetActive(false);
    }

    public void DeactivateTooltip2()
    {
        tooltip2.SetActive(false);
    }

    // STATS
    public void UpdatePlayerStats()
    {
        upgradePoints.text = Player.MyInstance.upgratablePoints.ToString();

        playerLevel.text = Player.MyInstance.level.ToString();
        playerMaxXP.text = Player.MyInstance.maxXP.ToString();
        playerCurrentXP.text = Player.MyInstance.XP.ToString();
        playerUpgradePoints.text = Player.MyInstance.upgratablePoints.ToString();

        playerSpirit.text = Player.MyInstance.spirit.ToString();
        playerMind.text = Player.MyInstance.mind.ToString();
        playerResistance.text = Player.MyInstance.resistance.ToString();
        playerSorcery.text = Player.MyInstance.sorcery.ToString();

        playerHealth.text = Player.MyInstance.maxHealth.ToString();
        playerMana.text = Player.MyInstance.maxMana.ToString();

        playerDamage.text = Player.MyInstance.damage.ToString();
        playerPoise.text = Player.MyInstance.maxPoise.ToString();

        playerMagicDamage.text = Player.MyInstance.magicDamage.ToString();
        playerDefence.text = Player.MyInstance.physicalDefence.ToString();
        playerFireDefence.text = Player.MyInstance.fireDefence.ToString();
        playerIceDefence.text = Player.MyInstance.iceDefence.ToString();
        playerLightningDefence.text = Player.MyInstance.lightningDefence.ToString();
        playerPoisonResistance.text = Player.MyInstance.poisonResistance.ToString();
        playerDecayResistance.text = Player.MyInstance.decayResistance.ToString();
        playerPetrifyResistance.text = Player.MyInstance.petrifyResistance.ToString();
        playerFortune.text = Player.MyInstance.fortune.ToString();
        playerLuck.text = Player.MyInstance.luck.ToString();
    }
    public void UpdateWeaponStats()
    {
        if (Player.MyInstance.equipedWeapon != null)
        {
            weaponSprite.sprite = Player.MyInstance.equipedWeapon.MySprite;
            weaponSprite.color = Color.white;
        }
        else
        {
            weaponSprite.sprite = null;
            weaponSprite.color = Color.clear;
        }

        if (weaponSprite.sprite != null)
        {
            wpUpgradePoints.text = Player.MyInstance.equipedWeapon.upgradePoints.ToString();
            // MAIN STATS
            weaponLevel.text = Player.MyInstance.equipedWeapon.level.ToString();
            weaponMaxXP.text = Player.MyInstance.equipedWeapon.maxXP.ToString();
            weaponCurrentXP.text = Player.MyInstance.equipedWeapon.XP.ToString();
            weaponMaxHP.text = Player.MyInstance.equipedWeapon.maxHP.ToString();
            weaponCurrentHP.text = Player.MyInstance.equipedWeapon.HP.ToString();
            weaponUpgradePoints.text = Player.MyInstance.equipedWeapon.upgradePoints.ToString();

            // UPGRADABLE STATS
            weaponAttack.text = Player.MyInstance.equipedWeapon.attack.ToString();
            weaponDurability.text = Player.MyInstance.equipedWeapon.durability.ToString();
            weaponFire.text = Player.MyInstance.equipedWeapon.fire.ToString();
            weaponIce.text = Player.MyInstance.equipedWeapon.ice.ToString();
            weaponLightning.text = Player.MyInstance.equipedWeapon.lightning.ToString();
            weaponBeast.text = Player.MyInstance.equipedWeapon.beast.ToString();
            weaponFlying.text = Player.MyInstance.equipedWeapon.flying.ToString();
            weaponSmash.text = Player.MyInstance.equipedWeapon.smash.ToString();
        }
        else
        {
            wpUpgradePoints.text = "0";
            // MAIN STATS
            weaponLevel.text = "0";
            weaponMaxXP.text = "0";
            weaponCurrentXP.text = "0";
            weaponMaxHP.text = "0";
            weaponCurrentHP.text = "0";
            weaponUpgradePoints.text = "0";

            // UPGRADABLE STATS
            weaponAttack.text = "0";
            weaponDurability.text = "0";
            weaponFire.text = "0";
            weaponIce.text = "0";
            weaponLightning.text = "0";
            weaponBeast.text = "0";
            weaponFlying.text = "0";
            weaponSmash.text = "0";
        }
    }

    // KEY ITEMS OBTAINED
    public void dungeonGateKeyObtained(Item key)
    {
        keyItemSlot.AddItem(key);
        Player.MyInstance.dungeonKeyObtained = true;
    }
    public void dungeonGateKeyUsed()
    {
        keyItemSlot.RemoveItem();
        Player.MyInstance.dungeonKeyObtained = false;
    }

    // GAME OVER
    public void PlayerDeath()
    {
        gameOverText.gameObject.SetActive(true);
    }
}
