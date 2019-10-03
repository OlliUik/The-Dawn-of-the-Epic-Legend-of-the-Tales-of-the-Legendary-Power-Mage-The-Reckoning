using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levitation : SpellModifier
{

    public GameObject levitationObjectPrefab;
    public GameObject levitatingObject;

    GameObject caster;

    public override void OnSpellCast(Spell spell)
    {
        caster = spell.caster;
        /*
        if(spell.spellType == SpellType.AOE)
        {
            GameObject portalgate = Instantiate(portalGatePrefab, spell.caster.transform.position, spell.caster.transform.rotation);
            PortalGateManager.Instance.CreatePortalGate(portalgate);
        }
        */
    }

    public override void ProjectileCollide(Collision collision, Vector3 direction)
    {
        Debug.Log("Levitation called");
        levitatingObject = Instantiate(levitationObjectPrefab, collision.contacts[0].point, Quaternion.identity);
        levitatingObject.transform.SetParent(caster.transform.Find("Camera").transform);
    }

}
