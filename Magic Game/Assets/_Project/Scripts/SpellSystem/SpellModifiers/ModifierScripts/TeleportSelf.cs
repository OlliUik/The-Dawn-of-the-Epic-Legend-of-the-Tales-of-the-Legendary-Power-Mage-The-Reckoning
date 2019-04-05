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
            Instantiate(teleportParticles, movement.transform.position, Quaternion.identity);
            Instantiate(teleportParticles, collision.contacts[0].point + collision.contacts[0].normal, Quaternion.identity);
            movement.Teleport(collision.contacts[0].point + collision.contacts[0].normal);
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
            Instantiate(teleportParticles, movement.transform.position, Quaternion.identity);
            Instantiate(teleportParticles, hitInfo.point, Quaternion.identity);
            movement.Teleport(hitInfo.point + hitInfo.normal);
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

            // teleport to random position
            Instantiate(teleportParticles, transform.position, Quaternion.identity);
            Vector3 randomPos = Random.onUnitSphere * aoe.radius;
            randomPos.y = Mathf.Abs(randomPos.y);
            spell.caster.GetComponent<PlayerMovement>().Teleport(randomPos);

        }
    }


}
