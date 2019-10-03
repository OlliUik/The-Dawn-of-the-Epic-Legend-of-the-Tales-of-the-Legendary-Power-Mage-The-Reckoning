using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Launch", menuName = "SpellSystem/Modifiers/Launch")]
public class LaunchModifier : SpellScriptableModifier
{

    [SerializeField] private float aoeForce         = 10f;
    [SerializeField] private float beamForce        = 10f;
    [SerializeField] private float projectileForce  = 100f;


    public override void AddSpellModifier(Spell spell)
    {
        var component = spell.GetComponent<Launch>();
        if(component != null)
        {
            component.aoeForce          += aoeForce;
            component.beamForce         += beamForce;
            component.projectileForce   += projectileForce;
            return;
        }

        Launch launch                   = spell.gameObject.AddComponent<Launch>();
        launch.aoeForce                 = aoeForce;
        launch.beamForce                = beamForce;
        launch.projectileForce          = projectileForce;
    }

}
