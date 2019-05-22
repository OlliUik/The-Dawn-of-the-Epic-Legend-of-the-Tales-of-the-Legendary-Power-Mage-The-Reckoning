using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    CapsuleCollider myCollider = new CapsuleCollider();

    public Transform target;

    RaycastHit hitInfo;

    public LayerMask ground;

    Vector3 currentNormal = Vector3.up;
    Vector3 forward;
    Vector3 surfaceNormal = Vector3.down;
    Vector3 colPoint;
    Vector3 normalizedDirection;
    Vector3 targetPosition = Vector3.zero;

    EnemyPath path;
 
    const float minPathUpdateTime = .2f;
    const float pathUpdateMoveTreshold = 0.5f;

    public float speed = 20;
    public float fallSpeed = 20;
    public float turnSpeed = 5;
    public float turnDistance = 5;
    public float stoppingDistance = 10;
    public float height = 1f;
    public float heightPadding = 0.1f;
    public float maxGroundAngle = 145;
    public float attackRange = 2;

    float normStoppingDist;
    float tempAttackRange;
    float groundAngle;
    float angle;
    float radius;

    bool bGrounded;
    bool bMovingToPlayer = false;


    private void Start()
    {
        myCollider = GetComponent<CapsuleCollider>();
        radius = myCollider.radius * 0.9f;

        tempAttackRange = attackRange;
        normStoppingDist = stoppingDistance;
    }

    //private void Update()
    //{
        //DrawDebugLines();
        //CalculateForward();
        //CheckGround();
        //ApplyGravity();

        //if(bMovingToPlayer)
        //{
        //    StartCoroutine(FollowPath());
        //}

        //if (bGrounded /*&& Vector3.Distance(transform.position, target.position) < 20 ||bMovingToPlayer*/)
        //{
        //    if (!bMovingToPlayer)
        //    {
        //        bMovingToPlayer = true;
        //    }
        //    StartCoroutine(UpdatePath());
        //}

    //}

    //private void CheckGround()
    //{
    //    if (Physics.Raycast(transform.position, Vector3.down, out hitInfo, height + heightPadding, ground))
    //    {
    //        transform.up = hitInfo.normal;
    //        bGrounded = true;
    //    }
    //    else
    //    {
    //        bGrounded = false;
    //    }
    //}

    //private void ApplyGravity()
    //{
    //    if (!bGrounded)
    //    {
    //        transform.position += Physics.gravity * Time.deltaTime;
    //    }
    //}

    //private void CalculateForward()
    //{
    //    // if not grounded forward is transform.forward else change according to groundAngle
    //    if (!bGrounded)
    //    {
    //        forward = transform.forward;
    //        currentNormal = Vector3.up;
    //        return;
    //    }

    //    forward = Vector3.Cross(transform.right, hitInfo.normal);
    //}

    private void OnPathFound(Vector3[] waypoints, bool bPathSuccessful)
    {
        if (bPathSuccessful)
        {
            path = new EnemyPath(waypoints, transform.position, turnDistance, stoppingDistance);
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    public IEnumerator UpdatePath()
    {
        if (Time.timeSinceLevelLoad < .3f)
        {
            yield return new WaitForSeconds(0.3f);
        }

        Debug.Log("target's position is: " + target.position);

        //Request path from current position to target        
        PathRequestManager.RequestPath(new PathRequest(transform.position, target.position, OnPathFound));

        float sqrMoveTreshhold = pathUpdateMoveTreshold * pathUpdateMoveTreshold;
        Vector3 targetPosOld = target.position;

        while (true)
        {
            yield return new WaitForSeconds(minPathUpdateTime);

            if ((target.position - targetPosOld).sqrMagnitude > sqrMoveTreshhold)
            {
                PathRequestManager.RequestPath(new PathRequest(transform.position, target.position, OnPathFound));
                targetPosOld = target.position;
            }
        }
    }

    IEnumerator FollowPath()
    {
        bool bFollowingPath = true;
        int pathIndex = 0;
        transform.LookAt(path.lookPoints[0]);

        float speedPercent = 1;

        while (bFollowingPath)
        {
            Vector2 position2d = new Vector2(transform.position.x, transform.position.z);

            while (path.turnBoundaries[pathIndex].HasCrossedLine(position2d))
            {
                if (pathIndex == path.finishedLineIndex)
                {
                    bFollowingPath = false;
                    break;
                }
                else
                {
                    pathIndex++;
                }
            }

            //move along the path
            if (bFollowingPath)
            {
                if (!bGrounded)
                {
                    bFollowingPath = false;
                    bMovingToPlayer = false;
                }

                // start slowing if next point is target's position
                if (pathIndex >= path.slowDownIndex && stoppingDistance > 0)
                {
                    speedPercent = Mathf.Clamp01(path.turnBoundaries[path.finishedLineIndex].DistanceFromThePoint(position2d) / stoppingDistance);

                    if (speedPercent < 0.01f || Physics.Raycast(transform.position, target.position, attackRange))
                    {
                        bFollowingPath = false;

                        if (bMovingToPlayer)
                        {                            
                            bMovingToPlayer = false;
                        }
                    }
                }

                var wantedPosY = transform.position.y;

                // Rotate towards next pathPoint
                Quaternion targetRotation = Quaternion.LookRotation(path.lookPoints[pathIndex] - transform.position);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);

                Vector3 targetPos = new Vector3(path.lookPoints[pathIndex].x, wantedPosY, path.lookPoints[pathIndex].z);
                transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * speed * speedPercent);

            }

            yield return null;
        }
    }

    public void SetTargetPosition(Vector3 targetPos)
    {
        targetPosition = targetPos;
    }

    private void DrawDebugLines()
    {
        Debug.DrawLine(transform.position, transform.position + forward * height * 2, Color.blue);
        Debug.DrawLine(transform.position, transform.position - Vector3.up * height, Color.green);
    }
}
