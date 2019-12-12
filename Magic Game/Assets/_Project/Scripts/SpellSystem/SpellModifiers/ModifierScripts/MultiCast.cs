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

        if(spell.spellType == SpellType.GENERIC || spell.spellType == SpellType.PROJECTILE)
        {
            for (int i = 0; i < copyCount; i++)
            {
                Projectile copyFrom = (Projectile)spell;
                Projectile copy = Instantiate(copyFrom, gameObject.transform.position, gameObject.transform.rotation);
                copy.name = "MultiCast copy " + i.ToString();
                copy.direction = copyFrom.direction;

                copy.direction = Quaternion.Euler(Random.Range(upDownRotation.x, upDownRotation.y), Random.Range(leftRightRotation.x, leftRightRotation.y), 0) * copy.direction;
                copy.caster = spell.caster;
                copy.statusEffects = copyFrom.statusEffects;
            }

            return;
        }

        if(spell.spellType == SpellType.BEAM)
        {
            for (int i = 0; i < copyCount; i++)
            {
                Beam copy = Instantiate(gameObject, transform.position, transform.rotation).GetComponent<Beam>();
                copy.name = "MultiCast copy " + i.ToString();
                copy.isMaster = true;               
                copy.statusEffects = gameObject.GetComponent<Spell>().statusEffects;

                if(i < copyCount * 0.5)
                {
                    copy.angle = -15f * (i+1);
                }
                else
                {
                    copy.angle = 15f * i;
                }
            }

            return;
        }
    }

}
