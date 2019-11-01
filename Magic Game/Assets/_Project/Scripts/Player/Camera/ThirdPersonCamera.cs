using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class ThirdPersonCamera : MonoBehaviour
{
    #region VARIABLES

    [Header("Input")]
    [SerializeField] private string horizontalAxis;
    [SerializeField] private string verticalAxis;
    //[SerializeField, Range(0, 20)] private float sensX;
    //[SerializeField, Range(0, 20)] private float sensY;

    [HideInInspector] public float cameraFOV = 0.0f;
    [HideInInspector] public bool isRagdolled = false;

    [Header("Public")]
    public GameObject cameraObject = null;
    public bool invertY = false;
    public bool slowdownEnabled = false;
    public Vector2 slowdownMaxTurn = new Vector2(25.0f, 25.0f);

    public bool isEnabled { get; private set; } = true;
    public Vector3 lookDirection { get; private set; } = Vector3.zero;

    [Header("Serialized")]
    [SerializeField] private float cameraFOVLerpSpeed = 10.0f;
    [SerializeField] private Vector3 pivotPoint = Vector3.zero;
    [SerializeField] private Transform pivotTransformRagdolled = null;
    [SerializeField] private Vector3 cameraClosePosition = Vector3.zero;
    [SerializeField] private LayerMask raycastLayerMask = 1;

    //private Vector2 sensitivity = new Vector2(1.0f, 1.0f);
    private Vector3 cameraOffset = Vector3.zero;
    private float cameraFOVLerp = 0.0f;
    private Vector2 minMaxPitch = new Vector2(-85.0f, 85.0f);
    public Camera cameraComponent { get; private set; } = null;
    private PostProcessLayer ppLayerComponent = null;
    private InputManager inputManager = null;

    //[SerializeField] private Transform cameraPivot = null;
    //[SerializeField] private CameraWallChecker wallChecker = null;

    //private Transform cameraOriginalPosition = null;
    //private Transform cameraTransform = null;
    //private bool bSwitchingSide = false;

    #endregion

    #region UNITY_DEFAULT_METHODS

    void Awake()
    {
        inputManager = GetComponent<InputManager>();
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

    void Update()
    {
        if (inputManager.controllerId == 1)
        {
            horizontalAxis = "Mouse X"; //Xbox_Mouse X
            verticalAxis = "Mouse Y"; //Xbox_Mouse Y
            //sensX = 10; //Default value?
            //sensY = 10; //Default value?
            //sensitivity = new Vector2(sensX, sensY);
        }

        if (inputManager.controllerId == 2)
        {
            horizontalAxis = "PS_Mouse X";
            verticalAxis = "PS_Mouse Y";
            //sensX = 10; //Default value?
            //sensY = 10; //Default value?
            //sensitivity = new Vector2(sensX, sensY);
        }

        else
        {
            horizontalAxis = "Mouse X";
            verticalAxis = "Mouse Y";
            //sensX = 2; //Default value?
            //sensY = 2; //Default value?
            //sensitivity = new Vector2(sensX, sensY);
        }
    }

    void LateUpdate()
    {
        if (isEnabled)
        {
            if (slowdownEnabled)
            {
                float time = Time.deltaTime;
                Look(Mathf.Clamp(Input.GetAxis(horizontalAxis), -slowdownMaxTurn.x * time, slowdownMaxTurn.x * time), Mathf.Clamp(Input.GetAxis(verticalAxis), -slowdownMaxTurn.y * time, slowdownMaxTurn.y * time));
            }
            else
            {
                Look(Input.GetAxis(horizontalAxis), Input.GetAxis(verticalAxis));
            }

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
    #if UNITY_EDITOR
        //Don't draw certain gizmos when in play mode
        if (!UnityEditor.EditorApplication.isPlaying)
        {
            //Draw pivot point
            Gizmos.color = Color.red;
            Gizmos.DrawCube(transform.position + pivotPoint, Vector3.one * 0.15f);
        }

        //Draw close position
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(transform.position + pivotPoint + cameraClosePosition, Vector3.one * 0.18f);
    #endif
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
        #region CAMERA_TURNING

        lookDirection += new Vector3(
            y * GlobalVariables.sensitivity.x * (invertY ? 1.0f : -1.0f),
            x * GlobalVariables.sensitivity.y,
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

        if (isRagdolled)
        {
            cameraObject.transform.position = pivotTransformRagdolled.position + cameraObject.transform.forward * -5.0f;
        }
        else
        {
            cameraObject.transform.position = transform.position + pivotPoint + offset;
        }

        #endregion

        #region CAMERA_WALLCHECKING

        if (isRagdolled)
        {
            return;
        }

        Vector3 cameraRaycast = cameraObject.transform.right * (cameraOffset.x - cameraClosePosition.x)
            + cameraObject.transform.up * (cameraOffset.y - cameraClosePosition.y)
            + cameraObject.transform.forward * (cameraOffset.z - cameraClosePosition.z);

        RaycastHit hit;
        float shortestDistance = Mathf.Infinity;

        for (int i = 0; i < 4; i++)
        {
            Vector2 checkCorner = Vector2.zero;

            switch (i)
            {
                case 0: checkCorner = Vector2.one; break;
                case 1: checkCorner = Vector2.up + Vector2.left; break;
                case 2: checkCorner = -Vector2.one; break;
                case 3: checkCorner = -(Vector2.up + Vector2.left); break;
                default: Debug.Log("Somehow " + this.gameObject + " made less/more raycasts than intended!"); break;
            }

            checkCorner *= cameraComponent.nearClipPlane;
            checkCorner.x *= cameraComponent.aspect;

            Vector3 v3 = cameraObject.transform.right * checkCorner.x * 1.0f + cameraObject.transform.up * checkCorner.y * 1.0f;
            Debug.DrawLine(transform.position + pivotPoint + cameraClosePosition + v3, transform.position + pivotPoint + cameraClosePosition + cameraRaycast + v3, Color.yellow);

            if (Physics.Raycast(
                  transform.position + pivotPoint + cameraClosePosition + v3,
                  cameraRaycast,
                  out hit,
                  Vector3.Magnitude(cameraOffset),
                  raycastLayerMask
                  ))
            {
                float distance = Vector3.Distance(transform.position + pivotPoint + cameraClosePosition + v3, hit.point);
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                }
            }
        }

        if (shortestDistance != Mathf.Infinity)
        {
            if (Physics.Raycast(
            transform.position + pivotPoint + cameraClosePosition,
            cameraRaycast,
            out hit,
            Mathf.Infinity,
            raycastLayerMask
            ))
            {
                cameraObject.transform.position = transform.position + pivotPoint + cameraClosePosition + Vector3.Normalize(cameraRaycast) * shortestDistance;
            }
        }
        
        Debug.DrawLine(transform.position + pivotPoint + cameraClosePosition, transform.position + pivotPoint + cameraClosePosition + cameraRaycast, Color.yellow);
        
        #endregion

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
