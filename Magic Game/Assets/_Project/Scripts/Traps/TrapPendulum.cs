using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapPendulum : MonoBehaviour
{
    [SerializeField, Range(0.0f, 360f)] private float angle = 90.0f;
    [SerializeField, Range(0.0f, 5.0f)] private float speed = 2.0f;
    [SerializeField, Range(0.0f, 10.0f)] private float startTime = 0.0f;

    private Quaternion start = new Quaternion(0, 0, 0, 0);
    private Quaternion end = new Quaternion(0, 0, 0, 0);

    private void Start()
    {
        start = PendulumRotation(angle);
        end = PendulumRotation(-angle);
    }

    private void FixedUpdate()
    {
        startTime += Time.deltaTime;
        transform.rotation = Quaternion.Lerp(start, end, (Mathf.Sin(startTime * speed + Mathf.PI / 2) + 1.0f) / 2.0f);
    }

    private Quaternion PendulumRotation(float angle)
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