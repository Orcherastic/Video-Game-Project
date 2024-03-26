using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YSorterPlayerEquipment : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public bool helmet;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // Adjust sorting order based on y-position
        if(!helmet)
            spriteRenderer.sortingOrder = (Mathf.RoundToInt(transform.position.y * 100f) * -1) + 1;
        else
            spriteRenderer.sortingOrder = (Mathf.RoundToInt(transform.position.y * 100f) * -1) + 27;
    }
}
