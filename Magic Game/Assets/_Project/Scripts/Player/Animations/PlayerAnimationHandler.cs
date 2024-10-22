﻿//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationHandler : MonoBehaviour
{
    #region VARIABLES

    public bool bRotateTowardsMoveDir = false;
    private bool rotateTemp = false;

    [SerializeField] private bool IkActive = true;
    [SerializeField] private LayerMask pointOfInterestMask = 0;
    [SerializeField] private Transform headTransform = null;
    [SerializeField] private Vector3 modelRotationOffset = Vector3.zero;
    [SerializeField] private float animationSpeedMultiplier = 5.0f;
    [SerializeField] private float animationBlendingMultiplier = 4.0f;

    private CharacterController cCharacter = null;
    private PlayerMovement cMovement = null;
    private ThirdPersonCamera cTPCamera = null;
    private Animator cAnimator = null;
    private Transform pointOfInterest = null;

    #endregion

    #region UNITY_DEFAULT_METHODS

    void Awake()
    {
        cAnimator = GetComponent<Animator>();
        GameObject parentGO = transform.parent.gameObject;
        cCharacter = parentGO.GetComponent<CharacterController>();
        cMovement = parentGO.GetComponent<PlayerMovement>();
        cTPCamera = parentGO.GetComponent<ThirdPersonCamera>();
        rotateTemp = bRotateTowardsMoveDir;
    }

    void Update()
    {
        #region ANIMATIONS_MOVEMENT

        Vector3 lookDirection = cTPCamera.lookDirection;
        Vector3 lookVector = new Vector3(Mathf.Sin(lookDirection.y * Mathf.Deg2Rad), 0.0f, Mathf.Cos(lookDirection.y * Mathf.Deg2Rad));

        float angleBetween = Vector3.SignedAngle(Vector3.forward, lookVector, Vector3.up);
        float sin = Mathf.Sin(angleBetween * Mathf.Deg2Rad);
        float cos = Mathf.Cos(angleBetween * Mathf.Deg2Rad);

        float nx = cCharacter.velocity.x * Time.fixedDeltaTime - cMovement.movingPlatformVelocity.x;
        float ny = cCharacter.velocity.z * Time.fixedDeltaTime - cMovement.movingPlatformVelocity.z;

        Vector2 velocityRotated = new Vector2(
            cos * nx - sin * ny,
            sin * nx + cos * ny
            );

        cAnimator.SetFloat("Movement Speed", (velocityRotated.magnitude) * animationSpeedMultiplier);
        cAnimator.SetFloat("Movement Forward", velocityRotated.y * animationBlendingMultiplier);
        cAnimator.SetFloat("Movement Right", velocityRotated.x * animationBlendingMultiplier);
        
        if (!cCharacter.isGrounded)
        {
            if (cMovement.bIsWallSliding)
            {
                cAnimator.SetFloat("Movement Right", cMovement.wallRightSide ? 1.0f : 0.0f);
                cAnimator.SetBool("Is Jumping", false);
                cAnimator.SetBool("Is Wall Running", true);
                bRotateTowardsMoveDir = true;
            }
            else
            {
                cAnimator.SetBool("Is Jumping", true);
                cAnimator.SetBool("Is Wall Running", false);
                bRotateTowardsMoveDir = rotateTemp;
            }
        }
        else
        {
            cAnimator.SetBool("Is Jumping", false);
            cAnimator.SetBool("Is Wall Running", false);
            bRotateTowardsMoveDir = rotateTemp;
        }

        #endregion

        #region ANIMATIONS_MODEL_ROTATION

        if (!bRotateTowardsMoveDir)
        {
            Vector3 cameraRotation = cTPCamera.cameraObject.transform.localRotation.eulerAngles;
            transform.localRotation = Quaternion.Euler(modelRotationOffset.x, modelRotationOffset.y + cameraRotation.y, modelRotationOffset.z);
        }
        else
        {
            float rotation = Vector3.SignedAngle(Vector3.forward, new Vector3(cMovement.moveVector.x, 0.0f, cMovement.moveVector.z).normalized, Vector3.up);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(modelRotationOffset.x, modelRotationOffset.y + rotation, modelRotationOffset.z), 15.0f * Time.deltaTime);
        }
        
        #endregion

        #region ANIMATIONS_POINT_OF_INTEREST

        if (IkActive && headTransform != null)
        {
            RaycastHit hit;
            if (Physics.Raycast(
                    cTPCamera.cameraComponent.transform.position,
                    cTPCamera.cameraComponent.transform.forward,
                    out hit,
                    Mathf.Infinity,
                    pointOfInterestMask
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
                    pointOfInterestMask
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

        #endregion
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (IkActive)
        {
            if (pointOfInterest != null)
            {
                cAnimator.SetLookAtPosition(pointOfInterest.position + Vector3.up * 1.2f);
                cAnimator.SetLookAtWeight(1.0f);
            }
            else
            {
                cAnimator.SetLookAtPosition(headTransform.position + cTPCamera.cameraComponent.transform.forward);
                cAnimator.SetLookAtWeight(1.0f);
            }
        }
        else
        {
            cAnimator.SetLookAtWeight(0.0f);
        }
    }

    #endregion

    #region CUSTOM_METHODS

    public void CastSpell(int spellType)
    {
        cAnimator.SetInteger("Spell Type", spellType);
        cAnimator.SetTrigger("Cast Spell");
        cAnimator.SetTrigger("Release Hold");
    }

    public void TakeDamage()
    {
        cAnimator.SetTrigger("Take Damage");
    }

    #endregion
}
