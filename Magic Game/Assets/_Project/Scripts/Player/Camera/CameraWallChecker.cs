using UnityEngine;

public class CameraWallChecker : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform = null;
    [SerializeField] private Transform cameraClosePosition = null;
    [SerializeField] private LayerMask physicsLayerMask = 1;

    private bool checkingForWalls = false;

    void OnTriggerStay(Collider other)
    {
        checkingForWalls = true;
    }

    void OnTriggerExit(Collider other)
    {
        checkingForWalls = false;
    }

    void LateUpdate()
    {
        if (checkingForWalls)
        {
            cameraTransform.position = cameraClosePosition.position;
        }
        else
        {
            cameraTransform.position = transform.position;
        }
    }
}
