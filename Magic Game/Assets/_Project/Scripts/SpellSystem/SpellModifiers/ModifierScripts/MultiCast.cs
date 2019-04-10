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
        for (int i = 0; i < copyCount; i++)
        {
            Spell copy = Instantiate(spell, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(copy.GetComponent<MultiCast>());

            copy.transform.Rotate(Vector3.up * Random.Range(upDownRotation.x, upDownRotation.y));    // randomize left-right rotation
            copy.transform.Rotate(Vector3.right * Random.Range(leftRightRotation.x, leftRightRotation.y)); // randomize up-down rotation

            copy.caster = spell.caster;
        }
    }
}
