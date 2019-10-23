//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private GameObject playerModel = null;
    [SerializeField] private float animationSpeedMultiplier = 0.1f;
    [SerializeField] private float animationBlendingMultiplier = 0.0625f;

    private Animator cAnimator = null;
    private PowerMage.ThirdPersonCamera cTPCamera = null;
    private CharacterController cCharacter = null;
    private PlayerMovement cMovement = null;


    void Start()
    {
        cCharacter = GetComponent<PlayerCore>().cCharacter;
        cTPCamera = GetComponent<PlayerCore>().cTPCamera;
        cMovement = GetComponent<PlayerCore>().cMovement;

        if (playerModel != null)
        {
            cAnimator = playerModel.GetComponent<Animator>();
        }
    }

    public void CastSpell(int spellType)
    {
        cAnimator.SetInteger("Spell Type", spellType);
        cAnimator.SetTrigger("Cast Spell");
    }

    public void TakeDamage()
    {
        cAnimator.SetTrigger("Take Damage");
    }

    void Update()
    {
        Vector3 lookDirection = cTPCamera.lookDirection;

        //Get the desired movement unit vector based on where the player is looking at
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

        cAnimator.SetBool("Is Jumping", !cCharacter.isGrounded);
    }
}
