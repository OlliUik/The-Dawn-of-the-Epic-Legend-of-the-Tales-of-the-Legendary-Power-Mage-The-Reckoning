using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TransformTo", menuName = "SpellSystem/Modifiers/TransformTo")]
public class TransformToModifier : SpellScriptableModifier
{

    [SerializeField] private GameObject transformToPrefab = null;
    [SerializeField] private float duration;

    public override void AddSpellModifier(Spell spell)
    {
        var comp = spell.GetComponent<TransformTo>();
        if(comp != null)
        {
            comp.duration += this.duration;
            return;
        }

        var compo = spell.gameObject.AddComponent<TransformTo>();
        compo.transformPrefab = transformToPrefab;
        compo.duration = duration;
    }
}
