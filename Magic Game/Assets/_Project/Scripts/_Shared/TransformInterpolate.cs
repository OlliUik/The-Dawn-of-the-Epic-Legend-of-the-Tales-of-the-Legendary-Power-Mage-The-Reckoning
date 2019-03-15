using UnityEngine;

public class TransformInterpolate : MonoBehaviour
{
    private Transform tf = null;
    private Vector3 currentPosition = Vector3.zero;
    private Vector3 prevPosition = Vector3.zero;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        tf = this.transform;
        currentPosition = tf.position;
    }

    void LateUpdate()
    {
        float timeBetween = (Time.time - Time.fixedTime) / Time.fixedDeltaTime;
        Vector3 difference = currentPosition - prevPosition;
        tf.position = prevPosition + Vector3.Lerp(Vector3.zero, difference, timeBetween);
    }

    void FixedUpdate()
    {
        prevPosition = currentPosition;
        currentPosition = tf.position;
    }
}
