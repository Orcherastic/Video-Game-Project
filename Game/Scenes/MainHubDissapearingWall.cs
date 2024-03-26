using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainHubDissapearingWall : MonoBehaviour
{
    public GameObject normalWall;
    public GameObject dissapearedWall;
    private bool entered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player") && !entered)
        {
            entered = true;
            normalWall.SetActive(false);
            dissapearedWall.SetActive(true);
        }
    }
}
