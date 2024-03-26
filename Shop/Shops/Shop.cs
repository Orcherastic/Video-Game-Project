using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShopType
{
    magic,
    item,
    armory
}

public class Shop : MonoBehaviour
{
    public ShopType shopType;

    public bool playerInRange = false;
    public bool shopOpened = false;
    public bool deactivatedDialogueBox = false;
    public ShopUI shop;
    public MagicShopItems magicShopItems;
    public ItemShopItems itemShopItems;
    public WeaponShopItems weaponShopItems;
    public TabGroup tabs;
    public Item giftPrefab;
    private bool giftGiven = false;
    private bool giftRecieved = false;

    protected string talker;
    public List<string> talking;
    public List<string> optionTexts;
    protected int numTalkedTo = 0;
    protected bool hasOptions = false;
    protected bool talkSelected = false;
    protected int numOfTalkSelected = 0;

    private int numOfText = 0;

    protected void Awake()
    {
        InitializeComponents();
    }

    protected void Start()
    {
        shop.GetComponent<Animator>().SetTrigger("Deactivate");
    }

    private void InitializeComponents()
    {
        shop = FindObjectOfType<ShopUI>();
        magicShopItems = FindObjectOfType<MagicShopItems>();
        itemShopItems = FindObjectOfType<ItemShopItems>();
        weaponShopItems = FindObjectOfType<WeaponShopItems>();
    }

