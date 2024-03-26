using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTorch : MonoBehaviour
{
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        int randomNum = Random.Range(0, 6);
        if (randomNum != 0)
            anim.SetTrigger("torch" + randomNum);
    }
}
