//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimationsCA : MonoBehaviour
{
    public bool bIkActive = true;

    [SerializeField] private EnemyCoreCA cEnemyCore = null;
    [SerializeField] private float animationSpeedMultiplier = 0.1f;
    [SerializeField] private float animationBlendingMultiplier = 0.0625f;
    [SerializeField] private float rotationOffset = 0.0f;
    
    private Animator cAnimator = null;

    void Start()
    {
        cAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (cAnimator.enabled)
        {
            float angle = transform.rotation.eulerAngles.y;
            float desiredAngle = 0.0f;

            if (cEnemyCore.cVision.bCanSeeTarget)
            {
                desiredAngle = Vector3.SignedAngle(Vector3.forward, cEnemyCore.cVision.targetLocation - transform.position, Vector3.up);
            }
            else
            {
                if (cEnemyCore.cNavigation.cAgent.velocity.magnitude > 1.0f)
                {
                    desiredAngle = Vector3.SignedAngle(Vector3.forward, cEnemyCore.cNavigation.cAgent.velocity, Vector3.up);
                }
                else
                {
                    desiredAngle = angle;
                }
            }

            desiredAngle += rotationOffset;

            Quaternion slerp = Quaternion.Slerp(Quaternion.Euler(0.0f, angle, 0.0f), Quaternion.Euler(0.0f, desiredAngle, 0.0f), Time.deltaTime * 2);

            transform.rotation = slerp;

            float sin = Mathf.Sin(angle * Mathf.Deg2Rad);
            float cos = Mathf.Cos(angle * Mathf.Deg2Rad);

            float nx = cEnemyCore.cNavigation.cAgent.velocity.x;
            float ny = cEnemyCore.cNavigation.cAgent.velocity.z;

            Vector2 velocityRotated = new Vector2(
                cos * nx - sin * ny,
                sin * nx + cos * ny
                );

            cAnimator.SetFloat("Movement Speed", (velocityRotated.magnitude) * animationSpeedMultiplier);
            cAnimator.SetFloat("Movement Forward", velocityRotated.y * animationBlendingMultiplier);
            cAnimator.SetFloat("Movement Right", velocityRotated.x * animationBlendingMultiplier);
        }
    }

    void OnAnimatorIK()
    {
        if (cAnimator.enabled)
        {
            if (bIkActive && cEnemyCore != null)
            {
                if (cEnemyCore.cVision.bCanSeeTarget)
                {
                    cAnimator.SetLookAtWeight(1.0f);
                    if (cEnemyCore.cVision.targetGO != null)
                    {
                        cAnimator.SetLookAtPosition(cEnemyCore.cVision.targetGO.transform.position + Vector3.up * cEnemyCore.cVision.HeightOffset);
                    }
                    else
                    {
                        cAnimator.SetLookAtPosition(cEnemyCore.cVision.targetLocation);
                    }
                }
                else
                {
                    if (cEnemyCore.currentState == EnemyCoreCA.EState.SEARCH && cEnemyCore.cVision.targetLocation != Vector3.zero)
                    {
                        if (Vector3.Distance(transform.position, cEnemyCore.cVision.targetLocation) > 2.0f)
                        {
                            cAnimator.SetLookAtWeight(1.0f);
                            cAnimator.SetLookAtPosition(cEnemyCore.cVision.targetLocation);

                        }
                        else
                        {
                            cAnimator.SetLookAtWeight(0.0f);
                        }
                    }
                    else
                    {
                        cAnimator.SetLookAtWeight(0.0f);
                    }
                }
            }
        }
    }
}
