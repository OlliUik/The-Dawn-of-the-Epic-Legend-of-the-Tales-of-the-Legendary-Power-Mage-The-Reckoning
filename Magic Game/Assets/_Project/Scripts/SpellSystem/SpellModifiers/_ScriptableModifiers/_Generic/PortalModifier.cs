using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Portal", menuName = "SpellSystem/Modifiers/Portal")]
public class PortalModifier : SpellScriptableModifier
{

    public GameObject portalGatePrefab;
    public int increasingPortal = 1;

    public override void AddSpellModifier(Spell spell)
    {
        var compo = spell.GetComponent<Portal>();
        if (compo != null)
        {
            PortalGateManager.Instance.maximumPortals += increasingPortal;
            return;
        }

        Portal component = spell.gameObject.AddComponent<Portal>();
        component.portalGatePrefab = portalGatePrefab;
    }

}
