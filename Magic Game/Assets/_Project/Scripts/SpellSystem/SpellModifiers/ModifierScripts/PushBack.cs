using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushBack : SpellModifier
{

    public float aoeForce;
    public float beamForce;
    public float projectileForce;

    private Spellbook spellbook;
    private Spell spell;

    private void Start() // REDO THIS SCRIPT
    {
        spell = GetComponent<Spell>();
        spellbook = spell.caster.GetComponent<Spellbook>();

        if(spell.spellType == SpellType.PROJECTILE)
        {
            // apply pushback
            for (int i = 0; i < 80; i++)
            {
                PushTargetBackwards(projectileForce);
            }
        }
    }

    void FixedUpdate()
    {        
        if(spell.spellType == SpellType.BEAM)
        {
            PushTargetBackwards(beamForce * Time.deltaTime);
        }
    }

    private void PushTargetBackwards(float force) // doesn't work so well with projectile yet plz fix
    {
        Vector3 direction = spellbook.GetDirection();
        direction *= -1;
        spellbook.GetComponent<PlayerCore>().cMovement.Move(direction * force * Time.deltaTime);
    }

}
