using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Portal", menuName = "SpellSystem/Modifiers/Portal")]
public class PortalModifier : SpellScriptableModifier
{

    public GameObject portalGatePrefab;
    public GameObject portalActiveParticle;
    public int increasingPortal = 1;

    public override void AddSpellModifier(Spell spell)
    {
        var compo = spell.GetComponent<Portal>();
        if (compo != null)
        {
            compo.maximumPortals += increasingPortal;
            //PortalGateManager.Instance.maximumPortals += increasingPortal;
            Debug.Log("Maximum portal " + PortalGateManager.Instance.maximumPortals);
            return;
        }

        Portal component = spell.gameObject.AddComponent<Portal>();
        component.portalGatePrefab = portalGatePrefab;
        PortalGateManager.Instance.portalActiveParticle = portalActiveParticle;
    }

}
