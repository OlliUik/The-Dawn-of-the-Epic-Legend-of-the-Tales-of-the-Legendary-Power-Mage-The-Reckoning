using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapPendulum : MonoBehaviour
{
    [SerializeField, Range(0.0f, 360f)]
    private float angle = 90.0f;

    [SerializeField, Range(0.0f, 5.0f)]
    private float speed = 2.0f;

    [SerializeField, Range(0.0f, 10.0f)]
    private float startTime = 0.0f;

    Quaternion start, end;

    void Start()
    {
        start = PendulumRotation(angle);
        end = PendulumRotation(-angle);
    }

    void FixedUpdate()
    {
        startTime += Time.deltaTime;
        transform.rotation = Quaternion.Lerp(start, end, (Mathf.Sin(startTime * speed + Mathf.PI / 2) + 1.0f) / 2.0f);
    }

    Quaternion PendulumRotation(float angle)
    {
        var pendulumRot = transform.rotation;
        var angleZ = pendulumRot.eulerAngles.z + angle;

        if (angleZ > 180)
        {
            angleZ -= 360;
        }

        else if (angleZ < -180)
        {
            angleZ += 360;
        }

        pendulumRot.eulerAngles = new Vector3(pendulumRot.eulerAngles.x, pendulumRot.eulerAngles.y, angleZ);
        return pendulumRot;
    }
}