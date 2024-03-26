using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ChangeSceneInTimeline : MonoBehaviour
{
    public float changeTime;
    public string sceneName;
    public CutsceneSkipButton button;
    private bool canSkip = false;

    private void Start()
    {
        button = FindObjectOfType<CutsceneSkipButton>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Interact") && button != null)
        {
            if (canSkip)
                changeTime = 0;
            else
            {
                button.anim.SetTrigger("FadeIn");
                canSkip = true;
            }
        }
        changeTime -= Time.deltaTime;
        if(changeTime <= 0)
            SceneManager.LoadScene(sceneName);
    }
}