    private void Update()
    {
        if (shopOpened == true)
            PlayerUI.MyInstance.tooltip.transform.SetParent(shop.transform);
        if (Input.GetButtonDown("Interact") && playerInRange && !DialogueBoxManager.MyInstance.dialogueBox.fullSentence
            && !Player.MyInstance.weaponRepairing)
        {
            if(!shopOpened)
            {
                // Start Dialogue
                if (!DialogueBoxManager.MyInstance.dialogueBox.typing)
                {
                    talkSelected = false;
                    Player.MyInstance.currentState = PlayerState.interact;
                    SetTalkerAndTalking();

                    DialogueBoxManager.MyInstance.Activate();
                    if (!hasOptions)
                        DialogueBoxManager.MyInstance.dialogueBox.SetDialogue(talker, talking[numOfText]);
                    else
                        DialogueBoxManager.MyInstance.dialogueBox.SetDialogueWithOptions(talker, talking[numOfText], optionTexts);
                }
                // Finish Dialogue, end typing, show full text
                else
                    DialogueBoxManager.MyInstance.dialogueBox.finish = true;
            }
            else
            {
                shopOpened = false;
                shop.transform.SetSiblingIndex(5);
                Player.MyInstance.currentState = PlayerState.run;
                Player.MyInstance.shopping = false;
                shop.GetComponent<Animator>().SetTrigger("Deactivate");

                magicShopItems.gameObject.SetActive(false);
                itemShopItems.gameObject.SetActive(false);
                weaponShopItems.gameObject.SetActive(false);

                PlayerUI.MyInstance.tooltip.transform.SetParent(Player.MyInstance.inventory.transform);
                DialogueBoxManager.MyInstance.dialogueBox.ResetDialogue();
                optionTexts.Clear();
                numOfText = 0;
            }
        }
        if (Input.GetButtonDown("Interact") && playerInRange && DialogueBoxManager.MyInstance.dialogueBox.fullSentence
            && !Player.MyInstance.weaponRepairing)
        {
            numOfText++;
            // Show Next Text if any
            if(numOfText < talking.Count)
            {
                DialogueBoxManager.MyInstance.dialogueBox.SetDialogue(talker, talking[numOfText]);
            }

            // Selected Option or End Dialogue
            else
            {
                // Give Gift
                if (numTalkedTo == 1 && !giftGiven && !giftRecieved)
                {
                    DialogueBoxManager.MyInstance.Deactivate();
                    giftGiven = true;
                    if (giftPrefab.itemType != ItemType.SpellBook)
                    {
                        Item item = Instantiate(giftPrefab, transform.position, Quaternion.identity);
                        item.gameObject.SetActive(false);
                        InventoryItemSlots.MyInstance.AddItem(item);
                    }
                    else
                    {
                        PlayerSpellsManager.MyInstance.playerSpells.spellBooks.Add(giftPrefab);
                        foreach (MagicSlot slot in InventoryMagicSlots.MyInstance.slots)
                        {
                            if (giftPrefab.itemName == slot.myMagicName)
                            {
                                slot.unlocked = true;
                                break;
                            }
                        }
                    }
                    Player.MyInstance.ThinkBubble.PopDown();
                    Player.MyInstance.RecieveItem(giftPrefab);
                    ItemPickUpPopUpController.MyInstance.CreateInstance(giftPrefab);
                }
                // Gift Given
                else if (giftGiven && !giftRecieved)
                {
                    giftRecieved = true;
                    Player.MyInstance.currentState = PlayerState.run;
                    Player.MyInstance.RecieveItem(giftPrefab);
                    Player.MyInstance.ThinkBubble.PopUp();
                }

                if (hasOptions)
                {
                    if(DialogueBoxManager.MyInstance.dialogueBox.dialogueOptions.selectedText == "Leave")
                    {
                        hasOptions = false;
                        DialogueBoxManager.MyInstance.Deactivate();
                        DialogueBoxManager.MyInstance.dialogueBox.ResetDialogue();
                        optionTexts.Clear();
                        numOfText = 0;
                        Player.MyInstance.currentState = PlayerState.run;
                        Player.MyInstance.shopping = false;
                    }
                    if (DialogueBoxManager.MyInstance.dialogueBox.dialogueOptions.selectedText == "Talk")
                    {
                        talkSelected = true;
                        numOfTalkSelected++;
                        numOfText = 0;
                        SetTalkerAndTalking();
                        DialogueBoxManager.MyInstance.dialogueBox.RemoveOptions();
                        DialogueBoxManager.MyInstance.dialogueBox.SetDialogue(talker, talking[numOfText]);
                    }
                    if (DialogueBoxManager.MyInstance.dialogueBox.dialogueOptions.selectedText == "Shop")
                    {
                        DialogueBoxManager.MyInstance.Deactivate();
                        DialogueBoxManager.MyInstance.dialogueBox.ResetDialogue();
                        optionTexts.Clear();
                        numOfText = 0;
                        hasOptions = false;
                        shopOpened = true;
                        shop.GetComponent<Animator>().SetTrigger("Activate");
                        SetShopItems();

                        Player.MyInstance.shopping = true;
                        shop.transform.SetSiblingIndex(6);
                        tabs.SetFirstTab();
                    }
                }
                else
                {
                    if(shopOpened)
                    {
                        shopOpened = false;
                        shop.transform.SetSiblingIndex(5);
                        Player.MyInstance.currentState = PlayerState.run;
                        Player.MyInstance.shopping = false;
                        shop.GetComponent<Animator>().SetTrigger("Deactivate");

                        magicShopItems.gameObject.SetActive(false);
                        itemShopItems.gameObject.SetActive(false);
                        weaponShopItems.gameObject.SetActive(false);

                        PlayerUI.MyInstance.tooltip.transform.SetParent(Player.MyInstance.inventory.transform);
                        DialogueBoxManager.MyInstance.dialogueBox.ResetDialogue();
                        optionTexts.Clear();
                        numOfText = 0;
                    }
                    else
                    {
                        DialogueBoxManager.MyInstance.Deactivate();
                        DialogueBoxManager.MyInstance.dialogueBox.ResetDialogue();
                        optionTexts.Clear();
                        numOfText = 0;
                        Player.MyInstance.currentState = PlayerState.run;
                        Player.MyInstance.shopping = false;
                    }
                }
            }
        }
    }

    public virtual void SetTalkerAndTalking()
    {

    }

    public void SetShopItems()
    {
        switch (shopType)
        {
            case ShopType.magic:
                magicShopItems.gameObject.SetActive(true);
                break;
            case ShopType.item:
                itemShopItems.gameObject.SetActive(true);
                break;
            case ShopType.armory:
                weaponShopItems.gameObject.SetActive(true);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            playerInRange = true;
            Player.MyInstance.inTriggerRange = true;
            if (!shopOpened)
                Player.MyInstance.ThinkBubble.PopUp();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            playerInRange = false;
            Player.MyInstance.inTriggerRange = false;
            Player.MyInstance.ThinkBubble.PopDown();
        }
    }
}
