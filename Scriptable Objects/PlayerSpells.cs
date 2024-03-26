using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSpells", menuName = "ScriptableObjects/PlayerSpells", order = 1)]
public class PlayerSpells : ScriptableObject
{
    public List<Item> spellBooks = new List<Item>();
    public List<Sprite> spellUseSprites = new List<Sprite>();
    public List<string> spellNames = new List<string>();

    public void Reset()
    {
        spellBooks = new List<Item>();
        spellUseSprites = new List<Sprite>();
        spellNames = new List<string>();
    }
}
