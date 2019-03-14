using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModelRotator : MonoBehaviour
{
    [SerializeField] private Transform cameraLockTarget = null;
    [SerializeField] private Vector3 offset = Vector3.zero;

    void Start()
    {
        if (cameraLockTarget == null)
        {
            Debug.Log(this.gameObject + " has no camera lock target attached to it!");
            this.enabled = false;
        }
    }

    void Update()
    {
        Vector3 temp = cameraLockTarget.localRotation.eulerAngles;
        transform.localRotation = Quaternion.Euler(offset.x, offset.y, offset.z + temp.y);
    }
}
