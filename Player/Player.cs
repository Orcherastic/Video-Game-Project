using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// PLAYER STATES
public enum PlayerState
{
    cutscene,
    interact,
    run,
    walk,
    attack1,
    attack2,
    attack3,
    heavyStrike,
    fireAttack,
    iceAttack,
    lightningAttack,
    poisonAttack,
    decayAttack,
    petrifyAttack,
    buffAttack,
    stagger,
    death
}
public class Player : MonoBehaviour
{
    #region VARIABLES

    private static Player instance;

    public static Player MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Player>();
            }
            return instance;
        }
    }

    [Header("COMPONENTS")]
    public Animator animator;
    private Rigidbody2D rb;
    public PlayerUI ui;
    public PlayerXP xpBar;
    public PlayerMP mpBar;
    public WeaponUI weaponUI;
    public PlayerInventory inventory;
    public ThinkBubble ThinkBubble;
    public SpriteRenderer recievedItemSprite;

    [Header("EQUIPMENT")]
    public PlayerEquipment[] equipments;
    public Weapon equipedWeapon;

    [Header("Status Effects")]
    public PlayerStatusEffects statusEffects;

    [Header("PREFABS")]
    [SerializeField]
    private Spell[] spellPrefab;

    [Header("STATE MACHINE")]
    public PlayerState currentState;

    [Header("Level Stats")]
    public int level = 0;
    public float maxXP = 30;
    public float XP;
    public int upgratablePoints = 0;
    public int numOfUpgrades = 0;

    [Header("Main Stats")]
    public int spirit = 5;
    public int mind = 5;
    public int resistance = 5;
    public int sorcery = 5;

    private int baseSpirit      = -1;
    private int baseMind        = -1;
    private int baseResistance  = -1;
    private int baseSorcery     = -1;

    [Header("Equipment Stats")]
    public int damage = 0;
    public int maxPoise = 0;
    public int currentPoise = 0;
    private float poiseCounter = 0f;

    [Header("Upgratable Stats")]
    public int maxHealth = 20;          
    public int health;                  
    public int maxMana = 5;             
    public int mana;                    
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

    public int baseMaxHealth           = -1;
    public int baseMaxMana             = -1;
    public int baseMagicDamage         = -1;
    public int basePhysicalDefence     = -1;
    public int baseFireDefence         = -1;
    public int baseIceDefence          = -1;
    public int baseLightningDefence    = -1;
    public int basePoisonResistance    = -1;
    public int baseDecayResistance     = -1;
    public int basePetrifyResistance   = -1;
    public int baseFortune             = -1;
    public int baseLuck                = -1;

    [Header("Coins")]
    public int coins = 0;

    [Header("MOVEMENT")]
    public float movementSpeed;
    public float playerSpeed = 3f;
    private Vector3 movement;

    [Header("ATTACK")]
    private bool canCombo = true;
    private bool shouldCombo1 = false;
    private bool shouldCombo2 = false;

    [Header("XP BAR FADE")]
    public float timeToFade;
    public float counter = 0f;

    [Header("INVENTORY")]
    public bool openInventory = false;
    public bool shopping = false;
    public bool interacting = false;
    public bool inTriggerRange = false;
    public bool upgrading = false;
    public bool weaponUpgrading = false;
    public bool weaponRepairing = false;
    public bool sceneTransitioning = false;
    public bool dungeonKeyObtained = false;

    [Header("USE ITEMS")]
    public Image itemBeingUsed;
    public bool itemUsed = false;

    [Header("Weapon")]
    float damageLessAttack = 0f;
    int numOfWeapon = 0;

    [Header("Cutscenes")]
    public Vector2 toGoPosition = Vector2.zero;

    #endregion

    #region START/UPDATE
    void Start()
    {
        InitializeComponents();

        LoadSpawnPosition();
        LoadSprite();
        LoadStats();
        ui.SetLevel(level);

        currentState = PlayerState.run;
        currentPoise = maxPoise;

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        inventory.GetComponent<Animator>().SetTrigger("Deactivate");

        LoadInventory();
        LoadQuickUseSlots();
        LoadEquipment();
        LoadSpells();
        LoadUseSpellSlots();
        LoadKeyItem();

        xpBar.gameObject.SetActive(false);

        ui.SetMaxXP(maxXP);
        ui.SetXP(XP);
        ui.SetMaxHealth(maxHealth);
        ui.SetHealth(health);
        ui.SetMaxMana(maxMana);
        ui.SetMana(mana);

        ui.SetCoins(coins);

        SetPlayerStats();
    }

    private void InitializeComponents()
    {
        ui = FindObjectOfType<PlayerUI>();
        xpBar = FindObjectOfType<PlayerXP>();
        mpBar = FindObjectOfType<PlayerMP>();
        weaponUI = FindObjectOfType<WeaponUI>();
        inventory = FindObjectOfType<PlayerInventory>();
        ThinkBubble = FindObjectOfType<ThinkBubble>();
        ThinkBubble.gameObject.SetActive(false);
        statusEffects = FindObjectOfType<PlayerStatusEffects>();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentState != PlayerState.cutscene)
        {
            // INPUT
            movement = Vector3.zero;
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            // Player cannot move if interacting
            if (currentState == PlayerState.interact)
            {
                animator.SetBool("isRunning", false);
                animator.SetBool("isWalking", false);
                foreach (PlayerEquipment pe in equipments)
                {
                    pe.SetXAndY(animator.GetFloat("Horizontal"), animator.GetFloat("Vertical"));
                    pe.SetRunWalkBools("isRunning", animator.GetBool("isRunning"),
                                       "isWalking", animator.GetBool("isWalking"));
                }
            }

            //INVENTORY
            if (Input.GetButtonDown("Interact") && !interacting && !inTriggerRange && !upgrading && !weaponUpgrading
                && !sceneTransitioning)
            {
                openInventory = !openInventory;
                if (openInventory == true)
                {
                    currentState = PlayerState.interact;
                    inventory.GetComponent<Animator>().SetTrigger("Activate");
                }
                else
                {
                    currentState = PlayerState.run;
                    inventory.GetComponent<Animator>().SetTrigger("Deactivate");
                }
            }

            // Player cannot attack if no Weapon is Equiped
            equipedWeapon = FindEquipedWeapon();
            if (equipedWeapon)
            {
                animator.SetBool("Battle", true);
                damage = equipedWeapon.attack;
                foreach (PlayerEquipment pe in equipments)
                {
                    pe.SetAnimatorBool("Battle", true);
                }
                weaponUI.gameObject.SetActive(true);
                ui.SetMaxWeaponXP(equipedWeapon.maxXP);
                ui.SetWeaponXP(equipedWeapon.XP);
                ui.SetMaxWeaponHealth(equipedWeapon.maxHP);
                ui.SetWeaponHealth(equipedWeapon.HP);
                ui.weaponUISprite.sprite = equipedWeapon.MySprite;

                if (equipedWeapon.upgradePoints <= 0)
                    ui.wpUpgradePoints.gameObject.SetActive(false);
                else
                    ui.wpUpgradePoints.gameObject.SetActive(true);

                // ATTACK1
                if (Input.GetMouseButtonDown(0) && (currentState == PlayerState.run || currentState == PlayerState.walk)
                    && !equipedWeapon.weaponBroken)
                {
                    canCombo = false;
                    StartCoroutine(AttackCo());
                }
                // ATTACK2
                if (Input.GetMouseButtonDown(0) && currentState == PlayerState.attack1 && canCombo && !equipedWeapon.weaponBroken)
                {
                    canCombo = false;
                    shouldCombo1 = true;
                    StartCoroutine(AttackCo2());
                }
                // ATTACK3
                if (Input.GetMouseButtonDown(0) && currentState == PlayerState.attack2 && canCombo && !equipedWeapon.weaponBroken)
                {
                    canCombo = false;
                    shouldCombo1 = false;
                    shouldCombo2 = true;
                    StartCoroutine(AttackCo3());
                }
            }
            else
            {
                animator.SetBool("Battle", false);
                damage = 0;
                foreach (PlayerEquipment pe in equipments)
                {
                    pe.SetAnimatorBool("Battle", false);
                }
                weaponUI.gameObject.SetActive(false);
            }

            // RUN / WALK
            if (currentState == PlayerState.run || currentState == PlayerState.walk)
            {
                shouldCombo1 = false;
                shouldCombo2 = false;
                UpdatePlayerMovement();
            }

            // FADE XP BAR
            if (xpBar.isActiveAndEnabled)
            {
                ToFadeXpBar();
            }

            if (upgratablePoints <= 0)
                ui.upgradePoints.gameObject.SetActive(false);
            else
                ui.upgradePoints.gameObject.SetActive(true);

            ui.UpdatePlayerStats();
            ui.UpdateWeaponStats();

            // Update Poise
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
        else
        {
            if (toGoPosition != Vector2.zero)
            {
                if(animator.GetBool("isRunning") == true)
                    MoveToPosition(toGoPosition, 3f);
                else if(animator.GetBool("isWalking") == true)
                    MoveToPosition(toGoPosition, 1.5f);
            }
        }
    }
    #endregion

    #region METHODS

    void UpdatePlayerMovement()
    {
        if(movement != Vector3.zero)
        {
            // PLAYER STAYS IN THE CORESPONDING IDLE ANIMATION
            if (Mathf.Abs(movement.x) == Mathf.Abs(movement.y))
            {
                animator.SetFloat("Horizontal", movement.x);
                animator.SetFloat("Vertical", 0);
            }
            // MOVING
            else
            {
                animator.SetFloat("Horizontal", movement.x);
                animator.SetFloat("Vertical", movement.y);
            }
            // WALKING
            if (Input.GetKey(KeyCode.LeftShift))
            {
                currentState = PlayerState.walk;
                animator.SetBool("isWalking", true);
                animator.SetBool("isRunning", false);
            }
            // RUNNING
            else
            {
                currentState = PlayerState.run;
                animator.SetBool("isWalking", false);
                animator.SetBool("isRunning", true);
            }
            
            foreach(StatusEffect se in statusEffects.ActiveStatusEffects)
            {
                if(se.statusEffectName == "Shock")
                {
                    animator.SetBool("isWalking", false);
                    animator.SetBool("isRunning", false);
                }
            }

            MovePlayer();
        }
        else
        {
            // IDLE
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalking", false);
        }

        foreach (PlayerEquipment pe in equipments)
        {
            pe.SetXAndY(animator.GetFloat("Horizontal"), animator.GetFloat("Vertical"));
            pe.SetRunWalkBools("isRunning", animator.GetBool("isRunning"),
                               "isWalking", animator.GetBool("isWalking"));
        }
    }

    void MovePlayer()
    {
        // SPEED VALUE CHANGE
        if (currentState == PlayerState.walk)
            movementSpeed = playerSpeed / 2;
        else
            movementSpeed = playerSpeed;
        rb.MovePosition(transform.position + movement.normalized * movementSpeed * Time.fixedDeltaTime);
    }

    void MovePlayerWhenAttacking()
    {
        Vector3 move = Vector3.zero;
        move.x = animator.GetFloat("Horizontal");
        move.y = animator.GetFloat("Vertical");
        transform.position += move / 5;
    }

    public void MoveToPosition(Vector2 position, float speed)
    {
        Vector3 temp = Vector3.MoveTowards(transform.position, position, speed * Time.deltaTime);
        rb.MovePosition(temp);
    }

    public void SaveStats()
    {
        PlayerStatsManager statsManager = PlayerStatsManager.MyInstance;
        statsManager.playerStats.level = level;
        statsManager.playerStats.maxXP = maxXP;
        statsManager.playerStats.XP = XP;
        statsManager.playerStats.upgratablePoints = upgratablePoints;

        statsManager.playerStats.spirit = spirit;
        statsManager.playerStats.mind = mind;
        statsManager.playerStats.resistance = resistance;
        statsManager.playerStats.sorcery = sorcery;

        statsManager.playerStats.damage = damage;
        statsManager.playerStats.maxPoise = maxPoise;

        statsManager.playerStats.maxHealth = maxHealth;
        statsManager.playerStats.health = health;
        statsManager.playerStats.maxMana = maxMana;
        statsManager.playerStats.mana = mana;
        statsManager.playerStats.magicDamage = magicDamage;
        statsManager.playerStats.physicalDefence = physicalDefence;
        statsManager.playerStats.fireDefence = fireDefence;
        statsManager.playerStats.iceDefence = iceDefence;
        statsManager.playerStats.lightningDefence = lightningDefence;
        statsManager.playerStats.poisonResistance = poisonResistance;
        statsManager.playerStats.decayResistance = decayResistance;
        statsManager.playerStats.petrifyResistance = petrifyResistance;
        statsManager.playerStats.fortune = fortune;
        statsManager.playerStats.luck = luck;

        statsManager.playerStats.coins = coins;
    }

    public void LoadStats()
    {
        PlayerStatsManager statsManager = PlayerStatsManager.MyInstance;
        level              = statsManager.playerStats.level;
        maxXP              = statsManager.playerStats.maxXP;
        XP                 = statsManager.playerStats.XP;
        upgratablePoints   = statsManager.playerStats.upgratablePoints;

        spirit             = statsManager.playerStats.spirit;
        mind               = statsManager.playerStats.mind;
        resistance         = statsManager.playerStats.resistance;
        sorcery            = statsManager.playerStats.sorcery;

        damage             = statsManager.playerStats.damage;
        maxPoise           = statsManager.playerStats.maxPoise;

        maxHealth          = statsManager.playerStats.maxHealth;
        health             = statsManager.playerStats.health;
        maxMana            = statsManager.playerStats.maxMana;
        mana               = statsManager.playerStats.mana;
        magicDamage        = statsManager.playerStats.magicDamage;
        physicalDefence    = statsManager.playerStats.physicalDefence;
        fireDefence        = statsManager.playerStats.fireDefence;
        iceDefence         = statsManager.playerStats.iceDefence;
        lightningDefence   = statsManager.playerStats.lightningDefence;
        poisonResistance   = statsManager.playerStats.poisonResistance;
        decayResistance    = statsManager.playerStats.decayResistance;
        petrifyResistance  = statsManager.playerStats.petrifyResistance;
        fortune            = statsManager.playerStats.fortune;
        luck               = statsManager.playerStats.luck;

        coins              = statsManager.playerStats.coins;
    }

    public void SaveSpawnPosition(Vector2 position)
    {
        PlayerSpawnManager.MyInstance.playerSpawn.spawnPoint = position;
    }

    public void LoadSpawnPosition()
    {
        transform.position = PlayerSpawnManager.MyInstance.playerSpawn.spawnPoint;
    }

    public void SaveSprite()
    {
        PlayerSpawnSpriteManager.MyInstance.playerSpriteInfo.playerHorizontal = animator.GetFloat("Horizontal");
        PlayerSpawnSpriteManager.MyInstance.playerSpriteInfo.playerVertical = animator.GetFloat("Vertical");
    }

    public void LoadSprite()
    {
        animator.SetFloat("Horizontal", PlayerSpawnSpriteManager.MyInstance.playerSpriteInfo.playerHorizontal);
        animator.SetFloat("Vertical", PlayerSpawnSpriteManager.MyInstance.playerSpriteInfo.playerVertical);
    }

    public void SaveInventory()
    {
        PlayerInventoryManager.MyInstance.playerInventoryItems.items.Clear();
        WeaponStatsManager.MyInstance.weaponStats.Clear();
        foreach (ItemSlot slot in InventoryItemSlots.MyInstance.usableSlots)
        {
            if (slot.gameObject.GetComponentInChildren<InventoryItem>().MyItem is Weapon)
            {
                (slot.gameObject.GetComponentInChildren<InventoryItem>().MyItem as Weapon).SaveStats();
            }
            PlayerInventoryManager.MyInstance.playerInventoryItems.AddItem(slot.gameObject.GetComponentInChildren<InventoryItem>().MyItem,
                                                                        slot.gameObject.GetComponentInChildren<InventoryItem>().numOfItems);
        }
    }

    public void LoadInventory()
    {
        for(int i=0; i< PlayerInventoryManager.MyInstance.playerInventoryItems.items.Count; i++)
        {
            if (PlayerInventoryManager.MyInstance.playerInventoryItems.items[i].item == null)
            {
                InventoryItemSlots.MyInstance.usableSlots[i].AddItem(PlayerInventoryManager.MyInstance.playerInventoryItems.items[i].item);
            }
            else
            {
                Item worldItem = Instantiate(PlayerInventoryManager.MyInstance.playerInventoryItems.items[i].item, transform.position, Quaternion.identity);
                
                if (worldItem is Weapon)
                {
                    (worldItem as Weapon).LoadStats(numOfWeapon);
                    numOfWeapon++;
                }
                
                InventoryItemSlots.MyInstance.usableSlots[i].AddItem(worldItem);
                InventoryItemSlots.MyInstance.usableSlots[i].gameObject.GetComponentInChildren<InventoryItem>().numOfItems = PlayerInventoryManager.MyInstance.playerInventoryItems.items[i].numberOf;
                InventoryItemSlots.MyInstance.usableSlots[i].gameObject.GetComponentInChildren<InventoryItem>().numOfItemsTxt.text = InventoryItemSlots.MyInstance.usableSlots[i].gameObject.GetComponentInChildren<InventoryItem>().numOfItems.ToString();
                worldItem.gameObject.SetActive(false);
            }
        }
    }

    public void SaveQuickUseSlots()
    {
        PlayerInventoryManager.MyInstance.playerInventoryItems.quickUseItems.Clear();
        foreach (QuickUseSlot slot in InventoryQuickUseSlots.MyInstance.slots)
        {
            PlayerInventoryManager.MyInstance.playerInventoryItems.AddQuickuseItem(slot.gameObject.GetComponentInChildren<InventoryItem>().MyItem,
                                                                        slot.gameObject.GetComponentInChildren<InventoryItem>().numOfItems);
        }
    }

    public void LoadQuickUseSlots()
    {
        for (int i = 0; i < PlayerInventoryManager.MyInstance.playerInventoryItems.quickUseItems.Count; i++)
        {
            if (PlayerInventoryManager.MyInstance.playerInventoryItems.quickUseItems[i].item == null)
                InventoryQuickUseSlots.MyInstance.slots[i].AddItem(PlayerInventoryManager.MyInstance.playerInventoryItems.quickUseItems[i].item);
            else
            {
                Item worldItem = Instantiate(PlayerInventoryManager.MyInstance.playerInventoryItems.quickUseItems[i].item, transform.position, Quaternion.identity);
                InventoryQuickUseSlots.MyInstance.slots[i].AddItem(worldItem);
                InventoryQuickUseSlots.MyInstance.slots[i].gameObject.GetComponentInChildren<InventoryItem>().numOfItems = PlayerInventoryManager.MyInstance.playerInventoryItems.quickUseItems[i].numberOf;
                InventoryQuickUseSlots.MyInstance.slots[i].gameObject.GetComponentInChildren<InventoryItem>().numOfItemsTxt.text = InventoryQuickUseSlots.MyInstance.slots[i].gameObject.GetComponentInChildren<InventoryItem>().numOfItems.ToString();
                worldItem.gameObject.SetActive(false);
            }
        }
    }

    public void SaveEquipment()
    {
        PlayerInventoryManager.MyInstance.playerInventoryItems.AddArmor(FindEquipmentOfType(ArmorType.Helmet), ArmorType.Helmet);
        PlayerInventoryManager.MyInstance.playerInventoryItems.AddArmor(FindEquipmentOfType(ArmorType.Chestplate), ArmorType.Chestplate);
        PlayerInventoryManager.MyInstance.playerInventoryItems.AddArmor(FindEquipmentOfType(ArmorType.Leggings), ArmorType.Leggings);
        PlayerInventoryManager.MyInstance.playerInventoryItems.AddArmor(FindEquipmentOfType(ArmorType.Boots), ArmorType.Boots);
        PlayerInventoryManager.MyInstance.playerInventoryItems.AddArmor(FindEquipmentOfType(ArmorType.Weapon), ArmorType.Weapon);
        if (FindEquipmentOfType(ArmorType.Weapon) != null)
        {
            (FindEquipmentOfType(ArmorType.Weapon) as Weapon).SaveStats();
        }
            

        PlayerInventoryManager.MyInstance.playerInventoryItems.AddAccessory(ChooseAccessorySlot(0), 0);
        PlayerInventoryManager.MyInstance.playerInventoryItems.AddAccessory(ChooseAccessorySlot(1), 1);
    }

    public void LoadEquipment()
    {
        EquipSlots[] slots = FindObjectsOfType<EquipSlots>();
        List<EquipSlots> accessorySlots = new List<EquipSlots>();
        Armor worldArmor, worldArmor1;
        foreach (EquipSlots slot in slots)
        {
            switch(slot.armorType)
            {
                case ArmorType.Helmet:
                    if(PlayerInventoryManager.MyInstance.playerInventoryItems.helmet != null)
                    {
                        worldArmor = Instantiate(PlayerInventoryManager.MyInstance.playerInventoryItems.helmet, transform.position, Quaternion.identity);
                        slot.AddItem(worldArmor);
                        worldArmor.gameObject.SetActive(false);
                    }
                    else
                        slot.AddItem(null);
                    break;
                case ArmorType.Chestplate:
                    if (PlayerInventoryManager.MyInstance.playerInventoryItems.chestplate != null)
                    {
                        worldArmor = Instantiate(PlayerInventoryManager.MyInstance.playerInventoryItems.chestplate, transform.position, Quaternion.identity);
                        slot.AddItem(worldArmor);
                        worldArmor.gameObject.SetActive(false);
                    }
                    else
                        slot.AddItem(null);
                    break;
                case ArmorType.Leggings:
                    if (PlayerInventoryManager.MyInstance.playerInventoryItems.leggings != null)
                    {
                        worldArmor = Instantiate(PlayerInventoryManager.MyInstance.playerInventoryItems.leggings, transform.position, Quaternion.identity);
                        slot.AddItem(worldArmor);
                        worldArmor.gameObject.SetActive(false);
                    }
                    else
                        slot.AddItem(null);
                    break;
                case ArmorType.Boots:
                    if (PlayerInventoryManager.MyInstance.playerInventoryItems.boots != null)
                    {
                        worldArmor = Instantiate(PlayerInventoryManager.MyInstance.playerInventoryItems.boots, transform.position, Quaternion.identity);
                        slot.AddItem(worldArmor);
                        worldArmor.gameObject.SetActive(false);
                    }
                    else
                        slot.AddItem(null);
                    break;
                case ArmorType.Weapon:
                    if (PlayerInventoryManager.MyInstance.playerInventoryItems.weapon != null)
                    {
                        worldArmor = Instantiate(PlayerInventoryManager.MyInstance.playerInventoryItems.weapon, transform.position, Quaternion.identity);
                        (worldArmor as Weapon).LoadStats(numOfWeapon);
                        numOfWeapon++;
                        slot.AddItem(worldArmor);
                        worldArmor.gameObject.SetActive(false);
                    }
                    else
                        slot.AddItem(null);
                    break;
                case ArmorType.Accessory:
                    accessorySlots.Add(slot);
                    break;
            }
        }
        if(accessorySlots.Count > 0)
        {
            if (PlayerInventoryManager.MyInstance.playerInventoryItems.accessory1 != null)
            {
                worldArmor = Instantiate(PlayerInventoryManager.MyInstance.playerInventoryItems.accessory1, transform.position, Quaternion.identity);
                accessorySlots[0].AddItem(worldArmor);
                worldArmor.gameObject.SetActive(false);
            }
            else
                accessorySlots[0].AddItem(null);
            if (PlayerInventoryManager.MyInstance.playerInventoryItems.accessory2 != null)
            {
                worldArmor1 = Instantiate(PlayerInventoryManager.MyInstance.playerInventoryItems.accessory2, transform.position, Quaternion.identity);
                accessorySlots[1].AddItem(worldArmor1);
                worldArmor1.gameObject.SetActive(false);
            }
            else
                accessorySlots[1].AddItem(null);
        }
    }

    public void LoadSpells()
    {
        foreach(Item spellBook in PlayerSpellsManager.MyInstance.playerSpells.spellBooks)
        {
            foreach (MagicSlot slot in InventoryMagicSlots.MyInstance.slots)
            {
                if (spellBook.itemName == slot.myMagicName)
                {
                    slot.unlocked = true;
                    break;
                }
            }
        }
    }

    public void SaveUseSpellSlots()
    {
        PlayerSpellsManager.MyInstance.playerSpells.spellUseSprites.Clear();
        PlayerSpellsManager.MyInstance.playerSpells.spellNames.Clear();
        for (int i=0; i<InventoryMagicUseSlots.MyInstance.slots.Count; i++)
        {
            PlayerSpellsManager.MyInstance.playerSpells.spellUseSprites.Add(InventoryMagicUseSlots.MyInstance.slots[i].myMagicSpriteSmall.sprite);
            PlayerSpellsManager.MyInstance.playerSpells.spellNames.Add(InventoryMagicUseSlots.MyInstance.slots[i].mySpellName);
        }
    }

    public void LoadUseSpellSlots()
    {
        for (int i = 0; i < PlayerSpellsManager.MyInstance.playerSpells.spellNames.Count; i++)
        {
            InventoryMagicUseSlots.MyInstance.AddMagicToSpecificSlot(PlayerSpellsManager.MyInstance.playerSpells.spellUseSprites[i], 
                                                        PlayerSpellsManager.MyInstance.playerSpells.spellNames[i], i);
            foreach(MagicSlot slot in InventoryMagicSlots.MyInstance.slots)
            {
                if(PlayerSpellsManager.MyInstance.playerSpells.spellNames[i] != string.Empty)
                {
                    if (slot.myMagicName == PlayerSpellsManager.MyInstance.playerSpells.spellNames[i])
                    {
                        slot.selected = true;
                        break;
                    }
                }
            }
        }
    }

    public void SaveKeyItem()
    {
        KeyItemSlot slot = FindObjectOfType<KeyItemSlot>();
        PlayerInventoryManager.MyInstance.playerInventoryItems.AddKeyItem(slot.MyItem);
    }

    public void LoadKeyItem()
    {
        if (PlayerInventoryManager.MyInstance.playerInventoryItems.keyItem != null)
            PlayerUI.MyInstance.dungeonGateKeyObtained(PlayerInventoryManager.MyInstance.playerInventoryItems.keyItem);
    }

    public void RecieveItem(Item recievedItem)
    {
        if(currentState == PlayerState.interact)
        {
            animator.SetBool("RecieveItem", true);
            foreach(PlayerEquipment pe in equipments)
            {
                pe.SetAnimatorBool("RecieveItem", true);
            }
            recievedItemSprite.sprite = recievedItem.MySprite;
            recievedItemSprite.GetComponent<SpriteRenderer>().sortingOrder = 1000;
        }
        else
        {
            animator.SetBool("RecieveItem", false);
            foreach (PlayerEquipment pe in equipments)
            {
                pe.SetAnimatorBool("RecieveItem", false);
            }
            recievedItemSprite.sprite = null;
        }
    }

    public void UseItem(string usageType, Item item)
    {
        currentState = PlayerState.interact;
        itemBeingUsed.sprite = item.MySprite;
        switch(usageType)
        {
            case "eating":
                StartCoroutine(EatCo(item));
                break;
            case "drinking":
                StartCoroutine(DrinkCo(item));
                break;
            case "repairWeapon":
                StartCoroutine(RepairWeaponCo(item));
                break;
        }
    }

    public void Knock(float knockTime, int damage, int poiseDamage)
    {
        health -= damage;
        if(poiseDamage > 0)
        {
            currentPoise -= poiseDamage;
            poiseCounter = 0f;
            if (currentPoise > 0)
                rb.velocity = Vector2.zero;
        }

        CombatTextManager.MyInstance.CreateTextWorld(transform.position, damage.ToString(), TextType.damage);
        HitEffectController.MyInstance.CreateHitEffect(transform.position);
        if (health > 0 && currentPoise <= 0 && poiseDamage >= 0)
        {
            currentPoise = maxPoise;
            StopAllCoroutines();
            StartCoroutine(KnockCo(knockTime));
        }
        else if (health <= 0)
        {
            health = 0;
            StopAllCoroutines();
            StartCoroutine(DeathCo());
        }
        ui.SetHealth(health);
    }

    public void DamageWeapon(int damage)
    {
        if(damage <= 0)
        {
            damageLessAttack += 1f / (-(damage) + 2f);
            if (damageLessAttack >= 1)
            {
                equipedWeapon.HP -= 1;
                damageLessAttack = 0f;
            }
        }
        else
        {
            equipedWeapon.HP -= damage;
        }
        if(equipedWeapon.HP <= 0)
        {
            equipedWeapon.HP = 0;
        }
        ui.SetWeaponHealth(equipedWeapon.HP);
    }

    public void HealHealth(int hp)
    {
        health += hp;
        CombatTextManager.MyInstance.CreateTextWorld(transform.position, hp.ToString(), TextType.healHP);
        if (health >= maxHealth)
            health = maxHealth;
        ui.SetHealth(health);
    }

    public void RepairWeapon()
    {
        equipedWeapon.HP = equipedWeapon.maxHP;
        ui.SetWeaponHealth(equipedWeapon.HP);
    }

    public void GainMana(int mp)
    {
        mana += mp;
        CombatTextManager.MyInstance.CreateTextWorld(transform.position, mp.ToString(), TextType.healMP);
        if (mana >= maxMana)
            mana = maxMana;
        ui.SetMana(mana);
    }

    public void GainXP(float xp)
    {
        xpBar.gameObject.SetActive(true);
        XP += xp;
        CombatTextManager.MyInstance.CreateText(XPTextPopUpHolder.MyInstance.transform.position, xp.ToString(), TextType.gainXP);
        if (XP >= maxXP)
        {
            LevelUp();
        }
        else
        {
            ui.SetXP(XP);
        }
    }

    public void LevelUp()
    {
        CombatTextManager.MyInstance.CreateText(PlayerLevelUpTextPopUpHolder.MyInstance.transform.position, "LevelUp", TextType.levelUp);
        level++;
        ui.SetLevel(level);
        upgratablePoints += 3;
        float offset = XP - maxXP;
        maxXP = Mathf.Floor(100 * level * Mathf.Pow(level, 0.5f));
        XP = offset;
        ui.SetMaxXP(maxXP);
        ui.SetXP(offset);
    }

    public void UpgradeStat(string stat)
    {
        if(upgratablePoints > 0)
        {
            upgrading = true;
            upgratablePoints--;
            numOfUpgrades++;
            switch(stat)
            {
                case "spirit":
                    #region SPIRIT
/*                    if (baseSpirit == -1)
                        baseSpirit = spirit;
                    if (baseMaxHealth == -1)
                        baseMaxHealth = maxHealth;
                    if (basePhysicalDefence == -1)
                        basePhysicalDefence = physicalDefence;
                    if (basePoisonResistance == -1)
                        basePoisonResistance = poisonResistance;
                    if (baseFortune == -1)
                        baseFortune = fortune;*/

                    spirit++;
                    maxHealth += 5;
                    health += 5;
                    PlayerUI.MyInstance.SetMaxHealth(maxHealth);
                    PlayerUI.MyInstance.SetHealth(health);
                    physicalDefence += 1;
                    poisonResistance += 1;
                    fortune += 1;

                    PlayerUI.MyInstance.playerSpirit.color = Color.cyan;
                    PlayerUI.MyInstance.playerHealth.color = Color.cyan;
                    PlayerUI.MyInstance.playerDefence.color = Color.cyan;
                    PlayerUI.MyInstance.playerPoisonResistance.color = Color.cyan;
                    PlayerUI.MyInstance.playerFortune.color = Color.cyan;
                    #endregion
                    break;

                case "mind":
                    #region Mind
/*                    if (baseMind == -1)
                        baseMind = mind;
                    if (baseMaxMana == -1)
                        baseMaxMana = maxMana;
                    if (baseFireDefence == -1)
                        baseFireDefence = fireDefence;
                    if (baseIceDefence == -1)
                        baseIceDefence = iceDefence;
                    if (baseLightningDefence == -1)
                        baseLightningDefence = lightningDefence;
                    if (baseDecayResistance == -1)
                        baseDecayResistance = decayResistance;
                    if (baseLuck == -1)
                        baseLuck = luck;*/

                    mind++;
                    maxMana += 2;
                    mana += 2;
                    PlayerUI.MyInstance.SetMaxMana(maxMana);
                    PlayerUI.MyInstance.SetMana(mana);
                    fireDefence += 1;
                    iceDefence += 1;
                    lightningDefence += 1;
                    decayResistance += 1;
                    luck += 1;

                    PlayerUI.MyInstance.playerMind.color = Color.cyan;
                    PlayerUI.MyInstance.playerMana.color = Color.cyan;
                    PlayerUI.MyInstance.playerFireDefence.color = Color.cyan;
                    PlayerUI.MyInstance.playerIceDefence.color = Color.cyan;
                    PlayerUI.MyInstance.playerLightningDefence.color = Color.cyan;
                    PlayerUI.MyInstance.playerDecayResistance.color = Color.cyan;
                    PlayerUI.MyInstance.playerLuck.color = Color.cyan;
                    #endregion
                    break;

                case "resistance":
                    #region Resistance
/*                    if (baseResistance == -1)
                        baseResistance = resistance;
                    if (basePhysicalDefence == -1)
                        basePhysicalDefence = physicalDefence;
                    if (baseFireDefence == -1)
                        baseFireDefence = fireDefence;
                    if (baseIceDefence == -1)
                        baseIceDefence = iceDefence;
                    if (baseLightningDefence == -1)
                        baseLightningDefence = lightningDefence;
                    if (basePetrifyResistance == -1)
                        basePetrifyResistance = petrifyResistance;*/

                    resistance++;
                    physicalDefence += 1;
                    fireDefence += 1;
                    iceDefence += 1;
                    lightningDefence += 1;
                    petrifyResistance += 1;

                    PlayerUI.MyInstance.playerResistance.color = Color.cyan;
                    PlayerUI.MyInstance.playerDefence.color = Color.cyan;
                    PlayerUI.MyInstance.playerFireDefence.color = Color.cyan;
                    PlayerUI.MyInstance.playerIceDefence.color = Color.cyan;
                    PlayerUI.MyInstance.playerLightningDefence.color = Color.cyan;
                    PlayerUI.MyInstance.playerPetrifyResistance.color = Color.cyan;
                    #endregion
                    break;

                case "sorcery":
                    #region Sorcery
/*                    if (baseSorcery == -1)
                        baseSorcery = sorcery;
                    if (baseMagicDamage == -1)
                        baseMagicDamage = magicDamage;
                    if (baseFireDefence == -1)
                        baseFireDefence = fireDefence;
                    if (baseIceDefence == -1)
                        baseIceDefence = iceDefence;
                    if (baseLightningDefence == -1)
                        baseLightningDefence = lightningDefence;*/

                    sorcery++;
                    magicDamage += 1;
                    fireDefence += 1;
                    iceDefence += 1;
                    lightningDefence += 1;

                    PlayerUI.MyInstance.playerSorcery.color = Color.cyan;
                    PlayerUI.MyInstance.playerMagicDamage.color = Color.cyan;
                    PlayerUI.MyInstance.playerFireDefence.color = Color.cyan;
                    PlayerUI.MyInstance.playerIceDefence.color = Color.cyan;
                    PlayerUI.MyInstance.playerLightningDefence.color = Color.cyan;
                    #endregion
                    break;
            }
        }
    }

    public void DowngradeStat(string stat)
    {
        if (numOfUpgrades > 0)
        {
            switch (stat)
            {
                case "spirit":
                    #region Spirit
                    int tempSpirit = 0;
                    if(spirit > baseSpirit)
                    {
                        tempSpirit = spirit;
                        upgratablePoints++;
                        numOfUpgrades--;
                        spirit--;
                    }
                    if(spirit == baseSpirit)
                    {
                        PlayerUI.MyInstance.playerSpirit.color = Color.black;
                    }

                    if(tempSpirit > spirit)
                    {
                        if (maxHealth > baseMaxHealth)
                        {
                            maxHealth -= 5;
                            health -= 5;
                            PlayerUI.MyInstance.SetMaxHealth(maxHealth);
                            PlayerUI.MyInstance.SetHealth(health);
                        }
                        if (maxHealth == baseMaxHealth)
                        {
                            PlayerUI.MyInstance.playerHealth.color = Color.black;
                        }

                        if (physicalDefence > basePhysicalDefence)
                        {
                            physicalDefence -= 1;
                        }
                        if (physicalDefence == basePhysicalDefence)
                        {
                            PlayerUI.MyInstance.playerDefence.color = Color.black;
                        }

                        if (poisonResistance > basePoisonResistance)
                        {
                            poisonResistance -= 1;
                        }
                        if (poisonResistance == basePoisonResistance)
                        {
                            PlayerUI.MyInstance.playerPoisonResistance.color = Color.black;
                        }

                        if (fortune > baseFortune)
                        {
                            fortune -= 1;
                        }
                        if (fortune == baseFortune)
                        {
                            PlayerUI.MyInstance.playerFortune.color = Color.black;
                        }
                    }
                    #endregion
                    break;

                case "mind":
                    #region Mind
                    int tempMind = 0;
                    if (mind > baseMind)
                    {
                        tempMind = mind;
                        upgratablePoints++;
                        numOfUpgrades--;
                        mind--;
                    }
                    if (mind == baseMind)
                    {
                        PlayerUI.MyInstance.playerMind.color = Color.black;
                    }

                    if(tempMind > mind)
                    {
                        if (maxMana > baseMaxMana)
                        {
                            maxMana -= 2;
                            mana -= 2;
                            PlayerUI.MyInstance.SetMaxMana(maxMana);
                            PlayerUI.MyInstance.SetMana(mana);
                        }
                        if (maxMana == baseMaxMana)
                        {
                            PlayerUI.MyInstance.playerMana.color = Color.black;
                        }

                        if (fireDefence > baseFireDefence)
                        {
                            fireDefence -= 1;
                        }
                        if (fireDefence == baseFireDefence)
                        {
                            PlayerUI.MyInstance.playerFireDefence.color = Color.black;
                        }

                        if (iceDefence > baseIceDefence)
                        {
                            iceDefence -= 1;
                        }
                        if (iceDefence == baseIceDefence)
                        {
                            PlayerUI.MyInstance.playerIceDefence.color = Color.black;
                        }

                        if (lightningDefence > baseLightningDefence)
                        {
                            lightningDefence -= 1;
                        }
                        if (lightningDefence == baseLightningDefence)
                        {
                            PlayerUI.MyInstance.playerLightningDefence.color = Color.black;
                        }

                        if (decayResistance > baseDecayResistance)
                        {
                            decayResistance -= 1;
                        }
                        if (decayResistance == baseDecayResistance)
                        {
                            PlayerUI.MyInstance.playerDecayResistance.color = Color.black;
                        }

                        if (luck > baseLuck)
                        {
                            luck -= 1;
                        }
                        if (luck == baseLuck)
                        {
                            PlayerUI.MyInstance.playerLuck.color = Color.black;
                        }
                    }
                    #endregion
                    break;

                case "resistance":
                    #region Resistance
                    int tempRes = 0;
                    if (resistance > baseResistance)
                    {
                        tempRes = resistance;
                        upgratablePoints++;
                        numOfUpgrades--;
                        resistance--;
                    }
                    if (resistance == baseResistance)
                    {
                        PlayerUI.MyInstance.playerResistance.color = Color.black;
                    }

                    if (tempRes > resistance)
                    {
                        if (physicalDefence > basePhysicalDefence)
                        {
                            physicalDefence -= 1;
                        }
                        if (physicalDefence == basePhysicalDefence)
                        {
                            PlayerUI.MyInstance.playerDefence.color = Color.black;
                        }

                        if (fireDefence > baseFireDefence)
                        {
                            fireDefence -= 1;
                        }
                        if (fireDefence == baseFireDefence)
                        {
                            PlayerUI.MyInstance.playerFireDefence.color = Color.black;
                        }

                        if (iceDefence > baseIceDefence)
                        {
                            iceDefence -= 1;
                        }
                        if (iceDefence == baseIceDefence)
                        {
                            PlayerUI.MyInstance.playerIceDefence.color = Color.black;
                        }

                        if (lightningDefence > baseLightningDefence)
                        {
                            lightningDefence -= 1;
                        }
                        if (lightningDefence == baseLightningDefence)
                        {
                            PlayerUI.MyInstance.playerLightningDefence.color = Color.black;
                        }

                        if (petrifyResistance > basePetrifyResistance)
                        {
                            petrifyResistance -= 1;
                        }
                        if (petrifyResistance == basePetrifyResistance)
                        {
                            PlayerUI.MyInstance.playerPetrifyResistance.color = Color.black;
                        }
                    }
                    #endregion
                    break;

                case "sorcery":
                    #region Sorcery
                    int tempSor = 0;
                    if (sorcery > baseSorcery)
                    {
                        tempSor = sorcery;
                        upgratablePoints++;
                        numOfUpgrades--;
                        sorcery--;
                    }
                    if (sorcery == baseSorcery)
                    {
                        PlayerUI.MyInstance.playerSorcery.color = Color.black;
                    }

                    if (tempSor > sorcery)
                    {
                        if(magicDamage > baseMagicDamage)
                        {
                            magicDamage -= 1;
                        }
                        if(magicDamage == baseMagicDamage)
                        {
                            PlayerUI.MyInstance.playerMagicDamage.color = Color.black;
                        }

                        if (fireDefence > baseFireDefence)
                        {
                            fireDefence -= 1;
                        }
                        if (fireDefence == baseFireDefence)
                        {
                            PlayerUI.MyInstance.playerFireDefence.color = Color.black;
                        }

                        if (iceDefence > baseIceDefence)
                        {
                            iceDefence -= 1;
                        }
                        if (iceDefence == baseIceDefence)
                        {
                            PlayerUI.MyInstance.playerIceDefence.color = Color.black;
                        }

                        if (lightningDefence > baseLightningDefence)
                        {
                            lightningDefence -= 1;
                        }
                        if (lightningDefence == baseLightningDefence)
                        {
                            PlayerUI.MyInstance.playerLightningDefence.color = Color.black;
                        }
                    }
                    #endregion
                    break;
            }
        }
        if (numOfUpgrades <= 0)
        {
            upgrading = false;
            PlayerUI.MyInstance.playerSpirit.color = Color.black;
            PlayerUI.MyInstance.playerMind.color = Color.black;
            PlayerUI.MyInstance.playerResistance.color = Color.black;
            PlayerUI.MyInstance.playerSorcery.color = Color.black;
        }
    }

    public void AcceptUpgrades()
    {
        upgrading = false;
        numOfUpgrades = 0;
        PlayerUI.MyInstance.playerSpirit.color = Color.black;
        PlayerUI.MyInstance.playerMind.color = Color.black;
        PlayerUI.MyInstance.playerResistance.color = Color.black;
        PlayerUI.MyInstance.playerSorcery.color = Color.black;

        PlayerUI.MyInstance.playerHealth            .color = Color.black;
        PlayerUI.MyInstance.playerMana              .color = Color.black;
        PlayerUI.MyInstance.playerMagicDamage       .color = Color.black;
        PlayerUI.MyInstance.playerDefence           .color = Color.black;
        PlayerUI.MyInstance.playerFireDefence       .color = Color.black;
        PlayerUI.MyInstance.playerIceDefence        .color = Color.black;
        PlayerUI.MyInstance.playerLightningDefence  .color = Color.black;
        PlayerUI.MyInstance.playerPoisonResistance  .color = Color.black;
        PlayerUI.MyInstance.playerDecayResistance   .color = Color.black;
        PlayerUI.MyInstance.playerPetrifyResistance .color = Color.black;
        PlayerUI.MyInstance.playerFortune           .color = Color.black;
        PlayerUI.MyInstance.playerLuck              .color = Color.black;

        SetPlayerStats();
    }

    public void SetPlayerStats()
    {
        baseSpirit = spirit;
        baseMind = mind;
        baseResistance = resistance;
        baseSorcery = sorcery;

        baseMaxHealth = maxHealth;
        baseMaxMana = maxMana;
        baseMagicDamage = magicDamage;
        basePhysicalDefence = physicalDefence;
        baseFireDefence = fireDefence;
        baseIceDefence = iceDefence;
        baseLightningDefence = lightningDefence;
        basePoisonResistance = poisonResistance;
        baseDecayResistance = decayResistance;
        basePetrifyResistance = petrifyResistance;
        baseFortune = fortune;
        baseLuck = luck;
    }

    public void UpgradeWeaponStats(MonsterLoot item)
    {
        if(equipedWeapon)
        {
            if(equipedWeapon.attack + item.attack <= equipedWeapon.maxAttack)
            {
                equipedWeapon.attack += item.attack;
                if (item.attack > 0)
                {
                    ui.weaponAttack.color = Color.cyan;
                }
                else if (item.attack < 0)
                {
                    ui.weaponAttack.color = Color.red;
                }
            }

            if (equipedWeapon.durability + item.durability <= equipedWeapon.maxDurability)
            {
                equipedWeapon.durability += item.durability;
                if (item.durability > 0)
                {
                    ui.weaponDurability.color = Color.cyan;
                    equipedWeapon.maxHP += item.durability * 2;
                    ui.SetMaxWeaponHealth(equipedWeapon.maxHP);
                    ui.weaponMaxHP.color = Color.cyan;
                }
                else if (item.durability < 0)
                {
                    ui.weaponDurability.color = Color.red;
                }
                equipedWeapon.endurance = Mathf.FloorToInt(equipedWeapon.durability / 2);
            }

            if(equipedWeapon.beast + item.beast <= equipedWeapon.maxBeast)
            {
                equipedWeapon.beast += item.beast;
                if (item.beast > 0)
                {
                    ui.weaponBeast.color = Color.cyan;
                }
                else if (item.beast < 0)
                {
                    ui.weaponBeast.color = Color.red;
                }
            }
            if (equipedWeapon.flying + item.flying <= equipedWeapon.maxFlying)
            {
                equipedWeapon.flying += item.flying;
                if (item.flying > 0)
                {
                    ui.weaponFlying.color = Color.cyan;
                }
                else if (item.flying < 0)
                {
                    ui.weaponFlying.color = Color.red;
                }
            }

            if (equipedWeapon.smash + item.smash <= equipedWeapon.maxSmash)
            {
                equipedWeapon.smash += item.smash;
                if (item.smash > 0)
                {
                    ui.weaponSmash.color = Color.cyan;
                }
                else if (item.smash < 0)
                {
                    ui.weaponSmash.color = Color.red;
                }
            }

            if (equipedWeapon.fire + item.fire <= equipedWeapon.maxFire)
            {
                equipedWeapon.fire += item.fire;
                if (item.fire > 0)
                {
                    ui.weaponFire.color = Color.cyan;
                }
                else if (item.fire < 0)
                {
                    ui.weaponFire.color = Color.red;
                }
            }

            if (equipedWeapon.ice + item.ice <= equipedWeapon.maxIce)
            {
                equipedWeapon.ice += item.ice;
                if (item.ice > 0)
                {
                    ui.weaponIce.color = Color.cyan;
                }
                else if (item.ice < 0)
                {
                    ui.weaponIce.color = Color.red;
                }
            }

            if (equipedWeapon.lightning + item.lightning <= equipedWeapon.maxLightning)
            {
                equipedWeapon.lightning += item.lightning;
                if (item.lightning > 0)
                {
                    ui.weaponLightning.color = Color.cyan;
                }
                else if (item.lightning < 0)
                {
                    ui.weaponLightning.color = Color.red;
                }
            }
        }
    }

    public void DowngradeWeaponStats(MonsterLoot item)
    {
        if(equipedWeapon.attack + item.attack <= equipedWeapon.maxAttack)
            equipedWeapon.attack -= item.attack;
        ui.weaponAttack.color = Color.black;

        if (equipedWeapon.durability + item.durability <= equipedWeapon.maxDurability)
        {
            equipedWeapon.durability -= item.durability;
            equipedWeapon.maxHP -= item.durability * 2;
            ui.SetMaxWeaponHealth(equipedWeapon.maxHP);
        }
        ui.weaponDurability.color = Color.black;
        ui.weaponMaxHP.color = Color.white;

        if (equipedWeapon.beast + item.beast <= equipedWeapon.maxBeast)
            equipedWeapon.beast -= item.beast;
        ui.weaponBeast.color = Color.black;

        if (equipedWeapon.flying + item.flying <= equipedWeapon.maxFlying)
            equipedWeapon.flying -= item.flying;
        ui.weaponFlying.color = Color.black;

        if (equipedWeapon.smash + item.smash <= equipedWeapon.maxSmash)
            equipedWeapon.smash -= item.smash;
        ui.weaponSmash.color = Color.black;

        if (equipedWeapon.fire + item.fire <= equipedWeapon.maxFire)
            equipedWeapon.fire -= item.fire;
        ui.weaponFire.color = Color.black;

        if (equipedWeapon.ice + item.ice <= equipedWeapon.maxIce)
            equipedWeapon.ice -= item.ice;
        ui.weaponIce.color = Color.black;

        if (equipedWeapon.lightning + item.lightning <= equipedWeapon.maxLightning)
            equipedWeapon.lightning -= item.lightning;
        ui.weaponLightning.color = Color.black;
    }

    public void AcceptWeaponUpgrades()
    {

        if(equipedWeapon != null && equipedWeapon.upgradePoints > 0 && WeaponUpgradeSlot.MyInstance.GetComponentInChildren<InventoryItem>().MyItem != null)
        {
            equipedWeapon.upgradePoints--;
            WeaponUpgradeSlot.MyInstance.GetComponentInChildren<InventoryItem>().numOfItems--;
            if(WeaponUpgradeSlot.MyInstance.GetComponentInChildren<InventoryItem>().numOfItems <= 0)
            {
                weaponUpgrading = false;
                WeaponUpgradeSlot.MyInstance.myItem = null;
                WeaponUpgradeSlot.MyInstance.GetComponentInChildren<InventoryItem>().RemoveItem();

                EquipSlots[] slots = FindObjectsOfType<EquipSlots>();
                foreach (EquipSlots slot in slots)
                {
                    if (slot.gameObject.GetComponentInChildren<InventoryItem>() != null)
                    {
                        if ((slot.gameObject.GetComponentInChildren<InventoryItem>().MyItem as Weapon) != null)
                        {
                            slot.gameObject.GetComponentInChildren<InventoryItem>().shopItem = false;
                        }
                    }
                }
                ui.weaponAttack.color = Color.black;
                ui.weaponDurability.color = Color.black;
                ui.weaponBeast.color = Color.black;
                ui.weaponFlying.color = Color.black;
                ui.weaponSmash.color = Color.black;
                ui.weaponFire.color = Color.black;
                ui.weaponIce.color = Color.black;
                ui.weaponLightning.color = Color.black;
                ui.weaponMaxHP.color = Color.white;
            }
            else
            {
                WeaponUpgradeSlot.MyInstance.GetComponentInChildren<InventoryItem>().numOfItemsTxt.text =
                    WeaponUpgradeSlot.MyInstance.GetComponentInChildren<InventoryItem>().numOfItems.ToString();
                UpgradeWeaponStats(WeaponUpgradeSlot.MyInstance.GetComponentInChildren<InventoryItem>().MyItem as MonsterLoot);
            }
        }
    }

    public void GainWeaponXP(float xp)
    {
        equipedWeapon.XP += xp;
        if(equipedWeapon.XP >= equipedWeapon.maxXP)
        {
            WeaponLevelUp();
        }
        ui.SetWeaponXP(equipedWeapon.XP);
    }

    public void WeaponLevelUp()
    {
        CombatTextManager.MyInstance.CreateText(WeaponLevelUpTextPopUpHolder.MyInstance.transform.position, "WeaponUp", TextType.wpLevelUp);
        equipedWeapon.level++;
        equipedWeapon.upgradePoints += 3;
        float offset = equipedWeapon.XP - equipedWeapon.maxXP;
        equipedWeapon.maxXP = Mathf.Floor(100 * level * Mathf.Pow(level, 0.5f));
        equipedWeapon.XP = offset;
        equipedWeapon.attack += 1;
        equipedWeapon.durability += 1;
        equipedWeapon.endurance = Mathf.FloorToInt(equipedWeapon.durability / 2);
        equipedWeapon.maxHP += Random.Range(1, 4);
        ui.SetMaxWeaponHealth(equipedWeapon.maxHP);
    }

    public void ToFadeXpBar()
    {
        counter +=0.01f;
        if (counter >= timeToFade)
        {
            counter = 0f;
            StartCoroutine(FadeCo());
        }
    }

    public void LoseMana(int loseMana)
    {
        mana -= loseMana;
        if (mana <= 0)
            mana = 0;
        ui.SetMana(mana);
    }

    public void GainCoins(int num)
    {
        coins += num;
        //CombatTextManager.MyInstance.CreateText(transform.position, num.ToString(), TextType.gainCoin);
        CombatTextManager.MyInstance.CreateText(CoinTextPopUpHolder.MyInstance.transform.position, num.ToString(), TextType.gainCoin);
        ui.SetCoins(coins);
    }

    public Weapon FindEquipedWeapon()
    {
        EquipSlots[] slots = FindObjectsOfType<EquipSlots>();
        foreach (EquipSlots slot in slots)
        {
            if(slot.gameObject.GetComponentInChildren<InventoryItem>() != null)
            {
                if ((slot.gameObject.GetComponentInChildren<InventoryItem>().MyItem as Weapon) != null)
                {
                    Weapon wp = (slot.gameObject.GetComponentInChildren<InventoryItem>().MyItem as Weapon);
                    return wp;
                }
            }
        }
        return null;
    }

    public Armor FindEquipmentOfType(ArmorType type)
    {
        Armor equipment = null;
        EquipSlots[] slots = FindObjectsOfType<EquipSlots>();
        foreach (EquipSlots slot in slots)
        {
            if (slot.armorType == type)
            {
                equipment = slot.gameObject.GetComponentInChildren<InventoryItem>().MyItem as Armor;
                break;
            }
        }
        return equipment;
    }

    public Armor ChooseAccessorySlot(int number)
    {
        EquipSlots[] slots = FindObjectsOfType<EquipSlots>();
        List<EquipSlots> accessorySlots = new List<EquipSlots>();
        foreach (EquipSlots slot in slots)
        {
            if (slot.armorType == ArmorType.Accessory)
            {
                accessorySlots.Add(slot);
            }
        }
        return accessorySlots[number].gameObject.GetComponentInChildren<InventoryItem>().MyItem as Armor;
    }

    public void CastSpell(string spellName)
    {
        switch(spellName)
        {
            case "Heavy Strike":
                if(equipedWeapon)
                    if (mana >= 2)
                        StartCoroutine(HeavyStrikeCo());
                break;
            case "Fireball":
                if (mana >= 2)
                    StartCoroutine(FireballCo());
                break;
            case "Ice Shards":
                if(mana >= 3)
                    StartCoroutine(IceShardsCo());
                break;
            case "Lightning Bolt":
                if (mana >= 5)
                    StartCoroutine(LightningBoltCo());
                break;
            case "Poisonous Touch":
                if (mana >= 4)
                    StartCoroutine(PoisonousTouchCo());
                break;
            case "Defence Up":
                if (mana >= 6)
                    StartCoroutine(DefenceUpCo());
                break;
        }
    }

    #endregion

    #region IENUMERATORS
    private IEnumerator AttackCo()
    {
        // ATTACKING 1
        animator.SetTrigger("Attacking1");
        foreach (PlayerEquipment pe in equipments)
        {
            pe.SetAnimatorTrigger("Attacking1");
        }
        yield return new WaitForSeconds(.01f);
        currentState = PlayerState.attack1;
        yield return null;
        MovePlayerWhenAttacking();
        yield return new WaitForSeconds(.65f);
        canCombo = true;
        yield return new WaitForSeconds(.15f);
        if((!shouldCombo1 || currentState == PlayerState.stagger) 
            && currentState != PlayerState.cutscene && currentState != PlayerState.interact)
        {
            currentState = PlayerState.run;
            canCombo = true;
        }
    }

    private IEnumerator AttackCo2()
    {
        // ATTACKING 2
        animator.SetTrigger("Attacking2");
        foreach (PlayerEquipment pe in equipments)
        {
            pe.SetAnimatorTrigger("Attacking2");
        }
        yield return new WaitForSeconds(.01f);
        currentState = PlayerState.attack2;
        yield return null;
        yield return new WaitForSeconds(.65f);
        canCombo = true;
        yield return new WaitForSeconds(.15f);
        if ((!shouldCombo2 || currentState == PlayerState.stagger)
            && currentState != PlayerState.cutscene && currentState != PlayerState.interact)
            currentState = PlayerState.run;
    }

    private IEnumerator AttackCo3()
    {
        // ATTACKING 3
        animator.SetTrigger("Attacking3");
        foreach (PlayerEquipment pe in equipments)
        {
            pe.SetAnimatorTrigger("Attacking3");
        }
        yield return new WaitForSeconds(.01f);
        currentState = PlayerState.attack3;
        yield return null;
        //MovePlayerWhenAttacking();
        yield return new WaitForSeconds(1.2f);
        shouldCombo2 = false;
        canCombo = true;
        if(currentState != PlayerState.cutscene && currentState != PlayerState.interact)
            currentState = PlayerState.run;
    }

    private IEnumerator HeavyStrikeCo()
    {
        animator.SetTrigger("Heavy Strike");
        foreach (PlayerEquipment pe in equipments)
        {
            pe.SetAnimatorTrigger("Heavy Strike");
        }
        currentState = PlayerState.heavyStrike;
        yield return new WaitForSeconds(0.6f);
        MovePlayerWhenAttacking();
        LoseMana(2);
        yield return new WaitForSeconds(1.3f);
        if (currentState != PlayerState.cutscene && currentState != PlayerState.interact)
            currentState = PlayerState.run;
    }

    private IEnumerator FireballCo()
    {
        // FIREBALL MOVEMENT DIRECTION
        Vector3 move = Vector3.zero;
        move.x = animator.GetFloat("Horizontal");
        move.y = animator.GetFloat("Vertical");

        animator.SetTrigger("Fireball");
        currentState = PlayerState.fireAttack;
        MovePlayerWhenAttacking();
        yield return new WaitForSeconds(0.4f);

        if(currentState != PlayerState.stagger)
        {
            spellPrefab[0].gameObject.SetActive(true);
            Instantiate(spellPrefab[0], transform.position + move, Quaternion.identity);
            LoseMana(2);
        }
        yield return new WaitForSeconds(0.5f);
        if (currentState != PlayerState.cutscene && currentState != PlayerState.interact)
            currentState = PlayerState.run;
    }

    private IEnumerator IceShardsCo()
    {
        // ICE SHARDS MOVEMENT DIRECTION
        Vector3 move = Vector3.zero;
        move.x = animator.GetFloat("Horizontal");
        move.y = animator.GetFloat("Vertical");

        animator.SetTrigger("Ice Shards");
        currentState = PlayerState.iceAttack;
        yield return new WaitForSeconds(0.4f);

        if (currentState != PlayerState.stagger)
        {
            spellPrefab[1].gameObject.SetActive(true);
            for(int i=0; i<5; i++)
            {
                Vector3 spawn;
                if (move.x != 0)
                    spawn = new Vector3(0, Random.Range(-1f, 1f), 0);
                else
                    spawn = new Vector3(Random.Range(-1f, 1f), 0, 0);
                yield return new WaitForSeconds(0.08f);
                IceShard iceShard = Instantiate(spellPrefab[1], transform.position + move + spawn, Quaternion.identity).GetComponent<IceShard>();
            }
            //Instantiate(spellPrefab[1], transform.position + move, Quaternion.identity);
            LoseMana(3);
        }
        yield return new WaitForSeconds(0.4f);
        if (currentState != PlayerState.cutscene && currentState != PlayerState.interact)
            currentState = PlayerState.run;
    }

    private IEnumerator LightningBoltCo()
    {
        // Lightning Bolt Direction
        Vector3 move = Vector3.zero;
        move.x = animator.GetFloat("Horizontal");
        move.y = animator.GetFloat("Vertical");
        if (move.x != 0)
        {
            move.x *= 5;
            move.y += 1f;
        }
        else if (move.y > 0)
            move.y *= 5;
        else
            move.y *= 3;

        animator.SetTrigger("Lightning Bolt");
        currentState = PlayerState.lightningAttack;
        yield return new WaitForSeconds(0.5f);

        if (currentState != PlayerState.stagger)
        {
            spellPrefab[2].gameObject.SetActive(true);
            Instantiate(spellPrefab[2], transform.position + move, Quaternion.identity);
            LoseMana(5);
        }
        yield return new WaitForSeconds(0.5f);
        if (currentState != PlayerState.cutscene && currentState != PlayerState.interact)
            currentState = PlayerState.run;
    }

    private IEnumerator PoisonousTouchCo()
    {
        // Poison Ball MOVEMENT DIRECTION
        Vector3 move = Vector3.zero;
        move.x = animator.GetFloat("Horizontal");
        move.y = animator.GetFloat("Vertical");

        animator.SetTrigger("Poisonous Touch");
        currentState = PlayerState.poisonAttack;
        yield return new WaitForSeconds(0.4f);
        MovePlayerWhenAttacking();

        if (move.y < 0)
            move.y -= 0.3f;

        if (currentState != PlayerState.stagger)
        {
            spellPrefab[3].gameObject.SetActive(true);
            Instantiate(spellPrefab[3], transform.position + move, Quaternion.identity);
            LoseMana(4);
        }
        yield return new WaitForSeconds(0.5f);
        if (currentState != PlayerState.cutscene && currentState != PlayerState.interact)
            currentState = PlayerState.run;
    }

    private IEnumerator DefenceUpCo()
    {
        // Poison Ball MOVEMENT DIRECTION
        Vector3 move = Vector3.zero;
        move.x = animator.GetFloat("Horizontal");
        move.y = animator.GetFloat("Vertical");

        animator.SetTrigger("Defence Up");
        currentState = PlayerState.buffAttack;

        yield return new WaitForSeconds(0.5f);

        if (currentState != PlayerState.stagger)
        {
            statusEffects.AddStatusEffectWithTimer(24, 60f);
            LoseMana(6);
        }
        yield return new WaitForSeconds(0.9f);
        if (currentState != PlayerState.cutscene && currentState != PlayerState.interact)
            currentState = PlayerState.run;
    }

    private IEnumerator KnockCo(float knockTime)
    {
        if (rb != null)
        {
            currentState = PlayerState.stagger;
            animator.SetTrigger("Staggering");
            foreach (PlayerEquipment pe in equipments)
                pe.SetAnimatorTrigger("Staggering");
            yield return new WaitForSeconds(knockTime);

            rb.velocity = Vector2.zero;
            // PLAYER CANT MOVE WHILE INVENTORY IS OPENED
            if(openInventory || currentState == PlayerState.interact)
                currentState = PlayerState.interact;
            else
            {
                currentState = PlayerState.run;
                shouldCombo1 = false;
                shouldCombo2 = false;
                canCombo = true;
            }

            rb.velocity = Vector2.zero;
        }
    }

    private IEnumerator EatCo(Item item)
    {
        animator.SetTrigger("Eating");
        foreach (PlayerEquipment pe in equipments)
        {
            // Weapon is invisible
            if(pe.equipType == ArmorType.Weapon)
            {
                Color c = pe.SpriteRenderer.color;
                c.a = 0;
                pe.SpriteRenderer.color = c;
            }
            pe.SetAnimatorBool("RecieveItem", true);
        }
        yield return new WaitForSeconds(1f);
        foreach (PlayerEquipment pe in equipments)
        {
            // Weapon is visible again
            if(pe.equipType == ArmorType.Weapon)
                pe.SpriteRenderer.color = Color.white;
            pe.SetAnimatorBool("RecieveItem", false);
        }
        if (currentState != PlayerState.stagger)
        {
            itemUsed = true;
            InventoryQuickUseSlots.MyInstance.slotWithUsedItem.DecreceItemNumber();
            (item as IUsable).UseItem();
            currentState = PlayerState.run;
        }
    }

    private IEnumerator DrinkCo(Item item)
    {
        animator.SetTrigger("Drinking");
        foreach (PlayerEquipment pe in equipments)
        {
            // Weapon is invisible
            if (pe.equipType == ArmorType.Weapon)
            {
                Color c = pe.SpriteRenderer.color;
                c.a = 0;
                pe.SpriteRenderer.color = c;
            }
            pe.SetAnimatorBool("RecieveItem", true);
        }
        yield return new WaitForSeconds(1f);
        foreach (PlayerEquipment pe in equipments)
        {
            // Weapon is visible again
            if (pe.equipType == ArmorType.Weapon)
                pe.SpriteRenderer.color = Color.white;
            pe.SetAnimatorBool("RecieveItem", false);
        }
        if (currentState != PlayerState.stagger)
        {
            itemUsed = true;
            InventoryQuickUseSlots.MyInstance.slotWithUsedItem.DecreceItemNumber();
            (item as IUsable).UseItem();
            currentState = PlayerState.run;
        }
    }

    private IEnumerator RepairWeaponCo(Item item)
    {
        animator.SetTrigger("RepairWeapon");
        foreach (PlayerEquipment pe in equipments)
        {
            if (pe.equipType == ArmorType.Weapon)
                pe.SetAnimatorTrigger("RepairWeapon");
            else
                pe.SetAnimatorBool("RecieveItem", true);
        }
        yield return new WaitForSeconds(1.3f);
        foreach (PlayerEquipment pe in equipments)
        {
            pe.SetAnimatorBool("RecieveItem", false);
        }
        if (currentState != PlayerState.stagger)
        {
            itemUsed = true;
            InventoryQuickUseSlots.MyInstance.slotWithUsedItem.DecreceItemNumber();
            (item as IUsable).UseItem();
            currentState = PlayerState.run;
        }
    }

    private IEnumerator FadeCo()
    {
        xpBar.GetComponent<Animator>().SetTrigger("Fading");
        yield return new WaitForSeconds(1f);
        xpBar.gameObject.SetActive(false);
    }

    private IEnumerator DeathCo()
    {
        inventory.GetComponent<Animator>().SetTrigger("Deactivate");
        ui.PlayerDeath();
        this.gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
    }
    #endregion
}
