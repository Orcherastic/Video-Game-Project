using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedOverYSorter : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.MyInstance.transform.position.y < 1)
            spriteRenderer.sortingOrder = -800;
        else
            spriteRenderer.sortingOrder = 1000;
    }
}
