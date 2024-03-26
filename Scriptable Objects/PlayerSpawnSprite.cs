using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSpriteInfo", menuName = "ScriptableObjects/PlayerSpriteInfo", order = 1)]
public class PlayerSpawnSprite : ScriptableObject
{
    public float playerHorizontal;
    public float playerVertical;

    public void Reset()
    {
        playerHorizontal = 1;
        playerVertical = 0;
    }
}
