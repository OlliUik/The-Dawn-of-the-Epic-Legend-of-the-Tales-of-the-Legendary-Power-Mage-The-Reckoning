using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levitation : SpellModifier
{

    public GameObject levitationObjectPrefab;
    public static GameObject levitatingObject;
    public static GameObject levitatingLineParticle;
    public static GameObject levitatingHoldParticle;

    public GameObject holdingParticle;
    public GameObject lineParticle;

    public float spellDuration = 3f;
    public float spellForceMultiplier = 1;

    GameObject caster;

    public override void OnSpellCast(Spell spell)
    {
        LevitationObject.spellDuration = spellDuration;
        LevitationObject.spellForceMultiplier = spellForceMultiplier;
        caster = spell.caster;
        Destroy(levitatingLineParticle);
        Destroy(levitatingObject);
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
        if(levitatingObject == null)
        {
            Debug.Log("Levitation called");
            levitatingObject = Instantiate(levitationObjectPrefab, collision.contacts[0].point, Quaternion.identity) as GameObject;
            levitatingObject.transform.SetParent(caster.transform.Find("Camera").transform);

            if (collision.gameObject.GetComponent<Rigidbody>() != null)
            {
                levitatingLineParticle = Instantiate(lineParticle, transform.position, Quaternion.identity) as GameObject;

                levitatingLineParticle.AddComponent<LevitationLineParticle>();
                levitatingLineParticle.GetComponent<LevitationLineParticle>().caster = caster;
            }

            LevitationObject.holdingParticlePrefab = holdingParticle;
        }
    }

}
