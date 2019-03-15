using UnityEngine;

[RequireComponent(typeof(PlayerCore))]
public class PlayerMovement : MonoBehaviour
{
    #region VARIABLES

    [HideInInspector] public float accelerationMultiplier = 1.0f;
    [HideInInspector] public int midAirJumps            = 0;

    [SerializeField] private float acceleration         = 100.0f;
    [SerializeField] private float airAcceleration      = 20.0f;
    [SerializeField] private float friction             = 5.5f;
    [SerializeField] private float airFriction          = 1.5f;
    [SerializeField] private float gravity              = -30.0f;
    [SerializeField] private float smoothStepDown       = 0.5f;
    [SerializeField] private float jumpForce            = 15.0f;
    [SerializeField] private float jumpGraceTime        = 0.2f;
    [SerializeField] private float dashSpeed            = 20.0f;
    [SerializeField] private float dashJumpForce        = 8.0f;
    [SerializeField] private float dashDuration         = 0.2f;
    [SerializeField] private float dashCooldown         = 1.0f;
    [SerializeField] private float gravityWallSliding   = -1.0f;
    [SerializeField] private float wallSlidingTime      = 2.0f;

    private ThirdPersonCamera cTPCamera                 = null;
    private CharacterController cCharacter              = null;
    private LayerMask physicsLayerMask                  = 1;

    //Input
    private Vector2 movementInput = Vector2.zero;
    private bool bJumpingActivated = false;
    private bool bDashingActivated = false;
    
    //Temporary values
    private float dt                                    = 0.0f;
    private bool bIsWallSliding                         = false;
    private Vector3 moveDirection                       = Vector3.zero;
    private Vector3 moveVector                          = Vector3.zero;
    private Vector3 slopeNormal                         = Vector3.zero;
    private float jgtTimer                              = 0.0f;
    private float dDurationTimer                        = 0.0f;
    private float dCooldownTimer                        = 0.0f;
    private float wstTimer                              = 0.0f;
    private int midAirJumpsLeft                         = 0;
    private Transform movingPlatform                    = null;
    private Vector3 movingPlatformPrevPosition          = Vector3.zero;
    private Vector3 movingPlatformPrevRotation          = Vector3.zero;
    private Vector3 movingPlatformVelocity              = Vector3.zero;

    #endregion

    #region UNITY_DEFAULT_METHODS

    void Start()
    {
        cCharacter       = GetComponent<PlayerCore>().cCharacter;
        cTPCamera        = GetComponent<PlayerCore>().cTPCamera;
        physicsLayerMask = GetComponent<PlayerCore>().physicsLayerMask;
    }

    void FixedUpdate()
    {
        Move(movementInput.x, movementInput.y, bJumpingActivated, bDashingActivated);
        bJumpingActivated = false;
        bDashingActivated = false;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        RaycastHit rcHit;
        if (Physics.Raycast(
            transform.position + Vector3.up * (cCharacter.height / 2),
            Vector3.down,
            out rcHit,
            Mathf.Infinity,
            physicsLayerMask
            ))
        {
            if (AlmostEqual(hit.normal, rcHit.normal, 0.01f))
            {
                slopeNormal = hit.normal;
            }
            else
            {
                //Most likely standing on stairs
                slopeNormal = Vector3.up;
            }
        }
        else
        {
            //We hit a collider but received nothing from raycast,
            //assume that we hit a wall / ceiling / other
            slopeNormal = Vector3.up;
        }

        if (hit.gameObject.tag == "MovingPlatform")
        {
            if (movingPlatform == null)
            {
                movingPlatform = hit.transform;
            }
            else if (hit.transform != movingPlatform)
            {
                //Uh oh, we're hitting multiple moving platforms at the same time!
                //Reset platform movement values to avoid warping.

                //NOTE: This solution is not perfect, warping still occurs during unknown edge cases.
                //If possible, avoid using multiple moving platforms close to each other!
                movingPlatform = null;
                movingPlatformPrevPosition = Vector3.zero;
                movingPlatformPrevRotation = Vector3.zero;
                movingPlatformVelocity = Vector3.zero;
            }
        }
        else
        {
            movingPlatform = null;
        }

        Vector2 temp = Vector2.Perpendicular(new Vector2(hit.normal.x, hit.normal.z));
        Vector3 temp2 = Vector3.Normalize(Vector3.Cross(hit.normal, new Vector3(temp.x, 0.0f, temp.y)));
        //Slope normal vector
        Debug.DrawLine(hit.point, hit.point + hit.normal * 0.2f, (Mathf.Abs(Vector3.Angle(Vector3.up, hit.normal)) < cCharacter.slopeLimit ? Color.green : Color.red), 0.5f);
        //Vector pointing down the slope
        Debug.DrawLine(hit.point, hit.point + temp2 * 0.2f, Color.blue, 0.5f);
    }

