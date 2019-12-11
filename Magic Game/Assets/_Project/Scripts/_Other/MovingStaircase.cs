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
    private float originY = 0.0f;
    private Vector3 current = Vector3.zero;

    private void Start()
    {
        originY = transform.localEulerAngles.y;
        originalWait = waitingTime;
        //Debug.Log(originY);
    }

    private void Update()
    {
        if (isPositiveRot)
        {
            moveTime += Time.deltaTime / speedDiv;
            current = new Vector3(0, Mathf.SmoothStep(originY, targetY, moveTime), 0);
            transform.localEulerAngles = current;

            if (current.y == targetY)
            {
                waitingTime -= Time.deltaTime;
            }
        }

        if (!isPositiveRot)
        {
            moveTime += Time.deltaTime / speedDiv;
            current = new Vector3(0, Mathf.SmoothStep(targetY, originY, moveTime), 0);
            transform.localEulerAngles = current;

            if (current.y == originY)
            {
                waitingTime -= Time.deltaTime;
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