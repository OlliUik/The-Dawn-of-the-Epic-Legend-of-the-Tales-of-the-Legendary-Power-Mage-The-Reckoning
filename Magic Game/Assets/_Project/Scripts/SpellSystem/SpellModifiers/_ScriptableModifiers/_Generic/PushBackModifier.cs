using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PushBack", menuName = "SpellSystem/Modifiers/PushBack")]
public class PushBackModifier : SpellScriptableModifier
{

    [SerializeField] private float aoeForce         = 10f;
    [SerializeField] private float beamForce        = 10f;
    [SerializeField] private float projectileForce  = 100f;

    public override void AddSpellModifier(Spell spell)
    {
        var compo = spell.GetComponent<PushBack>();
        if (compo != null)
        {
            compo.aoeForce += aoeForce;
            compo.beamForce += beamForce;
            compo.projectileForce += projectileForce;
            return;
        }

        PushBack comp = spell.gameObject.AddComponent<PushBack>();
        comp.aoeForce = aoeForce;
        comp.beamForce = beamForce;
        comp.projectileForce = projectileForce;
    }
}
