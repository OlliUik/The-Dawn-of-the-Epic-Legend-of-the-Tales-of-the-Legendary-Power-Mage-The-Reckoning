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
        float angle = 0.0f;

        if (cEnemyCore.vision.bCanSeeTarget)
        {
            angle = Vector3.SignedAngle(Vector3.forward, cEnemyCore.vision.targetLocation - transform.position, Vector3.up);
        }
        else
        {
            angle = Vector3.SignedAngle(Vector3.forward, cEnemyCore.navigation.agent.velocity, Vector3.up);
        }

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

        transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
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
                cAnimator.SetLookAtWeight(0.0f);
            }
        }
    }
}
