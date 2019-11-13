using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class CustomAgent : MonoBehaviour
{
    #region VARIABLES

    private NavMeshAgent agent;
    private Rigidbody rb;
    private NavMeshPath path = null;

    public bool hasPath { get; private set; } = false;
    public bool isStopped { get; set; } = false;
    private bool moving = false;
    private bool jumping = false;
    private bool hasJumped = false;

    private List<Vector3> corners;
    private Vector3 nextPos;
    private Vector3 direction;
    public Vector3 velocity { get; set; }

    public float stoppingDistance { get; set; } = 0f;
    public float speed { get; set; } = 8f;
    public float acceleration { get; set; }

    #endregion

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        path = new NavMeshPath();

        // Stop main agent from moving around.
        agent.autoTraverseOffMeshLink = false;
        agent.isStopped = true;
    }

    void Update()
    {
    }

    void FixedUpdate()
    {
        // Move agent along the path if it is available and agent is not stopped.
        if (hasPath && !isStopped)
        {
            MoveAlongPath();
        }
        else
        {
            return;
        }
    }

    public void Warp(Vector3 position)
    {
        transform.position = position;
    }

    public float GetRemainingDistance()
    {
        return agent.remainingDistance - stoppingDistance;
    }

    public void SetDestination(Vector3 target)
    {
        if (!jumping)
        {
            ResetPath();
            agent.CalculatePath(target, path);
            agent.path = path;

            if (corners == null)
            {
                corners = new List<Vector3>(path.corners);
            }
            else
            {
                corners.AddRange(path.corners);
            }
           
            hasPath = true;
            isStopped = false;
        }
    }

    public void ResetPath()
    {
        hasPath = false;
        moving = false;
        path.ClearCorners();
        if (corners != null) corners.Clear();
    }

    void MoveAlongPath()
    {
        if (corners.Count < 1)
        {
            hasPath = false;
        }
        else
        {
            MoveNextPos();
        }
    }

    void MoveNextPos()
    {
        if (agent.remainingDistance <= stoppingDistance) isStopped = true;
        if (moving)
        {
            if (VEqual(transform.position, nextPos))
            {
                moving = false;
                corners.Remove(corners[0]);
            }
            else
            {
                direction = (nextPos - transform.position).normalized;
                rb.MovePosition(transform.position + (direction * speed * Time.deltaTime));
            }
        }
        else if (jumping)
        {
            if (VEqual(transform.position, nextPos))
            {
                jumping = false;
                agent.CompleteOffMeshLink();
                corners.Remove(corners[0]);
            }
            else if (!hasJumped)
            {
                Vector3 force = CalculateForce(transform.position, nextPos);
                Debug.Log(force);
                rb.AddForce(force, ForceMode.Impulse);
                hasJumped = true;
            }
            else return;
        }
        else
        {
            if (agent.isOnOffMeshLink && !hasJumped)
            {
                nextPos = new Vector3(corners[0].x, corners[0].y + transform.localScale.y, corners[0].z);
                jumping = true;
                Debug.Log("Jump");
            }
            else
            {
                nextPos = new Vector3(corners[0].x, corners[0].y + transform.localScale.y, corners[0].z);
                moving = true;
                hasJumped = false;
            }
        }
    }

    bool VEqual(Vector3 a, Vector3 b)
    {
        return Vector3.SqrMagnitude(a - b) < 0.01;
    }

    // Calculate force needed for jump. (Not perfectly functional)
    Vector3 CalculateForce(Vector3 start, Vector3 end)
    {
        float gravity = Physics.gravity.magnitude;
        float defangle = 45f;
        Vector3 direction = end - start;
        float angle = Mathf.Abs(Mathf.Atan2(end.y - start.y, end.x - start.x) * Mathf.Rad2Deg);

        if (direction.y < 0)
        {
            angle = 0;
        }
        else if (angle <= defangle)
        {
            angle = defangle;
        }
        else
        {
            angle = angle + 45;
        }

        float height = direction.y;
        direction.y = 0;
        float distance = direction.magnitude;
        float rad = defangle * Mathf.Deg2Rad;
        direction.y = distance * Mathf.Tan(rad);
        distance += height / Mathf.Tan(rad);

        float velocity = Mathf.Sqrt(distance * gravity / Mathf.Sin(2 * rad));

        return direction.normalized * velocity;
    }

    void OnDrawGizmos()
    {
        if (corners != null)
        {
            foreach (Vector3 c in corners)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(c, 0.25f);
            }
            Gizmos.DrawLine(transform.position, nextPos);
        }
    }

}