    #endregion

    #region CUSTOM_METHODS

    public void GetInput(float inputX, float inputY, bool inputJump, bool inputDash)
    {
        movementInput = new Vector2(inputX, inputY);
        if (inputJump && !bJumpingActivated)
        {
            bJumpingActivated = true;
        }
        if (inputDash && !bDashingActivated)
        {
            bDashingActivated = true;
        }
    }

    void Move(float inputX, float inputY, bool inputJump, bool inputDash)
    {
        dt = Time.fixedDeltaTime;

        CalculateMovingPlatform();
        CalculateCooldowns();

        Vector3 lookDirection = cTPCamera.lookDirection;

        float moveSpeed = Mathf.Abs(inputX) + Mathf.Abs(inputY);
        if (moveSpeed >= 1.0f)
        {
            moveSpeed = 1.0f;
        }

        bool isGrounded = cCharacter.isGrounded;
        if ((cCharacter.collisionFlags & CollisionFlags.Below) == 0)
        {
            //slopeNormal = Vector3.up;
            movingPlatform = null;

            if ((cCharacter.collisionFlags & CollisionFlags.Sides) > 0 && wstTimer > 0.0f)
            {
                bIsWallSliding = true;
            }
            else
            {
                bIsWallSliding = false;
            }
        }
        else
        {
            bIsWallSliding = false;
            wstTimer = wallSlidingTime;
            midAirJumpsLeft = midAirJumps;
        }

        //Get the desired movement unit vector based on where the player is looking at
        Vector3 lookVector = new Vector3(Mathf.Sin(lookDirection.y * Mathf.Deg2Rad), 0.0f, Mathf.Cos(lookDirection.y * Mathf.Deg2Rad));
        Vector3 sideLookVector = Vector3.Cross(lookVector, Vector3.down);
        moveDirection = Vector3.Normalize(lookVector * inputY + sideLookVector * inputX);

        //Get a vector pointing downwards a slope
        Vector2 slopeNormalPerpendicular = Vector2.Perpendicular(new Vector2(slopeNormal.x, slopeNormal.z));
        Vector3 slopeDownDirection = Vector3.Normalize(Vector3.Cross(slopeNormal, new Vector3(slopeNormalPerpendicular.x, 0.0f, slopeNormalPerpendicular.y)));
        
        //Allow normal movement if not on a slope
        if (Vector3.Angle(Vector3.up, slopeNormal) < cCharacter.slopeLimit)
        {
            /*----------------------------------------------------------------------------------------*/
            //Calculate movement on XZ plane
            Vector3 tempVector = moveVector;
            tempVector.y = 0.0f;

            if (dDurationTimer <= 0.0f)
            {
                tempVector += isGrounded ?
                    moveDirection * moveSpeed * acceleration * accelerationMultiplier * dt
                    : moveDirection * moveSpeed * airAcceleration * accelerationMultiplier * dt;
            }
            /*----------------------------------------------------------------------------------------*/
            //Calculate friction
            if (dDurationTimer <= 0.0f)
            {
                tempVector -= isGrounded ?
                    tempVector * friction * dt
                    : tempVector * airFriction * dt;
            }
            /*----------------------------------------------------------------------------------------*/
            //Calculate movement in Y direction
            if (isGrounded)
            {
                Debug.DrawLine(
                    transform.position + Vector3.up * 0.01f + Vector3.Normalize(tempVector) * cCharacter.radius,
                    transform.position + Vector3.down * (0.01f + smoothStepDown) + Vector3.Normalize(tempVector) * cCharacter.radius,
                    Color.cyan
                );

                RaycastHit hit;
                if (Physics.Raycast(
                    transform.position + Vector3.up * 0.01f + Vector3.Normalize(tempVector) * cCharacter.radius,
                    Vector3.down,
                    out hit,
                    cCharacter.skinWidth + smoothStepDown,
                    physicsLayerMask
                    ))
                {
                    if (hit.transform != movingPlatform)
                    {
                        tempVector.y = -cCharacter.slopeLimit;
                    }
                }
            }

            float tempGravity = gravity;
            if (bIsWallSliding && moveVector.y < 0.0f && wstTimer > 0.0f)
            {
                tempGravity = gravityWallSliding;
                wstTimer -= dt;
            }

            tempVector.y = isGrounded ?
                tempVector.y + gravity * dt
                : moveVector.y + tempGravity * dt;
            /*----------------------------------------------------------------------------------------*/
            //Dashing
            if (inputDash && dCooldownTimer <= 0.0f && dDurationTimer <= 0.0f)
            {
                if (moveSpeed < 0.1f)
                {
                    moveDirection = -lookVector;
                }
                GetComponent<Health>().AddInvulnerability(dashDuration);
                dDurationTimer = dashDuration;
                dCooldownTimer = dashCooldown;
                tempVector = moveDirection * dashSpeed * accelerationMultiplier;
                tempVector.y = dashJumpForce;
            }
            /*----------------------------------------------------------------------------------------*/
            //Jumping
            if (inputJump)
            {
                //Jumping (normal)
                if (jgtTimer > 0.0f || midAirJumpsLeft > 0)
                {
                    jgtTimer = 0.0f;
                    tempVector.y = jumpForce;
                    if (midAirJumpsLeft > 0)
                    {
                        midAirJumpsLeft--;
                    }
                }

                //Jumping (wallsliding)
                if (bIsWallSliding)
                {
                    jgtTimer = 0.0f;
                    tempVector.y = jumpForce;
                    if (midAirJumpsLeft > 0)
                    {
                        midAirJumpsLeft--;
                    }
                    else
                    {
                        wstTimer = 0.0f;
                    }
                }
            }
            /*----------------------------------------------------------------------------------------*/
            //Move temporary values back to final variable
            moveVector = tempVector;
        }
        //Do something else when on a steep slope
        else
        {
            Vector3 tempVector = moveDirection * airAcceleration * dt;
            tempVector = Vector3.ProjectOnPlane(tempVector, slopeNormal);
            moveVector = Vector3.ProjectOnPlane(moveVector, slopeNormal);
            moveVector += tempVector + slopeDownDirection * -gravity * dt;

            RaycastHit hit;
            //if (!Physics.Raycast(
            //    transform.position + slopeNormal + moveVector * dt,
            //    -slopeNormal,
            //    out hit,
            //    1.0f + 0.5f,
            //    physicsLayerMask
            //    ))
            if (!Physics.Raycast(
                transform.position,
                -slopeNormal,
                out hit,
                cCharacter.skinWidth + 0.1f,
                physicsLayerMask
                ))
            {
                slopeNormal = Vector3.up;
            }
            else
            {
                Debug.DrawLine(hit.point, hit.point + hit.normal * 5.0f, Color.yellow);
            }
        }

        //Stop vertical movement if hitting a ceiling
        if ((cCharacter.collisionFlags & CollisionFlags.Above) != 0 && moveVector.y > 0.0f)
        {
            moveVector.y = 0.0f;
        }

        //Debug stuff
        Debug.DrawLine(transform.position, transform.position + lookVector, Color.blue);    //Forward vector
        Debug.DrawLine(transform.position, transform.position + sideLookVector, Color.red); //Right vector
        Debug.DrawLine(transform.position, transform.position + Vector3.up, Color.green);   //Up vector
        Debug.DrawLine(transform.position + Vector3.up, transform.position + Vector3.up + moveDirection, Color.cyan); //Desired movement unit vector
        Debug.DrawLine(transform.position + Vector3.up, transform.position + Vector3.up + moveVector.normalized, Color.magenta); //Desired movement unit vector

        //Make an ugly fix, because character controller doesn't detect collisions
        //if it's moving AWAY from it (e.g. moving platform going up)
        Vector3 finalMovement = moveVector * dt + movingPlatformVelocity;
        if (movingPlatform != null && finalMovement.y > 0.0f)
        {
            cCharacter.Move(finalMovement + Vector3.up * 0.5f);
            cCharacter.Move(Vector3.down * 0.5f);
        }
        else
        {
            cCharacter.Move(finalMovement);
        }
    }

