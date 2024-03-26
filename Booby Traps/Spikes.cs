using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    private Animator anim;
    public float timer = 2f;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= 0.01f;
        if(timer <= 0)
        {
            StartCoroutine(PopCo());
        }
    }

    private IEnumerator PopCo()
    {
        anim.SetBool("PopUp", true);
        yield return new WaitForSeconds(0.2f);
        timer = 2f;
        anim.SetBool("PopUp", false);
    }
}
