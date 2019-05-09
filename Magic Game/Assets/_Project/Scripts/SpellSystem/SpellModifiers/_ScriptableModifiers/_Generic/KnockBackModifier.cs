using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KnockBack", menuName = "SpellSystem/Modifiers/KnockBack")]
public class KnockBackModifier : SpellScriptableModifier
{

    [SerializeField] private float aoeForce = 10f;
    [SerializeField] private float beamForce = 10f;
    [SerializeField] private float projectileForce = 100f;

    public override void AddSpellModifier(Spell spell)
    {
        var compo = spell.GetComponent<KnockBack>();
        if (compo != null)
        {
            compo.aoeForce += aoeForce;
            compo.beamForce += beamForce;
            compo.projectileForce += projectileForce;
            return;
        }

        KnockBack component = spell.gameObject.AddComponent<KnockBack>();
        component.aoeForce = aoeForce;
        component.beamForce = beamForce;
        component.projectileForce = projectileForce;
    }
}
