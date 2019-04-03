using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportSelf : OnCollision
{

    public override void OnCollide(Collision collision, Vector3 direction)
    {

        Spell spell = gameObject.GetComponent<Spell>();
        GameObject caster = spell.caster;

        caster.transform.position = collision.contacts[0].normal; // player movement somehow stops this??

    }

}
