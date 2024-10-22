﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchPlaces : SpellModifier
{

    public GameObject teleportParticles;

    public override void ProjectileCollide(Collision collision, Vector3 direction)
    {

        Spell spell = gameObject.GetComponent<Spell>();
        PlayerMovement movement = spell.caster.GetComponent<PlayerMovement>();

        if (movement != null)
        {   
            if(collision.gameObject.GetComponent<Rigidbody>() != null)
            {
                Vector3 playersPosition = (movement.transform.position + (Vector3.up * 1));
                Instantiate(teleportParticles, playersPosition, Quaternion.identity);
                
                movement.Teleport(collision.contacts[0].point);
                Instantiate(teleportParticles, collision.contacts[0].point, Quaternion.identity);

                collision.gameObject.transform.position = playersPosition;
                return;
            }

            print("HitObject doesn't have rigidbody");
        }
        else
        {
            print("Error: missing PlayerMovement component");
        }
    }

    public override void AoeCollide(GameObject hitObject)
    {
        Debug.Log("SwitchPlaces is not compatible with aoe");
    }

    public override void BeamCollide(RaycastHit hitInfo, Vector3 direction, float distance)
    {

        var rb = hitInfo.collider.GetComponent<Rigidbody>();

        if(rb != null)
        {
            Spell spell = gameObject.GetComponent<Spell>();
            Vector3 casterPosition = (spell.caster.transform.position + (Vector3.up * 1));

            if(spell.caster.CompareTag("Player"))
            {
                spell.caster.GetComponent<PlayerMovement>().Teleport((hitInfo.point + hitInfo.normal));
                Instantiate(teleportParticles, spell.caster.transform.position, Quaternion.identity);
                hitInfo.transform.position = casterPosition + Vector3.up; // player pitvot is at feet
                Instantiate(teleportParticles, casterPosition, Quaternion.identity);
            }
            else
            {
                spell.caster.transform.position = (hitInfo.point + hitInfo.normal);
                Instantiate(teleportParticles, spell.caster.transform.position, Quaternion.identity);
                hitInfo.transform.position = casterPosition;
                Instantiate(teleportParticles, casterPosition, Quaternion.identity);
            }
        }
    }

}
