using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCave2TextTrigger : MonoBehaviour
{
    public TutorialPopUpsSaver saver;
    private bool popedUp = false;

    private void Start()
    {
        LoadPoped();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !popedUp)
        {
            popedUp = true;
            SavePoped();
            CombatTextManager.MyInstance.CreateTextWorld(transform.position, "Pick it up.", TextType.levelUp);
            Destroy(this.gameObject);
        }
    }

    public void SavePoped()
    {
        saver.pickItUpTrigger = popedUp;
    }

    public void LoadPoped()
    {
        popedUp = saver.pickItUpTrigger;
    }
}
