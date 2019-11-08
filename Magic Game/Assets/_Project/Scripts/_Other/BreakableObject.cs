using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BreakableObject : MonoBehaviour
{
    public GameObject destroyedObject = null;

    private bool isQuitting = false;

    private void OnApplicationQuit()
    {
        isQuitting = true;
    }

    protected virtual void OnDestroy()
    {
        if (isQuitting)
        {
            return;
        }
        
        if (destroyedObject != null)
        {
            Instantiate(destroyedObject, transform.position, transform.rotation, null);
        }
    }
}
