using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : OnCollision
{

    [SerializeField] private float knockbackForce = 10.0f;

    public override void Apply(GameObject go)
    {
        go.AddComponent<KnockBack>();
    }

    public override void Hit(GameObject go, Spellbook spellbook) // fix this and pushback
    {
        go.transform.position += (go.transform.position - spellbook.transform.position).normalized * knockbackForce * Time.deltaTime;
    }
}
