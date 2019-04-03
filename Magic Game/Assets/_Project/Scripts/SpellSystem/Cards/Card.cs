using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Card : ScriptableObject
{
    [Header("Card variables")]
    public List<SpellType> types                        = new List<SpellType>();
    public string cardName                              = null;

    [TextArea]
    public string cardDescription                       = null;

    // Important 3 ... modifiers are spell spesific (projectile, beam, etc.)
    public List<CastRequirement> castRequirements       = new List<CastRequirement>();
    public List<SpellBalance> balances                  = new List<SpellBalance>();
    public List<SpellScriptableModifier> modifiers      = new List<SpellScriptableModifier>();

    // StatusEffects, CastRequirements and balances are generic for all spells
    public List<ScriptableEffect> effects               = new List<ScriptableEffect>();
    public List<StatusEffectBase> statusEffects         = new List<StatusEffectBase>();

    [Space(10)]
    public GameObject graphics                          = null;
    public GameObject secendaryGraphics                 = null;
}
