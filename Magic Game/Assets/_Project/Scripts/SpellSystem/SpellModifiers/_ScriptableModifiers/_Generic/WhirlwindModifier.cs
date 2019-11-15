using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Whirlwind", menuName = "SpellSystem/Modifiers/Whirlwind")]
public class WhirlwindModifier : SpellScriptableModifier
{

    public WhirlwindVariables variables;
    public GameObject tornadoPrefab;

    public override void AddSpellModifier(Spell spell)
    {
        var compo = spell.GetComponent<Whirlwind>();
        if (compo != null)
        {
            compo.variables.duration += variables.duration;
            compo.variables.strength += variables.strength;
            return;
        }
        
        Whirlwind component = spell.gameObject.AddComponent<Whirlwind>();
        component.tornadoPrefab = tornadoPrefab;
        component.variables = variables;
        component.SetElementParticles(projectileGraphics, beamGraphics, aoeGraphics);
    }
}
