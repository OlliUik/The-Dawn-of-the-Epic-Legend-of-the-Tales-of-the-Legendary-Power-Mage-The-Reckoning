using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportSelf : SpellModifier
{

    public GameObject teleportParticles;

    public override void ProjectileCollide(Collision collision, Vector3 direction)
    {

        Spell spell = gameObject.GetComponent<Spell>();
        PlayerMovement movement = spell.caster.GetComponent<PlayerMovement>();

        if(movement != null)
        {
            Vector3 teleportPos = (collision.contacts[0].point + collision.contacts[0].normal);
            Teleport(teleportPos, spell.caster);
        }
        else
        {
            print("caster not found");
        }
       
    }

    public override void BeamCollide(RaycastHit hitInfo, Vector3 direction)
    {

        Spell spell = gameObject.GetComponent<Spell>();
        PlayerMovement movement = spell.caster.GetComponent<PlayerMovement>();

        if (movement != null)
        {
            Vector3 teleportPos = (hitInfo.point + hitInfo.normal);
            Teleport(teleportPos, spell.caster);
        }
        else
        {
            print("caster not found");
        }

    }

    public override void OnSpellCast(Spell spell)
    {
        if(spell.GetType() == typeof(Aoe))
        {

            Aoe aoe = spell.GetComponent<Aoe>();

            // get random position
            Vector3 randomPos = Random.onUnitSphere * aoe.radius;
            randomPos.y = Mathf.Abs(randomPos.y);

            Teleport(randomPos, aoe.caster);

        }
    }

    private void Teleport(Vector3 teleportPosition, GameObject caster)
    {
        caster.GetComponent<PlayerMovement>().Teleport(teleportPosition);
        Instantiate(teleportParticles, teleportPosition, teleportParticles.transform.rotation);
    }

}
