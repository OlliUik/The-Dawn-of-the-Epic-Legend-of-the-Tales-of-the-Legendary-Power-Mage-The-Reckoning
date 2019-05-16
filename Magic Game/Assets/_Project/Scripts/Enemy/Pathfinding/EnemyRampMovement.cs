using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRampMovement : MonoBehaviour
{
    GameObject[] rampWaypoints;
    GameObject floor;
    GameObject currentRampPoint;

    public Transform target;

    RaycastHit hitDown;
    RaycastHit hitInfo;

    private Vector3 targetPosition;
    private Vector3 normalizedDirection;
    private Vector3 oldYPos;

    public float height = 1f;
    public float heightPadding = 0.1f;
    public float speed = 5;

    private float closestDistToEnemy;
    private float closestDistToPlayer;

    private string floorName;

    public LayerMask ground;

    private bool bGrounded;
    public bool bArrivedToRamp = false;

    //private void Start()
    //{
    //    rampWaypoints = GameObject.FindGameObjectsWithTag("Waypoint");
    //}

    public void MoveOnRamp(Transform enemyUnit)
    {
        if (!bArrivedToRamp)
        {
            FindClosestRamp(enemyUnit);
        }

        //if (bArrivedToRamp)
        //{
        //    Vector3 newYPos = new Vector3(enemyUnit.position.x, hitInfo.point.y + height, enemyUnit.position.z);
        //    oldYPos = newYPos;
        //    enemyUnit.position = newYPos;
        //}

        if (Vector3.Distance(enemyUnit.position, targetPosition) <= 1f)
        {
            FindNextRampPoint();

            if (!bArrivedToRamp)
            {
                bArrivedToRamp = true;
            }
            else
            {
                bArrivedToRamp = false;
            }
        }



        //if(bArrivedToRamp)
        //{
        //    while (Mathf.Abs(enemyUnit.position.y - target.position.y) <= 1f)
        //    {
        //        Vector3 newYPos = new Vector3(enemyUnit.position.x, hitInfo.point.y + height, enemyUnit.position.z);
        //        oldYPos = newYPos;
        //        enemyUnit.position = newYPos;

        //        normalizedDirection = (targetPosition - enemyUnit.position).normalized;
        //        enemyUnit.position += normalizedDirection * speed * Time.deltaTime;
        //    }

        if (Mathf.Abs(enemyUnit.position.y - target.position.y) <= 1f)
        {
            bArrivedToRamp = false;
        }

    }

    public bool OnRamp(Transform enemyTransform)
    {
        if (!bGrounded)
        {
            return false;
        }

        RaycastHit hit;

        if (Physics.Raycast(enemyTransform.position, Vector3.down, out hit, height + heightPadding))
        {
            if (hit.normal != Vector3.up)
            {
                return true;
            }
        }

        return false;
    }

    public Vector3 FindClosestRamp(Transform enemyUnit)
    {
        //bMovingToRamp = true;

        Transform closest = enemyUnit;

        float distance = Mathf.Infinity;

        var dir = target.position - enemyUnit.position;
        closestDistToEnemy = dir.magnitude;
        closestDistToPlayer = closestDistToEnemy;

        //go through each ramp to find closest one to both enemy and player
        for (int i = 0; i < rampWaypoints.Length; i++)
        {

            distance = Vector3.Distance(enemyUnit.position, rampWaypoints[i].transform.position);

            if (distance < closestDistToEnemy && Mathf.Abs(enemyUnit.position.y - rampWaypoints[i].transform.position.y) <= 2f && currentRampPoint != rampWaypoints[i])
            {

                var ramp = rampWaypoints[i].transform.parent;

                foreach (Transform child in ramp)
                {
                    if (child.transform != rampWaypoints[i].transform && Mathf.Abs(target.position.y - child.position.y) <= 1)
                    {
                        closestDistToEnemy = distance;
                        closest = rampWaypoints[i].transform;
                        currentRampPoint = rampWaypoints[i];
                    }
                }
            }
        }

        targetPosition = closest.transform.position;
        return targetPosition;
    }

    public Vector3 FindNextRampPoint()
    {
        //print("finding next ramp point");

        bArrivedToRamp = false;

        var ramp = currentRampPoint.transform.parent;

        GameObject nextRampPoint = null;

        foreach (Transform child in ramp)
        {
            if (child != currentRampPoint)
            {
                nextRampPoint = child.gameObject;
            }
        }

        targetPosition = nextRampPoint.transform.position;

        return targetPosition;
    }

    public Vector3 GetTargetPosition()
    {
        return targetPosition;
    }
}
