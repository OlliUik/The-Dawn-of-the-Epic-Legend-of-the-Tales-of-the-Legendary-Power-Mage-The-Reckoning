//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class LookAtTargetParticleLine : MonoBehaviour
{
    public Transform targetTransform;
    public Vector3 offset;
    public float sizeOffset = 35;

    [SerializeField] public RotateTowardsTarget rotationReference = null;

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
            if(offset != null)
            {
                transform.LookAt(targetTransform.position + offset);
            }
            else {
                transform.LookAt(targetTransform.position);
            }
            transform.localScale = new Vector3(1, 1, Vector3.Distance(transform.position, targetTransform.position) / sizeOffset );
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
