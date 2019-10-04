using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Levitation", menuName = "SpellSystem/Modifiers/Levitation")]
public class LevitationModifier : SpellScriptableModifier
{

    public GameObject levitationObjectPrefab;
    public GameObject holdingParticle;
    public GameObject lineParticle;

    public float forceMultiplierAddition = 0.5f;
    public float durationAddition = 3f;

    public override void AddSpellModifier(Spell spell)
    {
        var compo = spell.GetComponent<Levitation>();
        if (compo != null)
        {
            compo.spellDuration += durationAddition;
            compo.spellForceMultiplier += forceMultiplierAddition;
            return;
        }

        Levitation component = spell.gameObject.AddComponent<Levitation>();
        component.levitationObjectPrefab = levitationObjectPrefab;
        component.holdingParticle = holdingParticle;
        component.lineParticle = lineParticle;
    }

}
