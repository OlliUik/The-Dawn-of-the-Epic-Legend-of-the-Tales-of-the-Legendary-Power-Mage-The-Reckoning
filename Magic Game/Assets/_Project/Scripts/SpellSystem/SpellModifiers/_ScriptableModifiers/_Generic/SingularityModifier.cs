using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Singularity", menuName = "SpellSystem/Modifiers/Singularity")]
public class SingularityModifier : SpellScriptableModifier
{

    public GameObject singularityPrefab = null;
    public BlackHoleVariables variables;

    public override void AddSpellModifier(GameObject spellObject)
    {
        var compo = spellObject.GetComponent<Singularity>();
        if (compo != null)
        {
            // do what
            return;
        }

        Singularity component = spellObject.AddComponent<Singularity>();
        component.singularityPrefab = singularityPrefab;
        component.variables = variables;
    }

}
