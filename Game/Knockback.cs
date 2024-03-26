using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    public float thrust;
    public float knockTime;
    //public int damage;
    public bool isPlayerSpell = false;
    Enemy enemy;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Breakable") && (this.gameObject.CompareTag("Player") || isPlayerSpell))
        {
            other.GetComponent<BreakableObject>().Break(1);
        }
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Player"))
        {
            Rigidbody2D hit = other.GetComponent<Rigidbody2D>();
            if (hit != null)
            {
                if((other.gameObject.CompareTag("Player") && !isPlayerSpell) || other.gameObject.CompareTag("Enemy"))
                {
                    Vector2 difference = hit.transform.position - transform.position;
                    difference = difference.normalized * thrust;
                    hit.AddForce(difference, ForceMode2D.Impulse);
                }

                if (other.gameObject.CompareTag("Enemy") && other.isTrigger)
                {
                    enemy = other.gameObject.GetComponent<Enemy>();
                    if (this.gameObject.CompareTag("Enemy") || (gameObject.transform.parent != null && gameObject.transform.parent.CompareTag("Enemy")))
                    {
                        enemy.FrendlyKnock(hit, knockTime);
                    }
                        
                    else
                    {
                        if(enemy.health > 0)
                        {
                            if(gameObject.CompareTag("Spell"))
                                enemy.Knock(hit, knockTime, CalculatePlayerSpellDamage(), gameObject.GetComponent<Spell>().poiseDamage, true);
                            else
                                enemy.Knock(hit, knockTime, CalculatePlayerDamage(), CalculatePlayerWeaponPoiseDamage(), false);
                            if(Player.MyInstance.currentState == PlayerState.attack3)
                                MainCamera.MyInstance.SmallShake();
                            else if(Player.MyInstance.currentState == PlayerState.heavyStrike)
                                MainCamera.MyInstance.BigShake();
                        }

                    }
                }

                if (other.gameObject.CompareTag("Player") && !isPlayerSpell && other.isTrigger)
                {
                    if(this.gameObject.CompareTag("Enemy") || gameObject.transform.parent.CompareTag("Enemy"))
                    {
                        if (this.gameObject.CompareTag("Enemy"))
                            enemy = this.gameObject.GetComponent<Enemy>();
                        else
                            enemy = this.gameObject.transform.parent.GetComponent<Enemy>();
                        if (enemy.currentState != EnemyStates.death)
                        {
                            enemy.playerHit = true;
                            other.GetComponent<Player>().Knock(knockTime, CalculateEnemyDamage(), enemy.poiseDamage);
                        }
                        else
                        {
                            other.GetComponent<Player>().Knock(0.1f, 0, 0);
                        }
                    }
                    else
                        other.GetComponent<Player>().Knock(knockTime, 5, 10);
                }
            }
        }
    }

    private int CalculatePlayerSpellDamage()
    {
        Player player = Player.MyInstance;
        int magicDamage = player.magicDamage;

        // ENEMY DEFENCE STATS
        int fireDefence = enemy.fireDefence;
        int iceDefence = enemy.iceDefence;
        int lightningDefence = enemy.lightningDefence;

        int enemyDefence = 0;

        switch (player.currentState)
        {
            case PlayerState.fireAttack:
                enemyDefence = fireDefence;
                break;
            case PlayerState.iceAttack:
                enemyDefence = iceDefence;
                break;
            case PlayerState.lightningAttack:
                enemyDefence = lightningDefence;
                break;
            case PlayerState.poisonAttack:
                enemyDefence = enemy.poisonResistance;
                break;
            case PlayerState.decayAttack:
                enemyDefence = enemy.decayResistance;
                break;
            case PlayerState.petrifyAttack:
                enemyDefence = enemy.petrifyResistance;
                break;
        }

        int finalDamage = magicDamage - enemyDefence;

        return finalDamage;
    }

    private int CalculatePlayerWeaponPoiseDamage()
    {
        int poiseDamage = Player.MyInstance.equipedWeapon.poiseDamage;
        if (Player.MyInstance.currentState == PlayerState.attack3)
            poiseDamage += 2;
        if (Player.MyInstance.currentState == PlayerState.heavyStrike)
            poiseDamage += 10;
        return poiseDamage;
    }

    private int CalculatePlayerDamage()
    {
        Player player = Player.MyInstance;
        // PLAYER DAMAGE STATS
        int physical = 0;
        int fire = 0;
        int ice = 0;
        int lightning = 0;
        if (player.equipedWeapon)
        {
            physical = player.damage + player.equipedWeapon.attack;
            fire = player.equipedWeapon.fire;
            ice = player.equipedWeapon.ice;
            lightning = player.equipedWeapon.lightning;
        }
        // ENEMY DEFENCE STATS
        int physicalDefence = enemy.defence;
        int fireDefence = enemy.fireDefence;
        int iceDefence = enemy.iceDefence;
        int lightningDefence = enemy.lightningDefence;
        // ENEMY TYPE
        EnemyType enemyType = enemy.enemyType;
        int enemyTypeDamage = 0;
        switch(enemyType)
        {
            case EnemyType.beast:
                enemyTypeDamage = player.equipedWeapon.beast;
                break;
            case EnemyType.flying:
                enemyTypeDamage = player.equipedWeapon.flying;
                break;
            case EnemyType.smash:
                enemyTypeDamage = player.equipedWeapon.smash;
                break;
            case EnemyType.none:
                enemyTypeDamage = 0;
                break;
        }
        /// DAMAGE CALCULATION \\\
        // Physical
        int physicalDamage = physical - physicalDefence;
        if (physicalDamage <= 0)
            physicalDamage = 1;
        // Fire
        int fireDamage = fire - fireDefence;
        if (fireDamage <= 0)
            fireDamage = 0;
        // Ice
        int iceDamage = ice - iceDefence;
        if (iceDamage <= 0)
            iceDamage = 0;
        // Lightning
        int lightningDamage = lightning - lightningDefence;
        if (lightningDamage <= 0)
            lightningDamage = 0;

        int finalDamage = physicalDamage + fireDamage + iceDamage + lightningDamage + enemyTypeDamage;
        int defence = 0;
        switch (player.currentState)
        {
            case PlayerState.attack2:
                finalDamage += Random.Range(1, 3);
                break;
            case PlayerState.attack3:
                finalDamage += Random.Range(2, 5);
                break;
            case PlayerState.heavyStrike:
                finalDamage += Mathf.FloorToInt(player.magicDamage/2);
                break;
            case PlayerState.fireAttack:
                finalDamage = player.magicDamage;
                defence = fireDefence;
                break;
            case PlayerState.iceAttack:
                finalDamage = player.magicDamage;
                defence = iceDefence;
                break;
            case PlayerState.lightningAttack:
                finalDamage = player.magicDamage;
                defence = lightningDefence;
                break;
            case PlayerState.poisonAttack:
                finalDamage = player.magicDamage;
                defence = enemy.poisonResistance;
                break;
            case PlayerState.decayAttack:
                finalDamage = player.magicDamage;
                defence = enemy.decayResistance;
                break;
            case PlayerState.petrifyAttack:
                finalDamage = player.magicDamage;
                defence = enemy.petrifyResistance;
                break;
        }

        finalDamage -= defence;
        if (finalDamage <= 1)
            finalDamage = 1;

        return finalDamage;
    }

    private int CalculateEnemyDamage()
    {
        Player player = Player.MyInstance;
        // ENEMY DAMAGE STATS
        int physical = enemy.damage;
        int fire = enemy.fire;
        int ice = enemy.ice;
        int lightning = enemy.lightning;
        // PLAYER DEFENCE STATS
        int physicalDefence = player.physicalDefence;
        int fireDefence = player.fireDefence;
        int iceDefence = player.iceDefence;
        int lightningDefence = player.lightningDefence;
        /// DAMAGE CALCULATION \\\
        // Physical
        int physicalDamage = physical - physicalDefence;
        if (physicalDamage <= 0)
            physicalDamage = 1;
        // Fire
        int fireDamage = fire - fireDefence;
        if (fireDamage <= 0)
            fireDamage = 0;
        // Ice
        int iceDamage = ice - iceDefence;
        if (iceDamage <= 0)
            iceDamage = 0;
        // Lightning
        int lightningDamage = lightning - lightningDefence;
        if (lightningDamage <= 0)
            lightningDamage = 0;

        int finalDamage = physicalDamage + fireDamage + iceDamage + lightningDamage;

        return finalDamage;
    }
}
