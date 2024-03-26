using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffectController : MonoBehaviour
{
    private static HitEffectController instance;

    public static HitEffectController MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<HitEffectController>();
            }
            return instance;
        }
    }

    [SerializeField]
    private HitEffect hitEffectPrefab;

    public void CreateHitEffect(Vector2 position)
    {
        HitEffect hit = Instantiate(hitEffectPrefab, transform);
        hit.transform.position = position;
    }
}
