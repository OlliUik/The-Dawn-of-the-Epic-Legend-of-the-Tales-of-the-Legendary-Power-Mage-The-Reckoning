using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Whirlwind", menuName = "SpellSystem/Modifiers/Whirlwind")]
public class WhirlwindModifier : SpellScriptableModifier
{

    public WhirlwindVariables variables;
    public GameObject tornadoPrefab;

    public override void AddSpellModifier(GameObject spellObject)
    {
        var compo = spellObject.GetComponent<Whirlwind>();
        if (compo != null)
        {
            compo.variables.duration += variables.duration;
            compo.variables.strength += variables.strength;
            return;
        }

        Whirlwind component = spellObject.AddComponent<Whirlwind>();
        component.tornadoPrefab = tornadoPrefab;
        component.variables = variables;
    }
}
