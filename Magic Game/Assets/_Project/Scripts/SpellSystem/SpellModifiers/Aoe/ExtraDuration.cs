using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraDuration : OnCast
{

    [SerializeField] float extraDuration = 5f;

    public override void Apply(GameObject go)
    {
        go.GetComponent<Aoe>().AddDuration(extraDuration);
    }
}
