using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private float moveTime = 5.0f;
    [SerializeField] private float moveSpeed = 1.0f;
    [SerializeField] private float rotationSpeed = 20.0f;

    public Vector3 positionDelta { get; private set; } = Vector3.zero;
    public Vector3 rotationDelta { get; private set; } = Vector3.zero;

    private float moveTimeTemp = 0.0f;
    private bool moveDown = false;

    void Start()
    {
        moveTimeTemp = moveTime;
    }

    void FixedUpdate()
    {
        positionDelta = transform.position;
        rotationDelta = transform.rotation.eulerAngles;

        if (moveTimeTemp > 0.0f)
        {
            transform.position += (moveDown ? Vector3.down + Vector3.left : Vector3.up + Vector3.right) * moveSpeed * Time.fixedDeltaTime;
            transform.Rotate(0.0f, rotationSpeed * Time.fixedDeltaTime, 0.0f);
            moveTimeTemp -= Time.fixedDeltaTime;
        }
        else
        {
            moveDown = !moveDown;
            moveTimeTemp = moveTime;
        }

        positionDelta -= transform.position;
        positionDelta = -positionDelta;
        rotationDelta -= transform.rotation.eulerAngles;
        rotationDelta = -rotationDelta;
    }
}
