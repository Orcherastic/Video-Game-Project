using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatText : MonoBehaviour
{
    [SerializeField]
    private float speed = 2f;
    [SerializeField]
    private Text text;
    [SerializeField]
    private float lifeTime = 2f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeCo());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    private IEnumerator FadeCo()
    {
        float startAlpha = text.color.a;
        float rate = 1.0f / lifeTime;
        float progress = 0.0f;
        while(progress < 1.0f)
        {
            Color tmp = text.color;
            tmp.a = Mathf.Lerp(startAlpha, 0, progress);
            text.color = tmp;
            progress += rate * Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
