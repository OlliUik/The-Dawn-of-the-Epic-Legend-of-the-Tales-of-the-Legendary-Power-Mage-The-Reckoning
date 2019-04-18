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
                Beam copy = Instantiate(gameObject, transform.position, transform.rotation).GetComponent<Beam>();
                copy.name = "MultiCast copy " + i.ToString();
                copy.isMaster = true;               

                if(i < copyCount * 0.5)
                {
                    copy.angle = -15f * (i+1);
                }
                else
                {
                    copy.angle = 15f * i;
                }
            }
        }
    }

}
