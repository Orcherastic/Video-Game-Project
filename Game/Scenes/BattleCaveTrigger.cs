using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCaveTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            TutorialBattleCaveEvents.MyInstance.StartCoroutine(TutorialBattleCaveEvents.MyInstance.CloseGates());
            gameObject.SetActive(false);
        }
    }
}
