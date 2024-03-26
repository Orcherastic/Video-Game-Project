using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustEffect : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(PopCo());
    }

    private IEnumerator PopCo()
    {
        yield return new WaitForSeconds(0.58f);
        gameObject.SetActive(false);
    }
}
