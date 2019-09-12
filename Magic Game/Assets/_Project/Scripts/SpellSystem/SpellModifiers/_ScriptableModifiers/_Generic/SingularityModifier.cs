using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Singularity", menuName = "SpellSystem/Modifiers/Singularity")]
public class SingularityModifier : SpellScriptableModifier
{

    public GameObject singularityPrefab = null;
    public BlackHoleVariables variables;

    public override void AddSpellModifier(Spell spell)
    {
        var compo = spell.GetComponent<Singularity>();
        if (compo != null)
        {
            // do what
            return;
        }

        Singularity component = spell.gameObject.AddComponent<Singularity>();
        component.singularityPrefab = singularityPrefab;
        component.variables = variables;
    }

}
