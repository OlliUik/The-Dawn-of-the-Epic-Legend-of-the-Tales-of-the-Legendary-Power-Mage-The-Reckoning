using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    [SerializeField] private Transform movingObject = null;
    [SerializeField] private Transform target = null;
    [SerializeField] private float speedDiv = 0.0f;
    [SerializeField] private float waitingTime = 0.0f;

    private float moveTime = 0.0f;
    private float originalWait = 0.0f;
    private bool isMovingTowards = false;
    private Vector3 origin = Vector3.zero;
    private Vector3 current = Vector3.zero;

    private void Start()
    {
        origin = movingObject.position;
        current = origin;

        originalWait = waitingTime;
        isMovingTowards = true;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (isMovingTowards)
        {
            moveTime += Time.deltaTime / speedDiv;
            current = new Vector3(Mathf.SmoothStep(origin.x, target.position.x, moveTime),
                                  Mathf.SmoothStep(origin.y, target.position.y, moveTime),
                                  Mathf.SmoothStep(origin.z, target.position.z, moveTime));
            movingObject.position = current;

            if (current == target.position)
            {
                waitingTime -= Time.deltaTime;
            }
        }

        else
        {
            moveTime += Time.deltaTime / speedDiv;
            current = new Vector3(Mathf.SmoothStep(target.position.x, origin.x, moveTime),
                                  Mathf.SmoothStep(target.position.y, origin.y, moveTime),
                                  Mathf.SmoothStep(target.position.z, origin.z, moveTime));
            movingObject.position = current;

            if (current == origin)
            {
                waitingTime -= Time.deltaTime;
            }
        }

        if (waitingTime < 0)
        {
            moveTime = 0;
            isMovingTowards = !isMovingTowards;
            waitingTime = originalWait;
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 cubeSize = new Vector3(0.25f, 0.25f, 0.25f);
        Gizmos.color = Color.red;
        Gizmos.DrawCube(origin, cubeSize);
        Gizmos.DrawCube(target.position, cubeSize);
    }
}