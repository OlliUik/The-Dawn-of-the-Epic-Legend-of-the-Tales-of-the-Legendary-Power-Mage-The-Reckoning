using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Portal", menuName = "SpellSystem/Modifiers/Portal")]
public class PortalModifier : SpellScriptableModifier
{

    public GameObject portalGatePrefab;
    public GameObject portalActiveParticle;
    public int increasingPortal = 1;
    public float increasingDuration = 5;

    public override void AddSpellModifier(Spell spell)
    {
        var compo = spell.GetComponent<Portal>();
        if (compo != null)
        {
            compo.maximumPortals += increasingPortal;
            compo.duration += increasingDuration;
            //PortalGateManager.Instance.maximumPortals += increasingPortal;
            Debug.Log("Maximum portal " + PortalGateManager.Instance.maximumPortals);
            return;
        }

        Portal component = spell.gameObject.AddComponent<Portal>();
        component.portalGatePrefab = portalGatePrefab;
        component.SetElementParticles(projectileGraphics, Beam.ElementType.Default, aoeGraphics);
        PortalGateManager.Instance.portalActiveParticle = portalActiveParticle;
    }

}
