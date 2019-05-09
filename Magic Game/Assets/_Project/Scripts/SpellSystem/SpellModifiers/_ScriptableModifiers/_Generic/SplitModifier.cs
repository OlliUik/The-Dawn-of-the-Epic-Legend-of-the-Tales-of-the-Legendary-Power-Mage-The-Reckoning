using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Split", menuName = "SpellSystem/Modifiers/Split")]
public class SplitModifier : SpellScriptableModifier
{

    [SerializeField] private int splitCount = 2;

    public override void AddSpellModifier(Spell spell)
    {
        var compo = spell.GetComponent<Split>();
        if (compo != null)
        {
            compo.splitCount += 2;
            return;
        }

        Split component = spell.gameObject.AddComponent<Split>();
        component.splitCount = splitCount;
    }
}
