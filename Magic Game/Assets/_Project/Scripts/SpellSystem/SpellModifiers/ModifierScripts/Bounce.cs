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
        if(bounceCount > 0)
        {
            GameObject copy = Instantiate(gameObject, transform.position, Quaternion.identity);
            Vector3 reflectionDir = Vector3.Reflect(direction, collision.contacts[0].normal);
            copy.transform.rotation = Quaternion.FromToRotation(copy.transform.forward, reflectionDir);  // also rotate the whole thing for graphics to face the right direction
            copy.GetComponent<Projectile>().direction = reflectionDir;                                   // this changes the direction the projectile is moving
            copy.GetComponent<Bounce>().bounceCount--;
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
        if (bounceCount > 0)
        {
            if(beamCopy == null)
            {
                beamCopy = Instantiate(gameObject, transform.position, transform.rotation);
                beamCopy.name = "BounceCopy";
                beamCopy.GetComponent<Bounce>().bounceCount--;
                beam = beamCopy.GetComponent<Beam>();
                beam.isMaster = false;
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
