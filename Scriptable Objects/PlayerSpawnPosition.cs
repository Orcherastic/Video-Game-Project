using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSpawnPosition", menuName = "ScriptableObjects/SpawnPosition", order = 1)]
public class PlayerSpawnPosition : ScriptableObject
{
    public Vector2 spawnPoint;

    public void Reset()
    {
        spawnPoint = new Vector2(-7.25f, -1.8f);
    }
}
