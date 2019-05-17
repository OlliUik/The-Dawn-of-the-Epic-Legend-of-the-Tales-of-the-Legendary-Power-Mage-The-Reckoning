using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whirlwind : SpellModifier
{

    public GameObject tornadoPrefab;
    public WhirlwindVariables variables;


    public override void OnSpellCast(Spell spell)
    {
        if(spell.spellType == SpellType.AOE)
        {
            GameObject tornado = Instantiate(tornadoPrefab, spell.caster.transform.position, spell.caster.transform.rotation);
            tornado.transform.SetParent(spell.caster.transform);
            Tornado script = tornado.GetComponentInChildren<Tornado>();
            script.variables = variables;
        }
    }

    //private void Update()
    //{
    //    var beam = gameObject.GetComponent<Beam>();
    //    if (beam != null)
    //    {
    //        RaycastHit[] hitObject = Physics.SphereCastAll(beam.startPos, variables.size, beam.direction);

    //        foreach (RaycastHit hit in hitObject)
    //        {
    //            var rb = hit.collider.GetComponent<Rigidbody>();
    //            if (rb != null)
    //            {

    //                float s1 = -beam.endPos.y + beam.startPos.y;
    //                float s2 = beam.endPos.x - beam.startPos.x;
    //                var direction = Mathf.Abs((hit.transform.position.x - beam.startPos.x) * s1 + (hit.transform.position.y - beam.startPos.y) * s2) / Mathf.Sqrt(s1 * s1 + s2 * s2);

    //                // pull towards the center
    //                Debug.Log("beam pulling " + hit.collider.gameObject.name);
    //                Vector3 difference = (transform.position - rb.transform.position).normalized;
    //                rb.AddForce(difference * variables.pullInSpeed);
    //            }
    //        }
    //    }
    //}

    public override void ProjectileCollide(Collision collision, Vector3 direction)
    {
        GameObject tornado = Instantiate(tornadoPrefab, collision.contacts[0].point, Quaternion.identity);
        Tornado script = tornado.GetComponentInChildren<Tornado>();
        script.variables = variables;
    }
}
