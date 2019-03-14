using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Card : ScriptableObject
{
    public List<CastRequirement> castRequirements = new List<CastRequirement>();
    public List<SpellBalance> balances = new List<SpellBalance>();
    public List<GameObject> spellModifiers = new List<GameObject>();
}
