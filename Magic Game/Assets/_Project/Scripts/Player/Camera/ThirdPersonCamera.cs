using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    #region VARIABLES
    
    public bool invertY                                 = false;
    public Vector2 sensitivity                          = new Vector2(1.0f, 1.0f);
    public Vector3 lookDirection = Vector3.zero;

    public bool bRightSide { get; private set; } = true;

    [SerializeField] private Transform cameraPivot      = null;

    private Vector2 minMaxPitch                         = new Vector2(-85.0f, 85.0f);

    #endregion

    #region UNITY_DEFAULT_METHODS

    void Start()
    {
        if (cameraPivot == null)
        {
            Debug.LogWarning(this + " is missing a camera pivot!");
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
            Transform camera = cameraPivot.GetChild(0);
            Vector3 camPos = camera.localPosition;
            camPos.x *= -1;
            camera.localPosition = camPos;
            bRightSide = !bRightSide;
        }
    }

    #endregion
}
