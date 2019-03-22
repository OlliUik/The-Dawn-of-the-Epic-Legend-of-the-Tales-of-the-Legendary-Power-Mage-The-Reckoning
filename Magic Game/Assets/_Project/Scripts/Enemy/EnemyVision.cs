using UnityEngine;
//using UnityEngine.AI;

[RequireComponent(typeof(EnemyCore))]
public class EnemyVision : MonoBehaviour
{
    #region VARIABLES

    public Transform headTransform = null;

    [SerializeField] private float sightDistance = 30.0f;
    [SerializeField] private float sightRadius = 45.0f;
    [SerializeField] private float checkInterval = 0.5f;
    [SerializeField] private float checkIntervalRandomRangeMax = 2.0f;

    public bool bCanSeeTarget { get; private set; } = false;
    public Vector3 targetLocation { get; private set; } = Vector3.zero;

    private float checkTimer = 0.0f;
    private float raycastGraceTimer = 0.0f;
    private GameObject targetGO = null;
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
        checkTimer = Random.Range(0.0f, checkIntervalRandomRangeMax);

        for (int i = 0; i < visionVertices.Length; i++)
        {
            visionVertices[i].x *= sightRadius / 2;
            visionVertices[i].y *= sightRadius / 2;
            visionVertices[i].z *= sightDistance;
        }
    }

    void FixedUpdate()
    {
        #region OLD_METHOD
        /*

        playerPosition = GlobalVariables.player.transform.position + playerOffset;
        Vector3 playerDirection = -Vector3.Normalize(transform.position - playerPosition);
        Quaternion headRotation = headTransform.rotation;

        if (bCanSeePlayer)
        {
            headTransform.LookAt(playerPosition);
            headTransform.rotation = Quaternion.Lerp(headTransform.rotation, headRotation, 0.9f);

            RaycastHit hit;
            if (Physics.Raycast(
                playerPosition,
                Vector3.down,
                out hit,
                Mathf.Infinity,
                1
                ))
            {
                playerLocation = hit.point + playerOffset;
            }
            else
            {
                playerLocation = playerPosition;
            }
        }
        else
        {
            headTransform.LookAt(headTransform.position + Vector3.Normalize(GetComponent<NavMeshAgent>().velocity));
            headTransform.rotation = Quaternion.Lerp(headTransform.rotation, headRotation, 0.9f);

            if (Vector3.Distance(transform.position, playerLocation) < 1.0f)
            {
                playerLocation = Vector3.zero;
            }
        }

        if (!bCanSeePlayer && checkTimer > 0.0f)
        {
            checkTimer -= Time.fixedDeltaTime;
        }
        else
        {
            if (!bCanSeePlayer)
            {
                checkTimer = checkInterval;
            }

            if (Vector3.Distance(headTransform.position, playerPosition) < sightDistance)
            {
                Vector3[] vvTemp = new Vector3[visionVertices.Length];
                vvTemp = TranslateVertices(visionVertices, transform.position, headTransform.rotation);
                Mesh mesh = GenerateMesh(vvTemp, visionTriangles);
                bool bIsInside = IsPointInside(mesh, playerPosition);

                if (bIsInside)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(
                        transform.position,
                        playerDirection,
                        out hit,
                        sightDistance,
                        1
                        ))
                    {
                        if (hit.transform.tag == "Player")
                        {
                            bCanSeePlayer = true;
                            raycastGraceTimer = 0.2f;
                        }
                        else
                        {
                            if (raycastGraceTimer > 0.0f)
                            {
                                raycastGraceTimer -= Time.fixedDeltaTime;
                            }
                            else
                            {
                                bCanSeePlayer = false;
                            }
                        }
                    }
                    else
                    {
                        if (raycastGraceTimer > 0.0f)
                        {
                            raycastGraceTimer -= Time.fixedDeltaTime;
                        }
                        else
                        {
                            bCanSeePlayer = false;
                        }
                    }
                }

                //Debug lines
                if (bIsInside)
                {
                    Debug.DrawLine(transform.position, transform.position + playerDirection * sightDistance, bCanSeePlayer ? Color.green : Color.red);
                    Debug.DrawLine(vvTemp[0], vvTemp[1], bCanSeePlayer ? Color.green : Color.yellow);
                    Debug.DrawLine(vvTemp[0], vvTemp[2], bCanSeePlayer ? Color.green : Color.yellow);
                    Debug.DrawLine(vvTemp[0], vvTemp[3], bCanSeePlayer ? Color.green : Color.yellow);
                    Debug.DrawLine(vvTemp[0], vvTemp[4], bCanSeePlayer ? Color.green : Color.yellow);
                }
                else
                {
                    Debug.DrawLine(vvTemp[0], vvTemp[1], Color.red);
                    Debug.DrawLine(vvTemp[0], vvTemp[2], Color.red);
                    Debug.DrawLine(vvTemp[0], vvTemp[3], Color.red);
                    Debug.DrawLine(vvTemp[0], vvTemp[4], Color.red);
                }
            }
            else
            {
                bCanSeePlayer = false;
            }
        }

        */
        #endregion

        if (checkTimer <= 0.0f || bCanSeeTarget)
        {
            checkTimer = checkInterval;

            Vector3[] vvTemp = new Vector3[visionVertices.Length];
            vvTemp = TranslateVertices(visionVertices, headTransform.position, headTransform.rotation);
            Mesh mesh = GenerateMesh(vvTemp, visionTriangles);

            if (!bCanSeeTarget)
            {
                foreach (GameObject entity in GlobalVariables.entityList)
                {
                    if (entity.tag == (cEnemyCore.status.isConfused ? "Enemy" : "Player"))
                    {
                        if (Vector3.Distance(headTransform.position, entity.transform.position) < sightDistance)
                        {
                            if (IsPointInside(mesh, entity.transform.position))
                            {
                                targetGO = entity;
                                break;
                            }
                        }
                    }
                }
            }

            if (targetGO != null)
            {
                Vector3 entityPosition = Vector3.zero;
                Vector3 entityDirection = Vector3.zero;

                if (targetGO.tag == "Player")
                {
                    entityPosition = targetGO.transform.position + Vector3.up * targetGO.GetComponent<CharacterController>().height / 2;
                    entityDirection = -Vector3.Normalize(headTransform.position - entityPosition);
                }
                else
                {
                    entityPosition = targetGO.transform.position + Vector3.up * 0.5f;
                    entityDirection = -Vector3.Normalize(headTransform.position - entityPosition);
                }

                if (IsPointInside(mesh, entityPosition))
                {
                    RaycastHit hit;
                    if (Physics.Raycast(
                        headTransform.position,
                        entityDirection,
                        out hit,
                        sightDistance,
                        1
                        ))
                    {
                        if (hit.transform == targetGO.transform)
                        {
                            bCanSeeTarget = true;
                            raycastGraceTimer = 0.2f;
                            targetLocation = entityPosition;
                        }
                        else
                        {
                            if (raycastGraceTimer > 0.0f)
                            {
                                raycastGraceTimer -= Time.fixedDeltaTime;
                            }
                            else
                            {
                                if (bCanSeeTarget)
                                {
                                    if (Physics.Raycast(
                                        entityPosition,
                                        Vector3.down,
                                        out hit,
                                        Mathf.Infinity,
                                        1
                                        ))
                                    {
                                        targetLocation = hit.point + Vector3.up * 0.5f;
                                    }
                                    else
                                    {
                                        targetLocation = entityPosition;
                                    }
                                }
                                bCanSeeTarget = false;
                            }
                        }
                    }
                    else
                    {
                        bCanSeeTarget = false;
                    }
                }
                else
                {
                    bCanSeeTarget = false;
                }

                //Draw the pyramid with debug lines
                {
                    if (IsPointInside(mesh, entityPosition))
                    {
                        Debug.DrawLine(headTransform.position, headTransform.position + entityDirection * sightDistance, bCanSeeTarget ? Color.green : Color.red);
                        if (cEnemyCore.currentEnemyType != EnemyCore.EEnemyType.MELEE)
                        {
                            Debug.DrawLine(headTransform.position, headTransform.position + entityDirection * cEnemyCore.RangedEscapeRadius * 2, Color.yellow);
                            Debug.DrawLine(headTransform.position, headTransform.position + entityDirection * cEnemyCore.RangedEscapeRadius, Color.red);
                        }
                    }
                    Debug.DrawLine(vvTemp[0], vvTemp[1], bCanSeeTarget ? Color.green : Color.yellow);
                    Debug.DrawLine(vvTemp[0], vvTemp[2], bCanSeeTarget ? Color.green : Color.yellow);
                    Debug.DrawLine(vvTemp[0], vvTemp[3], bCanSeeTarget ? Color.green : Color.yellow);
                    Debug.DrawLine(vvTemp[0], vvTemp[4], bCanSeeTarget ? Color.green : Color.yellow);
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

                Debug.DrawLine(vvTemp[0], vvTemp[1], Color.red);
                Debug.DrawLine(vvTemp[0], vvTemp[2], Color.red);
                Debug.DrawLine(vvTemp[0], vvTemp[3], Color.red);
                Debug.DrawLine(vvTemp[0], vvTemp[4], Color.red);
            }
        }
        else
        {
            checkTimer -= Time.fixedDeltaTime;
        }

        //Look at the player (IMPLEMENT THIS BETTER LATER)
        //Quaternion headRotation = headTransform.rotation;

        //if (bCanSeeTarget)
        //{
        //    if (targetGO != null)
        //    {
        //        headTransform.LookAt(targetGO.transform.position + Vector3.up * 0.5f);
        //    }
        //}
        //else
        //{
        //    headTransform.LookAt(headTransform.position + Vector3.Normalize(GetComponent<NavMeshAgent>().velocity));
        //}

        //headTransform.rotation = Quaternion.Lerp(headTransform.rotation, headRotation, 0.9f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = bCanSeeTarget ?
            Color.red
            : Color.yellow;
        Gizmos.DrawSphere(targetLocation, 0.4f);

        Gizmos.color = new Color(1.0f, 0.0f, 0.0f, 0.2f);
    }

    #endregion

    #region CUSTOM_METHODS

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

    Mesh GenerateMesh(Vector3[] vertices, int[] triangles)
    {
        Mesh mesh = new Mesh();
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        return mesh;
    }

    bool IsPointInside(Mesh aMesh, Vector3 aLocalPoint)
    {
        var verts = aMesh.vertices;
        var tris = aMesh.triangles;
        int triangleCount = tris.Length / 3;
        for (int i = 0; i < triangleCount; i++)
        {
            var V1 = verts[tris[i * 3]];
            var V2 = verts[tris[i * 3 + 1]];
            var V3 = verts[tris[i * 3 + 2]];
            var P = new Plane(V1, V2, V3);
            if (P.GetSide(aLocalPoint))
                return false;
        }
        return true;
    }

    #endregion
}
