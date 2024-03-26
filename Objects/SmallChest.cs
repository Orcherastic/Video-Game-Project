using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallChest : MonoBehaviour
{
    private Animator anim;
    public Item pickUpPrefab;
    public bool playerInRange = false;
    public bool isOpened = false;
    public bool clicked = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Interact") && playerInRange && Player.MyInstance.inTriggerRange
            && !clicked)
        {
            if (!isOpened)
            {
                if (InventoryItemSlots.MyInstance.InventoryIsFull(pickUpPrefab))
                {
                    return;
                }
                clicked = true;
                Player.MyInstance.currentState = PlayerState.interact;
                Player.MyInstance.ThinkBubble.PopDown();
                StartCoroutine(OpenCo());
            }
            else
            {
                clicked = true;
                StartCoroutine(RecieveItemCo());
            }
        }
    }

    public void OpenChest()
    {
        isOpened = true;
        if (pickUpPrefab is Weapon)
        {
            (pickUpPrefab as Weapon).HP = (pickUpPrefab as Weapon).maxHP;
            (pickUpPrefab as Weapon).XP = 0;
        }
        
        if(pickUpPrefab.itemType != ItemType.SpellBook)
        {
            Item item = Instantiate(pickUpPrefab, transform.position, Quaternion.identity);
            item.gameObject.SetActive(false);
            InventoryItemSlots.MyInstance.AddItem(item);
        }
        else
        {
            PlayerSpellsManager.MyInstance.playerSpells.spellBooks.Add(pickUpPrefab);
            foreach (MagicSlot slot in InventoryMagicSlots.MyInstance.slots)
            {
                if (pickUpPrefab.itemName == slot.myMagicName)
                {
                    slot.unlocked = true;
                    break;
                }
            }
        }

        // Show ItemPickUpPopUp when an Item is Picked Up
        ItemPickUpPopUpController.MyInstance.CreateInstance(pickUpPrefab);

        Player.MyInstance.RecieveItem(pickUpPrefab);
        clicked = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            playerInRange = true;
            Player.MyInstance.inTriggerRange = true;
            if(!isOpened)
                Player.MyInstance.ThinkBubble.PopUp();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            playerInRange = false;
            Player.MyInstance.inTriggerRange = false;
            if(!isOpened)
                Player.MyInstance.ThinkBubble.PopDown();
        }
    }

    private IEnumerator OpenCo()
    {
        anim.SetTrigger("Opening");
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach(Enemy enemy in enemies)
        {
            enemy.currentState = EnemyStates.wait;
        }
        yield return new WaitForSeconds(1f);
        OpenChest();
    }

    private IEnumerator RecieveItemCo()
    {
        yield return new WaitForSeconds(0.3f);
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            enemy.currentState = EnemyStates.idle;
        }
        Player.MyInstance.currentState = PlayerState.run;
        Player.MyInstance.RecieveItem(pickUpPrefab);
        clicked = false;
        anim.SetTrigger("Opened");
    }
}
