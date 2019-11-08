using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : SpellModifier
{

    public int bounceCount = 2;

    // BEAM variables
    GameObject beamCopy;
    Beam beam;


    /// <summary>
    /// If projectile has bounces left creates a new instance 
    /// of the projectile and rotates it to face direction of collision reflect
    /// </summary>
    public override void ProjectileCollide(Collision collision, Vector3 direction)
    {
        var bounce = GetComponent<Bounce>();
        if (bounce == null || bounceCount <= 0) return;
        
        Vector3 reflectionDir = Vector3.Reflect(direction, collision.contacts[0].normal);
        Quaternion rot = Quaternion.LookRotation(reflectionDir, Vector3.up);
        Projectile copy = Instantiate(gameObject, transform.position, rot).GetComponent<Projectile>();
        copy.name = "Bounce copy";
        copy.direction = reflectionDir;
        copy.GetComponent<Bounce>().bounceCount--;
        bounceCount = 0; //Remove bounces from the original projectile to avoid infinite loops.
        copy.isMaster = false;
        copy.statusEffects = gameObject.GetComponent<Spell>().statusEffects;

        var homing = copy.GetComponent<Homing>();
        if(homing != null)
        {
            homing.Start();
            homing.target = homing.FindClosestTarget();
        }
    }

    /// <summary>
    /// If beam hits a something and it has bounces left create new instance of the colliding beam
    /// and make it face the direction of collision reflect
    /// 
    /// Destroy the copy when beam is not hitting anything TODO:: optimize
    /// </summary>
    public override void BeamCollide(RaycastHit hitInfo, Vector3 direction, float distance)
    {

        if (hitInfo.collider.GetComponent<Health>() != null) return;

        if (bounceCount > 0)
        {
            if(beamCopy == null)
            {
                beamCopy = Instantiate(gameObject, transform.position, transform.rotation);
                beamCopy.name = "BounceCopy";
                beamCopy.GetComponent<Bounce>().bounceCount--;
                beam = beamCopy.GetComponent<Beam>();
                beam.isMaster = false;
                beam.statusEffects = gameObject.GetComponent<Spell>().statusEffects;
            }

            beam.Range = gameObject.GetComponent<Beam>().Range - distance;
            beamCopy.gameObject.SetActive(true);
            beamCopy.transform.position = hitInfo.point;
            beam.startPos = hitInfo.point;

            Vector3 reflectDir = Vector3.Reflect(direction, hitInfo.normal);
            beam.direction = reflectDir;
            beam.UpdateBeam(hitInfo.point, reflectDir);
        }
    }
    public override void BeamCollisionEnd()
    {
        if (beamCopy != null)
        {
            beam.CollisionEnd();
            Destroy(beamCopy);
        }
    }
    public override void BeamCastingEnd()
    {
        if(beamCopy != null)
        {
            beam.CastingEnd();
            Destroy(beamCopy);
        }
    }

}
