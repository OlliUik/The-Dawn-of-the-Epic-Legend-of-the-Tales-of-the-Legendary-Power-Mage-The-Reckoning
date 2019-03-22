using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    #region VARIABLES

    public bool invertY = false;
    public Vector2 sensitivity = new Vector2(1.0f, 1.0f);
    public Vector3 lookDirection = Vector3.zero;

    [SerializeField] private Transform cameraPivot = null;
    [SerializeField] private CameraWallChecker wallChecker = null;

    private Transform cameraOriginalPosition = null;
    private Transform cameraTransform = null;
    private bool bSwitchingSide = false;
    private Vector2 minMaxPitch = new Vector2(-85.0f, 85.0f);

    #endregion

    #region UNITY_DEFAULT_METHODS

    void Start()
    {
        if (cameraPivot == null)
        {
            Debug.LogWarning(this + " is missing a camera pivot!");
        }
        else
        {
            cameraOriginalPosition = cameraPivot.GetChild(0);
            cameraTransform = cameraOriginalPosition.GetChild(0);
        }
    }

    void LateUpdate()
    {
        if (bSwitchingSide && cameraTransform != null)
        {
            if (wallChecker.checkingForWalls)
            {
                cameraTransform.localPosition = Vector3.zero;
                bSwitchingSide = false;
                return;
            }

            if (Vector3.Distance(cameraTransform.localPosition, Vector3.zero) > 0.05f)
            {
                cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, Vector3.zero, Time.deltaTime * 8.0f);
            }
            else
            {
                cameraTransform.localPosition = Vector3.zero;
                bSwitchingSide = false;
            }
        }
    }

    #endregion

    #region CUSTOM_METHODS

    public void Look(float x, float y)
    {
        if (cameraPivot != null)
        {
            lookDirection += new Vector3(
            y * sensitivity.x * (invertY ? 1.0f : -1.0f),
            x * sensitivity.y,
            0.0f
            );

            if (lookDirection.x < minMaxPitch.x)
            {
                lookDirection = new Vector3(minMaxPitch.x, lookDirection.y, lookDirection.z);
            }
            if (lookDirection.x > minMaxPitch.y)
            {
                lookDirection = new Vector3(minMaxPitch.y, lookDirection.y, lookDirection.z);
            }

            cameraPivot.localRotation = Quaternion.Euler(lookDirection);
        }
    }

    public void SwitchSide()
    {
        if (cameraPivot != null)
        {
            Vector3 camPos = cameraOriginalPosition.localPosition;
            camPos.x *= -1;
            cameraOriginalPosition.localPosition = camPos;
            cameraTransform.localPosition = new Vector3(-cameraOriginalPosition.localPosition.x * 2, 0.0f, 0.0f);
            bSwitchingSide = true;
        }
    }

    #endregion
}
