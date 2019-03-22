using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushBack : SpellModifier
{

    public float pushbackForce = 15.0f;
    private Spellbook spellbook;
    private Camera cam;

    public override void Apply(GameObject go)
    {
        go.AddComponent<PushBack>();
    }

    private void Start()
    {
        cam = Camera.main;
        spellbook = GetComponentInParent<Spellbook>();
    }

    void FixedUpdate()
    {
        Debug.DrawLine(cam.transform.position, cam.transform.forward); // fix this
        spellbook.transform.position += (cam.transform.TransformDirection(-Vector3.forward) * pushbackForce * Time.deltaTime);
    }
}
