using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThinkBubble : MonoBehaviour
{
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void PopUp()
    {
        this.gameObject.SetActive(true);
        StartCoroutine(PopUpCo());
    }

    public void PopDown()
    {
        StartCoroutine(PopDownCo());
    }

    private IEnumerator PopUpCo()
    {
        anim.SetBool("PopUp", true);
        yield return new WaitForSeconds(0.2f);
        
    }

    private IEnumerator PopDownCo()
    {
        anim.SetBool("PopUp", false);
        yield return new WaitForSeconds(0.3f);
        if(!Player.MyInstance.inTriggerRange || Player.MyInstance.currentState == PlayerState.interact)
            this.gameObject.SetActive(false);
    }
}
