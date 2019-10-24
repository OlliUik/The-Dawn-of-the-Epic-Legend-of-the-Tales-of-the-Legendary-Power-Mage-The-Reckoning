using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingStaircase : MonoBehaviour
{
    [SerializeField, Range(-90, 270)]
    private float targetY;

    [SerializeField]
    private float speedDiv;

    [SerializeField]
    private float waitingTime;
    
    private Vector3 origin, current;
    
    public bool isPositiveRot = true;

    float moveTime, originalWait;

    void Start()
    {
        origin = transform.eulerAngles;
        current = origin;
        originalWait = waitingTime;
    }

    void Update()
    {
        if (targetY >= 0)
        {
            if (isPositiveRot)
            {
                moveTime += Time.deltaTime / speedDiv;
                current = new Vector3(0, Mathf.SmoothStep(origin.y, targetY, moveTime), 0);
                transform.eulerAngles = current;

                if (current.y == targetY)
                {
                    waitingTime -= Time.deltaTime;
                }
            }

            if (!isPositiveRot)
            {
                moveTime += Time.deltaTime / speedDiv;
                current = new Vector3(0, Mathf.SmoothStep(targetY, origin.y, moveTime), 0);
                transform.eulerAngles = current;

                if (current.y == origin.y)
                {
                    waitingTime -= Time.deltaTime;
                }
            }
        }

        if (targetY < 0)
        {
            if (isPositiveRot)
            {
                moveTime += Time.deltaTime / speedDiv;
                current = new Vector3(0, Mathf.SmoothStep(origin.y, targetY, moveTime), 0);
                transform.eulerAngles = current;

                if (current.y == targetY)
                {
                    waitingTime -= Time.deltaTime;
                }
            }

            if (!isPositiveRot)
            {
                moveTime += Time.deltaTime / speedDiv;
                current = new Vector3(0, Mathf.SmoothStep(targetY, origin.y, moveTime), 0);
                transform.eulerAngles = current;

                if (current.y == origin.y)
                {
                    waitingTime -= Time.deltaTime;
                }
            }
        }

        if (waitingTime < 0)
        {
            moveTime = 0;
            isPositiveRot = !isPositiveRot;
            waitingTime = originalWait;
        }
    }
}