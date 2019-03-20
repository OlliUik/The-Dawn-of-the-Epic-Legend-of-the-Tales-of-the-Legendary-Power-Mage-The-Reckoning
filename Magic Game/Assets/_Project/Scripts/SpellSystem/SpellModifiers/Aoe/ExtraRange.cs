using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraRange : OnCast
{

    [SerializeField] private float extraRange = 5f;

    public override void Apply(GameObject go)
    {
        go.GetComponent<Aoe>().AddRange(extraRange);
    }
}
