using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCave2Wall : MonoBehaviour
{    
    private Weapon weapon;

    public TutorialPopUpsSaver saver;
    private bool destroyed = false;
    private bool textPoped = false;

    // Start is called before the first frame update
    void Start()
    {
        weapon = FindObjectOfType<WoodenStick>();
        LoadDestroyed();
    }

    void Update()
    {
        if ((weapon == null || !weapon.gameObject.activeInHierarchy) && !destroyed)
        {
            CombatTextManager.MyInstance.CreateTextWorld(Player.MyInstance.transform.position, "Now equip your weapon.", TextType.levelUp);
            TutorialPopUps.MyInstance.finished = false;
            destroyed = true;
            SaveDestroyed();
            Destroy(this.gameObject);
        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player") && !textPoped)
        {
            CombatTextManager.MyInstance.CreateTextWorld(transform.position, "Pick up the stick.", TextType.levelUp);
            textPoped = true;
        }
    }

    public void SaveDestroyed()
    {
        saver.invisibleWall = destroyed;
    }

    public void LoadDestroyed()
    {
        destroyed = saver.invisibleWall;
    }
}
