using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class ThirdPersonCamera : MonoBehaviour
{
    #region VARIABLES

    [HideInInspector] public float cameraFOV = 0.0f;

    [Header("Public")]
    public GameObject cameraObject = null;
    public bool invertY = false;
    public Vector2 sensitivity = new Vector2(1.0f, 1.0f);

    public bool isEnabled { get; private set; } = true;
    public Vector3 lookDirection { get; private set; } = Vector3.zero;

    [Header("Serialized")]
    [SerializeField] private float cameraFOVLerpSpeed = 10.0f;
    [SerializeField] private Vector3 pivotPoint = Vector3.zero;

    private Vector3 cameraOffset = Vector3.zero;
    private float cameraFOVLerp = 0.0f;
    private Vector2 minMaxPitch = new Vector2(-85.0f, 85.0f);
    private Camera cameraComponent = null;
    private PostProcessLayer ppLayerComponent = null;

    //[SerializeField] private Transform cameraPivot = null;
    //[SerializeField] private CameraWallChecker wallChecker = null;

    //private Transform cameraOriginalPosition = null;
    //private Transform cameraTransform = null;
    //private bool bSwitchingSide = false;

    #endregion

    #region UNITY_DEFAULT_METHODS

    void Awake()
    {
        cameraComponent = cameraObject.GetComponent<Camera>();
        cameraOffset = cameraObject.transform.localPosition - pivotPoint;
        cameraFOV = cameraComponent.fieldOfView;
        cameraFOVLerp = cameraFOV;
        cameraObject.tag = "MainCamera";
        cameraObject.transform.parent = transform;

        if (cameraComponent.actualRenderingPath == UnityEngine.RenderingPath.DeferredShading)
        {
            cameraComponent.allowMSAA = false;
        }

        if (cameraObject.GetComponent<PostProcessLayer>() != null)
        {
            ppLayerComponent = cameraObject.GetComponent<PostProcessLayer>();
            switch (GlobalVariables.aaMethod)
            {
                case "fxaa": ppLayerComponent.antialiasingMode = PostProcessLayer.Antialiasing.FastApproximateAntialiasing; break;
                case "smaa": ppLayerComponent.antialiasingMode = PostProcessLayer.Antialiasing.SubpixelMorphologicalAntialiasing; break;
                case "taa": ppLayerComponent.antialiasingMode = PostProcessLayer.Antialiasing.TemporalAntialiasing; break;
                default: ppLayerComponent.antialiasingMode = PostProcessLayer.Antialiasing.None; break;
            }
        }

        lookDirection = transform.rotation.eulerAngles;
    }

    void OnEnable()
    {
        EnableCameraControls(true);
    }

    void OnDisable()
    {
        EnableCameraControls(false);
    }

    void Start()
    {
        //if (cameraPivot == null)
        //{
        //    Debug.LogWarning(this + " is missing a camera pivot!");
        //}
        //else
        //{
        //    cameraOriginalPosition = cameraPivot.GetChild(0);
        //    cameraTransform = cameraOriginalPosition.GetChild(0);
        //}
    }

    void LateUpdate()
    {
        if (isEnabled)
        {
            Look(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

            if (cameraFOVLerp != cameraFOV)
            {
                cameraComponent.fieldOfView = Mathf.Lerp(cameraFOVLerp, cameraFOV, Time.deltaTime * cameraFOVLerpSpeed);
                cameraFOVLerp = cameraComponent.fieldOfView;
            }
        }

        //if (bSwitchingSide && cameraTransform != null)
        //{
        //    if (wallChecker.checkingForWalls)
        //    {
        //        cameraTransform.localPosition = Vector3.zero;
        //        bSwitchingSide = false;
        //        return;
        //    }

        //    if (Vector3.Distance(cameraTransform.localPosition, Vector3.zero) > 0.05f)
        //    {
        //        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, Vector3.zero, Time.deltaTime * 8.0f);
        //    }
        //    else
        //    {
        //        cameraTransform.localPosition = Vector3.zero;
        //        bSwitchingSide = false;
        //    }
        //}
    }

    void OnDrawGizmosSelected()
    {
        //Don't draw gizmos when in play mode
        if (!UnityEditor.EditorApplication.isPlaying)
        {
            //Draw pivot point
            Gizmos.color = Color.red;
            Gizmos.DrawCube(transform.position + pivotPoint, Vector3.one * 0.15f);
        }
    }

    #endregion

    #region CUSTOM_METHODS

    public void EnableCameraControls(bool b)
    {
        Cursor.lockState = b ?
            CursorLockMode.Locked
            : CursorLockMode.None;
        Cursor.visible = !b;

        isEnabled = b;
    }

    void Look(float x, float y)
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

        cameraObject.transform.rotation = Quaternion.Euler(lookDirection);
        Vector3 offset = cameraObject.transform.right * cameraOffset.x + cameraObject.transform.up * cameraOffset.y + cameraObject.transform.forward * cameraOffset.z;
        cameraObject.transform.position = transform.position + pivotPoint + offset;


        //if (cameraPivot != null)
        //{
        //    lookDirection += new Vector3(
        //    y * sensitivity.x * (invertY ? 1.0f : -1.0f),
        //    x * sensitivity.y,
        //    0.0f
        //    );

        //    if (lookDirection.x < minMaxPitch.x)
        //    {
        //        lookDirection = new Vector3(minMaxPitch.x, lookDirection.y, lookDirection.z);
        //    }
        //    if (lookDirection.x > minMaxPitch.y)
        //    {
        //        lookDirection = new Vector3(minMaxPitch.y, lookDirection.y, lookDirection.z);
        //    }

        //    cameraPivot.localRotation = Quaternion.Euler(lookDirection);
        //}
    }

    //public void SwitchSide()
    //{
    //    if (cameraPivot != null)
    //    {
    //        Vector3 camPos = cameraOriginalPosition.localPosition;
    //        camPos.x *= -1;
    //        cameraOriginalPosition.localPosition = camPos;
    //        cameraTransform.localPosition = new Vector3(-cameraOriginalPosition.localPosition.x * 2, 0.0f, 0.0f);
    //        bSwitchingSide = true;
    //    }
    //}

    #endregion
}