    void CalculateMovingPlatform()
    {
        if (movingPlatform != null)
        {
            //Independent from external scripts
            if (movingPlatformPrevPosition != Vector3.zero)
            {
                movingPlatformVelocity = movingPlatform.position - movingPlatformPrevPosition;
                Vector3 dir = transform.position - movingPlatform.position;
                dir = Quaternion.Euler(movingPlatform.rotation.eulerAngles - movingPlatformPrevRotation) * dir;
                movingPlatformVelocity -= transform.position - (dir + movingPlatform.position);
            }
            movingPlatformPrevPosition = movingPlatform.position;
            movingPlatformPrevRotation = movingPlatform.rotation.eulerAngles;
            
            //Get values from the platform's script

            //Move position based on platform's position delta
            //movingPlatformVelocity = movingPlatform.GetComponent<MovingPlatform>().positionDelta;

            //Rotate position around platform's pivot point by platform's rotation delta
            //Vector3 dir = transform.position - movingPlatform.position;
            //dir = Quaternion.Euler(movingPlatform.GetComponent<MovingPlatform>().rotationDelta) * dir;
            //movingPlatformVelocity -= transform.position - (dir + movingPlatform.position);
        }
        else
        {
            movingPlatformPrevPosition = Vector3.zero;
            movingPlatformPrevRotation = Vector3.zero;
            movingPlatformVelocity = Vector3.zero;
        }
    }

    void CalculateCooldowns()
    {
        jgtTimer -= jgtTimer > 0.0f ? dt : 0.0f;
        dDurationTimer -= dDurationTimer > 0.0f ? dt : 0.0f;
        dCooldownTimer -= (dDurationTimer <= 0.0f && dCooldownTimer > 0.0f) ? dt : 0.0f;

        if (cCharacter.isGrounded)
        {
            jgtTimer = jumpGraceTime;
        }

        //if (jgtTimer > 0.0f)
        //{
        //    jgtTimer -= dt;
        //}

        //if (dDurationTimer > 0.0f)
        //{
        //    dDurationTimer -= dt;
        //}
        //else
        //{
        //    if (dCooldownTimer > 0.0f)
        //    {
        //        dCooldownTimer -= dt;
        //    }
        //}
    }

    bool AlmostEqual(Vector3 v1, Vector3 v2, float precision)
    {
        bool equal = true;
        if (Mathf.Abs(Vector3.Angle(v1, v2)) > precision)
        {
            equal = false;
        }
        return equal;
    }

    #endregion
}
