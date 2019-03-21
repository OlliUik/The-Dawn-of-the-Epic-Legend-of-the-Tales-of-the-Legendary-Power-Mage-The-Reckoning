using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : ScriptableObject
{
    [Header("Card variables")]
    public List<CastRequirement> castRequirements   = new List<CastRequirement>();
    public List<SpellBalance> balances              = new List<SpellBalance>();
    public List<GameObject> spellModifiers          = new List<GameObject>();

    // here tell the spell what it should look like ?
    [Space(10)]
    public GameObject graphics                      = null;
}
