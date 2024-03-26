using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        Initialize();
    }

    public void Initialize()
    {
        int random = Random.Range(0, 101);
        if (random < 50)
            anim.SetTrigger("Tree2");
    }
}
