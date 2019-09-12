//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    public Transform targetTransform;

    [SerializeField] private RotateTowardsTarget rotationReference = null;

    void Start()
    {
        if (rotationReference != null)
        {
            targetTransform = rotationReference.targetTransform;
        }
    }

    void Update()
    {
        if (targetTransform != null)
        {
            transform.LookAt(targetTransform);
        }
        else
        {
            if (rotationReference != null)
            {
                targetTransform = rotationReference.targetTransform;
            }
        }
    }
}
