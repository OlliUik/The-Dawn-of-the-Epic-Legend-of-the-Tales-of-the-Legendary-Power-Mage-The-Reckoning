using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SwitchPlaces", menuName = "SpellSystem/Modifiers/SwitchPlaces")]
public class SwitchPlacesModifier : SpellScriptableModifier
{

    [SerializeField] private GameObject teleportParticles = null;

    public override void AddSpellModifier(Spell spell)
    {
        var compo = spell.GetComponent<SwitchPlaces>();
        if (compo != null)
        {
            // do what
            return;
        }

        SwitchPlaces sp = spell.gameObject.AddComponent<SwitchPlaces>();
        sp.teleportParticles = teleportParticles;
    }
}
