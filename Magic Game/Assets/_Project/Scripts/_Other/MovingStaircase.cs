using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingStaircase : MonoBehaviour
{
    [SerializeField, Range(-90, 270)] private float targetY = 0.0f;
    [SerializeField] private float speedDiv = 0.0f;
    [SerializeField] private float waitingTime = 0.0f;

    private float moveTime = 0.0f;
    private float originalWait = 0.0f;
    private bool isPositiveRot = true;
    private Vector3 origin = Vector3.zero;
    private Vector3 current = Vector3.zero;

    private void Start()
    {
        origin = transform.eulerAngles;
        current = origin;
        originalWait = waitingTime;
    }

    private void Update()
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