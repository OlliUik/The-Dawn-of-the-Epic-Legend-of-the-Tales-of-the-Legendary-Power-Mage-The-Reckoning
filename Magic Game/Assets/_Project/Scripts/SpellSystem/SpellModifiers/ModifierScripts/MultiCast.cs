using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiCast : SpellModifier
{

    private bool ready                  = false;
    public int copyCount                = 2;
    public Vector2 upDownRotation       = Vector2.zero;
    public Vector2 leftRightRotation    = Vector2.zero;

    //private void Start()
    //{
    //    if (ready) return;

    //    for (int i = 0; i < projectileCount; i++)
    //    {
    //        GameObject copy = Instantiate(gameObject, gameObject.transform.position, gameObject.transform.rotation);
    //        copy.GetComponent<MultiProjectile>().ready = true;

    //        copy.transform.Rotate(Vector3.up * Random.Range(upDownRotation.x, upDownRotation.y));    // randomize left-right rotation
    //        copy.transform.Rotate(Vector3.right * Random.Range(leftRightRotation.x, leftRightRotation.y)); // randomize up-down rotation
    //    }
    //    ready = true;
    //}

    public override void OnSpellCast(Spell spell)
    {
        for (int i = 0; i < copyCount; i++)
        {
            Spell copy = Instantiate(spell, gameObject.transform.position, gameObject.transform.rotation);
            copy.GetComponent<MultiCast>().ready = true;

            copy.transform.Rotate(Vector3.up * Random.Range(upDownRotation.x, upDownRotation.y));    // randomize left-right rotation
            copy.transform.Rotate(Vector3.right * Random.Range(leftRightRotation.x, leftRightRotation.y)); // randomize up-down rotation

            copy.caster = spell.caster;
        }
    }
}
