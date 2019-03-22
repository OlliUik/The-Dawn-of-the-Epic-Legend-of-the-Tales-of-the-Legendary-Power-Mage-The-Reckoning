using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Card : ScriptableObject
{
    [Header("Card variables")]
    public List<CastRequirement> castRequirements       = new List<CastRequirement>();
    public List<SpellBalance> balances                  = new List<SpellBalance>();
    public List<SpellScriptableModifier> modifiers      = new List<SpellScriptableModifier>();

    [Space(10)]
    public GameObject graphics                          = null;
    public GameObject secendaryGraphics                 = null;
}
