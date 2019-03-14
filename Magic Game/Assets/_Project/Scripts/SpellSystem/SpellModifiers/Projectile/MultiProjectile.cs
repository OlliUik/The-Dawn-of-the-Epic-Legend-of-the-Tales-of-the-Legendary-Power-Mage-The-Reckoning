using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiProjectile : OnCast
{

    private bool ready = false;

    public override void Apply(GameObject go)
    {
        go.AddComponent<MultiProjectile>();
    }

    private void Start()
    {
        if (ready) return;

        print("Multi projectile");

        // create two instances of the current projectile
        GameObject copy1 = Instantiate(gameObject, gameObject.transform.position, gameObject.transform.rotation);
        GameObject copy2 = Instantiate(gameObject, gameObject.transform.position, gameObject.transform.rotation);

        // rotate direction 45 deg and other -45 deg etc.
        copy1.transform.Rotate(Vector3.up * 45f);
        copy2.transform.Rotate(Vector3.up * -45f);

        copy1.GetComponent<MultiProjectile>().ready = true;
        copy2.GetComponent<MultiProjectile>().ready = true;
        ready = true;
    }
}
