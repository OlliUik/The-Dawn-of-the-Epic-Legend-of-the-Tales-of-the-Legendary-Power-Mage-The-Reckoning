using UnityEngine;

public class CameraWallChecker : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform = null;
    [SerializeField] private Transform cameraClosePosition = null;
    [SerializeField] private LayerMask physicsLayerMask = 1;

    public bool checkingForWalls { get; private set; } = false;
    
    void OnTriggerStay(Collider other)
    {
        if (physicsLayerMask == (physicsLayerMask | (1 << other.gameObject.layer)))
        {
            if (other.tag != "Player")
            {
                checkingForWalls = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        cameraTransform.position = transform.position;
        checkingForWalls = false;
    }

    void LateUpdate()
    {
        if (checkingForWalls)
        {
            cameraTransform.position = cameraClosePosition.position;
        }
    }
}
