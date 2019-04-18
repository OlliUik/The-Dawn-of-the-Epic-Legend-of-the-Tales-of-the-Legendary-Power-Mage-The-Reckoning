using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Split : SpellModifier
{

    public bool splitted = false;
    public int splitCount = 2;


    public override void ProjectileCollide(Collision collision, Vector3 direction)
    {
        
        if(!splitted)
        {
            for (int i = 0; i < splitCount; i++)
            {
                GameObject copy = Instantiate(gameObject, transform.position, Quaternion.identity);
                copy.transform.rotation = Quaternion.FromToRotation(copy.transform.forward, collision.contacts[0].normal);  // also rotate the whole thing for graphics to face the right direction
                copy.GetComponent<Projectile>().direction = collision.contacts[0].normal;                                   // this changes the direction the projectile is moving
                Destroy(copy.GetComponent<Split>());

                copy.transform.Rotate(copy.transform.right, UnityEngine.Random.Range(-45f, 45f));
                copy.transform.Rotate(copy.transform.up, UnityEngine.Random.Range(-45f, 45f));

                Projectile copyProj = copy.GetComponent<Projectile>();
                copyProj.caster = gameObject.GetComponent<Projectile>().caster;
            }
        }

    }


    List<Beam> beams = new List<Beam>();


    public override void BeamCollide(RaycastHit hitInfo, Vector3 direction, float distance)
    {

        Beam beam = GetComponent<Beam>();

        if (beam.isMaster && !splitted)
        {
            for (int i = 0; i < splitCount; i++)
            {
                Beam copyBeam = Instantiate(beam);
                copyBeam.GetComponent<Split>().splitted = true;
                copyBeam.isMaster = false;
                copyBeam.name = "SplitCopy " + i;
                beams.Add(copyBeam);
            }

            splitted = true;
        }

        for (int i = 0; i < beams.Count; i++)
        {
            beams[i].Range = distance;
            beams[i].transform.position = hitInfo.point;
            beams[i].gameObject.SetActive(true);
            beams[i].startPos = hitInfo.point;
            Vector3 reflectDir = Vector3.Reflect(direction, hitInfo.normal);
            Vector3 rotatedVector = Quaternion.AngleAxis(i == 1 ? 45f : -45f, direction) * reflectDir;

            beams[i].direction = rotatedVector;
            beams[i].UpdateBeam(beams[i].startPos, beams[i].direction);
        }

    }

    public override void BeamCollisionEnd()
    {
        for (int i = 0; i < beams.Count; i++)
        {
            beams[i].gameObject.SetActive(false);
        }
    }

}
