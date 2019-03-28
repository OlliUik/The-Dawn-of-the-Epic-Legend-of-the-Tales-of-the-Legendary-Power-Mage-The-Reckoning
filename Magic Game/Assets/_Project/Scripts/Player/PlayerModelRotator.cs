//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class PlayerModelRotator : MonoBehaviour
{
    [SerializeField] private bool IkActive = true;
    [SerializeField] private Transform cameraLockTarget = null;
    [SerializeField] private Vector3 offset = Vector3.zero;
    [SerializeField] private PlayerSpellCaster caster = null;
    [SerializeField] private Transform headTransform = null;

    private Animator cAnimator = null;
    private Transform pointOfInterest = null;
    
    void Start()
    {
        cAnimator = GetComponent<Animator>();
        if (cameraLockTarget == null)
        {
            Debug.Log(this.gameObject + " has no camera lock target attached to it!");
            this.enabled = false;
        }
    }

    void Update()
    {
        Vector3 temp = cameraLockTarget.localRotation.eulerAngles;
        transform.localRotation = Quaternion.Euler(offset.x, offset.y + temp.y, offset.z);

        if (IkActive && headTransform != null)
        {
            RaycastHit hit;
            if (Physics.Raycast(
                    headTransform.position,
                    headTransform.forward,
                    out hit,
                    Mathf.Infinity,
                    1
                    ))
            {
                if (hit.transform.tag == "Enemy")
                {
                    pointOfInterest = hit.transform;
                }
            }

            if (pointOfInterest != null)
            {
                if (Physics.Raycast(
                    headTransform.position,
                    pointOfInterest.position - headTransform.position,
                    out hit,
                    Mathf.Infinity,
                    1
                    ))
                {
                    if (hit.transform != pointOfInterest)
                    {
                        pointOfInterest = null;
                        return;
                    }
                }
                else
                {
                    pointOfInterest = null;
                    return;
                }

                if (Vector3.Angle(headTransform.forward, pointOfInterest.position - transform.position) > 120.0f)
                {
                    pointOfInterest = null;
                }
            }
        }
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (IkActive && caster != null)
        {
            cAnimator.SetLookAtWeight(1.0f);

            if (pointOfInterest != null)
            {
                cAnimator.SetLookAtPosition(pointOfInterest.position + Vector3.up * 1.2f);
            }
            else
            {
                cAnimator.SetLookAtPosition(caster.castPoint);
            }
        }
        else
        {
            cAnimator.SetLookAtWeight(0.0f);
        }
    }
}
