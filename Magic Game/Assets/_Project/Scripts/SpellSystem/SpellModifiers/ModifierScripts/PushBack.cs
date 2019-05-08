using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushBack : SpellModifier
{

    public float pushbackForce = 15.0f;
    private Spellbook spellbook;
    private Spell spell;

    private void Start()
    {
        GameObject player = FindObjectOfType<PlayerCore>().gameObject;
        spellbook = player.GetComponent<Spellbook>();
        spell = GetComponent<Spell>();

        if(spell.GetType() == typeof(Projectile))
        {
            for (int i = 0; i < 3; i++)
            {
                PushTargetBackwards();
            }
        }
    }

    void FixedUpdate()
    {        
        if(spell.spellType == SpellType.BEAM)
        {
            //Debug.DrawLine(cam.transform.position, cam.transform.forward); // fix this
            spellbook.transform.position += (spellbook.lookTransform.transform.TransformDirection(-Vector3.forward) * pushbackForce * Time.deltaTime);
        }
    }

    private void PushTargetBackwards()
    {
        spellbook.transform.Translate(-spellbook.lookTransform.forward * pushbackForce);
    }

}
