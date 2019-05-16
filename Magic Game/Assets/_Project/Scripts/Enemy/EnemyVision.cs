using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    #region VARIABLES

    public Transform headTransform = null;

    [HideInInspector] public Vector3 targetLocation = Vector3.zero;

    [SerializeField] private bool alwaysSeeTarget = false;
    [SerializeField] private bool neverLoseVisionToTarget = false;
    [SerializeField] private float sightDistance = 30.0f;
    [SerializeField] [Range(1.0f, 180.0f)] private float sightRadius = 45.0f;
    [SerializeField] private float checkInterval = 0.2f;
    //[SerializeField] private float checkIntervalRandomRangeMax = 2.0f;
    [SerializeField] private float checkHeightOffset = 0.5f;
    [SerializeField] private LayerMask raycastLayerMask = 3073;

    public bool bCanSeeTarget { get; private set; } = false;
    public GameObject targetGO { get; private set; } = null;

    public float HeightOffset { get { return checkHeightOffset; } }

    private bool debugCheckingForTarget = false;
    //private float checkTimer = 0.0f;
    private float sightRadiusTemp = 0.0f;
    //private float raycastGraceTimer = 0.0f;
    private EnemyCore cEnemyCore = null;

    #endregion

    #region MAGIC_NUMBERS

    /*--------------------------------------------------------------*/

    //These two variables describe the vision "pyramid".
    //Do not modify!

    private Vector3[] visionVertices = new Vector3[]
    {
        new Vector3(0.0f, 0.0f, 0.0f),
        new Vector3(1.0f, 1.0f, 1.0f),
        new Vector3(1.0f, -1.0f, 1.0f),
        new Vector3(-1.0f, 1.0f, 1.0f),
        new Vector3(-1.0f, -1.0f, 1.0f)
    };

    private Vector3[] vvModified = null;

    private int[] visionTriangles = new int[]
    {
        0, 1, 2,
        0, 3, 1,
        0, 4, 3,
        0, 2, 4
    };

    /*--------------------------------------------------------------*/

    #endregion

    #region UNITY_DEFAULT_METHODS

    void Awake()
    {
        cEnemyCore = GetComponent<EnemyCore>();
    }

    void Start()
    {
        vvModified = visionVertices;
        InvokeRepeating("CheckVision", Time.fixedDeltaTime * cEnemyCore.entitySpawnNumber, Time.fixedDeltaTime * Mathf.FloorToInt(checkInterval / Time.fixedDeltaTime));
    }

    void OnDrawGizmosSelected()
    {
        Vector3[] vvTemp = new Vector3[visionVertices.Length];

        for (int i = 1; i < visionVertices.Length; i++)
        {
            vvTemp[i] = visionVertices[i];
            vvTemp[i].z *= -Mathf.Tan((sightRadius / 2 + 90.0f) * Mathf.Deg2Rad);
            vvTemp[i] = vvTemp[i].normalized;
        }

        vvTemp = TranslateVertices(vvTemp, headTransform.position, headTransform.rotation);

        Gizmos.color = debugCheckingForTarget ?
            Color.green
            : Color.red;

        Gizmos.DrawLine(vvTemp[0], vvTemp[1]);
        Gizmos.DrawLine(vvTemp[0], vvTemp[2]);
        Gizmos.DrawLine(vvTemp[0], vvTemp[3]);
        Gizmos.DrawLine(vvTemp[0], vvTemp[4]);

        Gizmos.DrawLine(headTransform.position, headTransform.position + (headTransform.forward * sightDistance));

        Gizmos.DrawSphere(targetLocation, 0.4f);

        debugCheckingForTarget = false;
    }

    #endregion

    #region CUSTOM_METHODS

    void CheckVision()
    {
        if (cEnemyCore.status.isOnFire || cEnemyCore.status.isKnocked)
        {
            return;
        }

        debugCheckingForTarget = true;

        if (sightRadiusTemp != sightRadius)
        {
            sightRadiusTemp = sightRadius;
            for (int i = 1; i < vvModified.Length; i++)
            {
                vvModified[i].z *= -Mathf.Tan((sightRadius / 2 + 90.0f) * Mathf.Deg2Rad);
                vvModified[i] = vvModified[i].normalized;
            }
        }

        Vector3[] vvTemp = new Vector3[vvModified.Length];
        vvTemp = TranslateVertices(vvModified, headTransform.position, headTransform.rotation);

        if (!bCanSeeTarget)
        {
            foreach (GameObject entity in (cEnemyCore.status.isConfused ? GlobalVariables.teamBadBoys : GlobalVariables.teamGoodGuys))
            {
                Vector3 entityPosition = entity.transform.position + Vector3.up * checkHeightOffset;

                if (alwaysSeeTarget)
                {
                    targetGO = entity;
                    break;
                }

                if ((headTransform.position - entityPosition).sqrMagnitude < sightDistance * sightDistance)
                {
                    if (IsPointInside(vvTemp, visionTriangles, entityPosition))
                    {
                        targetGO = entity;
                        break;
                    }
                }
            }
        }

        if (targetGO != null)
        {
            Vector3 entityPosition = targetGO.transform.position + Vector3.up * checkHeightOffset;
            Vector3 entityDirection = -Vector3.Normalize(headTransform.position - entityPosition);

            if (alwaysSeeTarget || neverLoseVisionToTarget)
            {
                bCanSeeTarget = true;
                targetLocation = entityPosition;
                return;
            }

            if (IsPointInside(vvTemp, visionTriangles, entityPosition))
            {
                RaycastHit hit;
                if (Physics.Raycast(headTransform.position, entityDirection, out hit, sightDistance, raycastLayerMask))
                {
                    if (hit.transform == targetGO.transform)
                    {
                        bCanSeeTarget = true;
                        targetLocation = entityPosition;
                    }
                    else
                    {
                        if (bCanSeeTarget)
                        {
                            if (Physics.Raycast(entityPosition, Vector3.down, out hit, Mathf.Infinity, raycastLayerMask))
                            {
                                targetLocation = hit.point + Vector3.up * checkHeightOffset;
                            }
                            else
                            {
                                targetLocation = entityPosition + Vector3.up * checkHeightOffset;
                            }
                        }
                        bCanSeeTarget = false;
                    }
                }
                else
                {
                    bCanSeeTarget = false;
                }
            }
            else
            {
                if (bCanSeeTarget)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(targetLocation, Vector3.down, out hit, Mathf.Infinity, raycastLayerMask))
                    {
                        if (Vector3.Distance(targetLocation, hit.point) > 0.5f)
                        {
                            targetLocation = hit.point + Vector3.up * checkHeightOffset;
                        }
                    }
                    bCanSeeTarget = false;
                }
            }
        }
        else
        {
            if (bCanSeeTarget)
            {
                //Target was destroyed, reset some variables
                bCanSeeTarget = false;
                targetLocation = Vector3.zero;
            }
        }
    }

    Vector3[] TranslateVertices(Vector3[] inputArray, Vector3 translateVector, Quaternion headRotation)
    {
        Vector3[] returnArray = new Vector3[inputArray.Length];
        Matrix4x4 rotationMatrix = Matrix4x4.Rotate(headRotation);

        for (int i = 0; i < returnArray.Length; i++)
        {
            returnArray[i] = rotationMatrix.MultiplyPoint3x4(inputArray[i]) + translateVector;
        }

        return returnArray;
    }

    //Mesh GenerateMesh(Vector3[] vertices, int[] triangles)
    //{
    //    Mesh mesh = new Mesh();
    //    mesh.Clear();
    //    mesh.vertices = vertices;
    //    mesh.triangles = triangles;
    //    return mesh;
    //}

    bool IsPointInside(Vector3[] vertices, int[] triangles, Vector3 aLocalPoint)
    {
        int triangleCount = triangles.Length / 3;
        for (int i = 0; i < triangleCount; i++)
        {
            Vector3 V1 = vertices[triangles[i * 3]];
            Vector3 V2 = vertices[triangles[i * 3 + 1]];
            Vector3 V3 = vertices[triangles[i * 3 + 2]];
            Plane P = new Plane(V1, V2, V3);
            if (P.GetSide(aLocalPoint))
            {
                return false;
            }
        }
        return true;
    }

    #endregion
}
