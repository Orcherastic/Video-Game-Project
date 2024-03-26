using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyStates
{
    idle,
    chase,
    attack,
    attack2,
    attackCombo,
    stagger,
    wait,
    death
}

public enum EnemyType
{
    none,
    beast,
    flying,
    smash
}

public class Enemy : MonoBehaviour
{
    #region VARIABLES

    public EnemyStates currentState;
    public EnemyType enemyType;
    public Canvas canvas;
    protected Rigidbody2D rb;
    protected Animator animator;
    public HealthUI ui;
    // PLAYER
    public Transform target;

    public CoinPouch coinPouch;
    public List<GameObject> pickUpPrefab = new List<GameObject>();
    public GameObject dungeonKeyPrefab;
    public bool playerHit = false;
    protected bool hit = false;
    [Header("Status Effects")]
    public PlayerStatusEffects statusEffects;

    [Header("Passive Stats")]
    private float poiseCounter = 0f;
    public int maxPoise;
    public int currentPoise;
    public int poiseDamage;

    [Header("Stats")]
    public int health;
    public int maxHealth;
    public int damage;
    public int weaponDamage;
    public int defence;
    public int fire;
    public int ice;
    public int lightning;
    public int fireDefence;
    public int iceDefence;
    public int lightningDefence;
    public int poisonResistance;
    public int decayResistance;
    public int petrifyResistance;

    [Header("Behaviour Stats")]
    public float chaseRadius;
    public float attackRadius;
    public float chaseSpeed;
    public float avoidanceDistance = 2f;
    public LayerMask obstacleLayer;

    [Header("Coin Reward")]
    public int minAmount;
    public int maxAmount;

    [Header("XP Reward")]
    public int xpGain;
    public int weaponXpGain;

    bool frendlyFire = false;
    #endregion

    void Start()
    {
        canvas.gameObject.SetActive(false);
        health = maxHealth;
        ui.SetMaxHealth(maxHealth);
        ui.SetHealth(health);
        currentState = EnemyStates.idle;
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player").transform;
        currentPoise = maxPoise;
    }

    #region METHODS
    public /*virtual*/ void takeDamage(int damage, int poiseDamage, bool isPlayerSpell)
    {
        health -= damage;
        currentPoise -= poiseDamage;
        poiseCounter = 0f;
        ui.SetHealth(health);
        if (!isPlayerSpell)
            Player.MyInstance.DamageWeapon(weaponDamage - Player.MyInstance.equipedWeapon.endurance);
        CombatTextManager.MyInstance.CreateTextWorld(transform.position, damage.ToString(), TextType.damage);
        HitEffectController.MyInstance.CreateHitEffect(transform.position);

        if (health <= 0)
        {
            Player.MyInstance.GainXP(xpGain);
            if (!isPlayerSpell)
            {
                // Wooden Stick does not gain XP
                if(Player.MyInstance.equipedWeapon != null && 
                    Player.MyInstance.equipedWeapon.itemName != "Wooden Stick")
                    Player.MyInstance.GainWeaponXP(weaponXpGain);
            }
            Player.MyInstance.counter = 0f;
        }
    }

    public virtual void Knock(Rigidbody2D myRB, float knockTime, int damage, int poiseDamage, bool isPlayerSpell)
    {
        takeDamage(damage, poiseDamage, isPlayerSpell);
        if (health > 0 && currentPoise > 0 && rb.bodyType != RigidbodyType2D.Dynamic)
            StartCoroutine(HitCo());
        else if(health > 0 && currentPoise <= 0)
        {
            currentPoise = maxPoise;
            currentState = EnemyStates.stagger;
            animator.SetTrigger("Staggering");
            StopAllCoroutines();
            StartCoroutine(KnockCo(myRB, knockTime));
        }
        if (health <= 0)
        {
            health = 0;
            StopAllCoroutines();
            StartCoroutine(DeathCo());
        }
    }

    public void FrendlyKnock(Rigidbody2D myRB, float knockTime)
    {
        frendlyFire = true;
        StartCoroutine(KnockCo(myRB, knockTime));
    }

    public virtual void OnPlayerHit()
    {
        // When the Enemy hits the Player
    }

    public virtual void CheckDistance()
    {
        // Check the distance to the Player
    }

    public void UpdatePoise()
    {
        if (currentPoise != maxPoise)
        {
            poiseCounter += Time.deltaTime;
            if (poiseCounter >= 5f)
            {
                currentPoise = maxPoise;
                poiseCounter = 0f;
            }
        }
    }

    #endregion

    #region IENUMERATORS
    private IEnumerator KnockCo(Rigidbody2D myRB, float knockTime)
    {
        if (myRB != null)
        {
            if(myRB.bodyType != RigidbodyType2D.Dynamic && !frendlyFire)
                myRB.bodyType = RigidbodyType2D.Dynamic;
            yield return new WaitForSeconds(knockTime);
            myRB.velocity = Vector2.zero;
            //yield return new WaitForSeconds(0.2f);
            if(currentState != EnemyStates.wait || currentState != EnemyStates.death)
                currentState = EnemyStates.idle;
            myRB.velocity = Vector2.zero;
            frendlyFire = false;
        }
    }

    private IEnumerator HitCo()
    {
        hit = true;
        yield return new WaitForSeconds(0.1f);
        hit = false;
    }

    private IEnumerator DeathCo()
    {
        currentState = EnemyStates.death;
        animator.SetBool("isDead", true);

        // COIN POUCH
        if (coinPouch != null)
        {
            int coinAmount = Random.Range(minAmount, maxAmount + 1);
            int bonus = Mathf.FloorToInt((coinAmount / 100) * Player.MyInstance.fortune);
            coinAmount += bonus;
            coinPouch.amountValue = coinAmount;
            Instantiate(coinPouch, transform.position, Quaternion.identity);
        }


        // 4 in 10 ODDS OF DROPPING AN ITEM IF ANY
        if (pickUpPrefab != null)
        {
            int itemProb = Random.Range(0, 100);
            if (itemProb <= 40 + Player.MyInstance.luck)
            {
                Instantiate(pickUpPrefab[Random.Range(0, pickUpPrefab.Count)], transform.position, Quaternion.identity);
            }
        }

        if(dungeonKeyPrefab != null)
        {
            Instantiate(dungeonKeyPrefab, transform.position, Quaternion.identity);
        }

        yield return new WaitForSeconds(0.4f);
        this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }
    #endregion
}
