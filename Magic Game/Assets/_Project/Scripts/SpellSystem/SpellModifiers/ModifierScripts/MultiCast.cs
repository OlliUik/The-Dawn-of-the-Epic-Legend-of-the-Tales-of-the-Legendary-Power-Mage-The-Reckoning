using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiCast : SpellModifier
{

    public int copyCount                = 2;
    public Vector2 upDownRotation       = Vector2.zero;
    public Vector2 leftRightRotation    = Vector2.zero;


    public override void OnSpellCast(Spell spell)
    {

        if(spell.GetType() == typeof(Projectile))
        {
            for (int i = 0; i < copyCount; i++)
            {
                Spell copy = Instantiate(spell, gameObject.transform.position, gameObject.transform.rotation);
                Destroy(copy.GetComponent<MultiCast>());
            
                copy.transform.Rotate(Vector3.up * Random.Range(upDownRotation.x, upDownRotation.y));    // randomize left-right rotation
                copy.transform.Rotate(Vector3.right * Random.Range(leftRightRotation.x, leftRightRotation.y)); // randomize up-down rotation
            
                copy.caster = spell.caster;
            }
        }

        if(spell.GetType() == typeof(Beam))
        {
            for (int i = 0; i < copyCount; i++)
            {
                GameObject copy = Instantiate(gameObject);
                copy.name = "MultiCast copy " + i.ToString();
                Beam beam = copy.GetComponent<Beam>();
                beam.isMaster = true;
                
                if(i == 0)
                {
                    beam.angle = -15f;
                }
                else
                {
                    beam.angle = 15f;
                }
            }
        }
    }

    //public override void BeamCollide(RaycastHit hitInfo, Vector3 direction)
    //{
        
    //    if(!copiesCreated)
    //    {
    //        for (int i = 0; i < copyCount; i++)
    //        {
    //            GameObject copy = Instantiate(gameObject);
    //            copy.name = "MultiCast copy " + i.ToString();
    //            Beam beam = copy.GetComponent<Beam>();
    //            beam.isMaster = false;
    //            beam.startPos = hitInfo.point;
    //            beams.Add(beam);
    //            copiesCreated = true;
    //        }
    //    }

    //    for (int i = 0; i < beams.Count; i++)
    //    {
    //        beams[i].gameObject.SetActive(true);

    //        Vector3 reflectDir = Vector3.Reflect(beams[i].direction, hitInfo.normal);
    //        if(i == 0)
    //        {
    //            reflectDir = Quaternion.Euler(0, -25f, 0) * reflectDir;
    //        }
    //        else
    //        {
    //            reflectDir = Quaternion.Euler(0, 25f, 0) * reflectDir;
    //        }

    //        beams[i].direction = reflectDir;
    //    }
    //}

    //public override void BeamCollisionEnd()
    //{
    //    for (int i = 0; i < beams.Count; i++)
    //    {
    //        beams[i].gameObject.SetActive(false);
    //    }
    //}


}
