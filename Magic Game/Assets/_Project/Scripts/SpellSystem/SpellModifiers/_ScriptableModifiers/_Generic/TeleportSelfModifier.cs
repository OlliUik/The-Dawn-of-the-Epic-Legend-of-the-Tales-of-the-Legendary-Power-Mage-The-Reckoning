using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TeleportSelf", menuName = "SpellSystem/Modifiers/TeleportSelf")]
public class TeleportSelfModifier : SpellScriptableModifier
{

    [SerializeField] private GameObject teleportParticles;

    public override void AddSpellModifier(Spell spell)
    {
        var compo = spell.GetComponent<TeleportSelf>();
        if (compo != null)
        {
            // do what
            return;
        }

        TeleportSelf tp = spell.gameObject.AddComponent<TeleportSelf>();
        tp.teleportParticles = teleportParticles;
    }
}
