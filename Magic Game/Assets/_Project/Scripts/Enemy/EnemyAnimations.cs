//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimations : MonoBehaviour
{
    public bool bIkActive = true;

    [SerializeField] private EnemyCore cEnemyCore = null;
    [SerializeField] private float animationSpeedMultiplier = 0.1f;
    [SerializeField] private float animationBlendingMultiplier = 0.0625f;

    private Animator cAnimator = null;

    void Start()
    {
        cAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        float angle = transform.rotation.eulerAngles.y;
        float desiredAngle = 0.0f;

        if (cEnemyCore.vision.bCanSeeTarget)
        {
            desiredAngle = Vector3.SignedAngle(Vector3.forward, cEnemyCore.vision.targetLocation - transform.position, Vector3.up);
        }
        else
        {
            if (cEnemyCore.navigation.agent.velocity.magnitude > 1.0f)
            {
                desiredAngle = Vector3.SignedAngle(Vector3.forward, cEnemyCore.navigation.agent.velocity, Vector3.up);
            }
            else
            {
                desiredAngle = angle;
            }
        }

        Quaternion slerp = Quaternion.Slerp(Quaternion.Euler(0.0f, angle, 0.0f), Quaternion.Euler(0.0f, desiredAngle, 0.0f), Time.deltaTime * 2);

        transform.rotation = slerp;

        float sin = Mathf.Sin(angle * Mathf.Deg2Rad);
        float cos = Mathf.Cos(angle * Mathf.Deg2Rad);

        float nx = cEnemyCore.navigation.agent.velocity.x;
        float ny = cEnemyCore.navigation.agent.velocity.z;

        Vector2 velocityRotated = new Vector2(
            cos * nx - sin * ny,
            sin * nx + cos * ny
            );

        cAnimator.SetFloat("Movement Speed", (velocityRotated.magnitude) * animationSpeedMultiplier);
        cAnimator.SetFloat("Movement Forward", velocityRotated.y * animationBlendingMultiplier);
        cAnimator.SetFloat("Movement Right", velocityRotated.x * animationBlendingMultiplier);
    }

    void OnAnimatorIK()
    {
        if (bIkActive && cEnemyCore != null)
        {
            if (cEnemyCore.vision.bCanSeeTarget)
            {
                cAnimator.SetLookAtWeight(1.0f);
                cAnimator.SetLookAtPosition(cEnemyCore.vision.targetLocation + Vector3.up * 0.8f);
            }
            else
            {
                if (cEnemyCore.currentState == EnemyCore.EState.SEARCH && cEnemyCore.vision.targetLocation != Vector3.zero)
                {
                    if (Vector3.Distance(transform.position, cEnemyCore.vision.targetLocation) > 2.0f)
                    {
                        cAnimator.SetLookAtWeight(1.0f);
                        cAnimator.SetLookAtPosition(cEnemyCore.vision.targetLocation + Vector3.up * 0.3f);

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
