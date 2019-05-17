//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsTarget : MonoBehaviour
{
    public Transform targetTransform;

    [SerializeField] private Transform headTransform;
    [SerializeField] private Vector3 offset = new Vector3(-90.0f, 90.0f, 90.0f);
    [SerializeField] private float rotationSpeed = 10.0f;
    [SerializeField] private Vector2[] rotationLimit = new Vector2[3];

    private Vector3 originalRotation = Vector3.zero;
    private Quaternion previousRotation = Quaternion.identity;
    private Quaternion offsetQuaternion = Quaternion.identity;

    void OnValidate()
    {
        if (rotationLimit.Length != 3)
        {
            Vector2[] temp = rotationLimit;
            rotationLimit = new Vector2[3];
            for (int i = 0; i < rotationLimit.Length; i++)
            {
                if (temp.Length > i)
                {
                    rotationLimit[i] = temp[i];
                }
            }
        }
    }

    void Start()
    {
        if (headTransform != null)
        {
            originalRotation = headTransform.localRotation.eulerAngles;
            offsetQuaternion = Quaternion.Euler(offset);
            return;
        }

        Debug.LogError(this.gameObject + " has no head transform attached to " + this);
        this.enabled = false;
    }

    void LateUpdate()
    {
        if (targetTransform != null)
        {
            headTransform.LookAt(targetTransform, Vector3.up);
            headTransform.localRotation *= offsetQuaternion;
            headTransform.localRotation = Quaternion.Lerp(previousRotation, headTransform.localRotation, Time.deltaTime * rotationSpeed);
            previousRotation = headTransform.localRotation;
        }
        else
        {
            headTransform.localRotation = Quaternion.Euler(originalRotation);
        }
    }
}
