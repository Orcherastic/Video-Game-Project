using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    private static MainCamera instance;

    public static MainCamera MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<MainCamera>();
            }
            return instance;
        }
    }

    public Transform target;
    public Animator animator;
    public float smoothing;
    public bool cutscene = false;

    // Set your room borders
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    // Start is called before the first frame update
    void Start()
    {
        target = FindObjectOfType<Player>().transform;
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPos;
        if(target != null)
        {
            targetPos = new Vector3(target.position.x, target.position.y, transform.position.z);
            if (!cutscene)
                transform.position = Vector3.Lerp(transform.position, targetPos, smoothing);
            else
                transform.position = Vector3.Lerp(transform.position, targetPos, 0.02f);
        }

        // Clamp camera position within room borders
        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        float clampedY = Mathf.Clamp(transform.position.y, minY, maxY);
        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }

    public void MoveToPosition(Transform newTarget)
    {
        target = newTarget;
    }

    public void SmallShake()
    {
        animator.SetTrigger("Shake");
    }

    public void BigShake()
    {
        animator.SetTrigger("Big Shake");
    }
}
