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

        Projectile proj = gameObject.GetComponent<Projectile>();

        var split = gameObject.GetComponent<Split>();
        if (split == null || splitted || !proj.isMaster) return;

        for (int i = 0; i < splitCount; i++)
        {
            Vector3 dir = collision.contacts[0].normal;
            dir = Quaternion.AngleAxis(UnityEngine.Random.Range(-70.0f, 70.0f) * (i + 1), dir) * dir;
            Quaternion rot = Quaternion.LookRotation(dir, Vector3.up);
            Vector3 rotatedDir = Vector3.zero;

            if (i < splitCount * 0.5)
            {
                rotatedDir = Quaternion.AngleAxis(-25f * (i + 1), direction) * dir;
            }
            else
            {
                rotatedDir = Quaternion.AngleAxis(25f * (i + 1), direction) * dir;
            }

            Projectile copy = Instantiate(gameObject, transform.position, rot).GetComponent<Projectile>();
            copy.name = "Split copy";
            copy.direction = rotatedDir;
            copy.caster = proj.caster;
            copy.isMaster = false;
            copy.statusEffects = proj.statusEffects;

            var homing = copy.GetComponent<Homing>();
            if (homing != null)
            {
                homing.Start();
                homing.target = homing.FindClosestTarget();
            }
        }

    }


    List<Beam> beams = new List<Beam>();

    public override void BeamCollide(RaycastHit hitInfo, Vector3 direction, float distance)
    {

        if (hitInfo.collider.GetComponent<Health>() != null) return;

        Beam beam = gameObject.GetComponent<Beam>();

        if(!beam.isMaster)
        {
            return;
        }

        if (beam.isMaster && !splitted)
        {
            for (int i = 0; i < splitCount; i++)
            {
                Beam copyBeam = Instantiate(beam);
                copyBeam.GetComponent<Split>().splitted = true;
                copyBeam.isMaster = false;
                copyBeam.name = "SplitCopy " + i;
                beams.Add(copyBeam);
                copyBeam.statusEffects = beam.statusEffects;
            }

            splitted = true;
        }

        for (int i = 0; i < beams.Count; i++)
        {
            beams[i].transform.position = hitInfo.point;
            beams[i].startPos = hitInfo.point;
            beams[i].Range = beam.Range - distance;
            beams[i].gameObject.SetActive(true);
            Vector3 reflectDir = Vector3.Reflect(direction, hitInfo.normal);
            Vector3 rotatedDir = Vector3.zero;

            // rotate half of the splitcount to left by the degree * i+1 and others right by degree * i+1
            if(i < splitCount * 0.5)
            {
                rotatedDir = Quaternion.AngleAxis(-25f * (i + 1), beam.direction) * reflectDir;   
            }
            else
            {
                rotatedDir = Quaternion.AngleAxis(25f * (i + 1), direction) * reflectDir;
            }

            beams[i].direction = rotatedDir;
            beams[i].UpdateBeam(beams[i].startPos, beams[i].direction);
        }

    }
    public override void BeamCollisionEnd()
    {
        for (int i = 0; i < beams.Count; i++)
        {
            beams[i].CollisionEnd();
            beams[i].gameObject.SetActive(false);
        }
    }
    public override void BeamCastingEnd()
    {
        if(beams.Count > 0)
        {
            foreach (Beam beam in beams)
            {
                beam.CastingEnd();
                Destroy(beam.gameObject);
            }
        }

        Destroy(gameObject);
    }

}
