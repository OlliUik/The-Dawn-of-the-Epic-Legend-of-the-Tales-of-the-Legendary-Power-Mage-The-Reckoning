//using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PowerMage
{
    public class AnimatableModel : MonoBehaviour, IAnimatable
    {
        #region VARIABLES

        public GameObject model = null;
        public RuntimeAnimatorController controller = null;
        public Color color = Color.blue;
        public bool rotateToMoveDir = true;
        public float rotationLerp = 0.2f;
        public float animationSpeedMultiplier = 0.15f;
        public float animationBlendingMultiplier = 0.15f;

        public GameObject instantiatedModel { get; private set; } = null;
        public Animator animator { get; private set; } = null;
        public InverseKinematicsHandler ikHandler { get; private set; } = null;
        [HideInInspector] public Vector2 lookDirection = Vector2.zero;
        [HideInInspector] public Vector3 lookVector = Vector3.zero;
        [HideInInspector] public Vector3 lookPivot = Vector3.zero;
        
        private Vector3 _lookAtTarget = Vector3.zero;
        private Vector3 LookAtTarget
        {
            get { return _lookAtTarget; }
            set
            {
                _lookAtTarget = value;
                Vector3 dir = value - transform.position;
                lookAtDirection = new Vector2(dir.x, dir.z).normalized;
            }
        }
        private Vector2 lookAtDirection = Vector2.zero;
        private Vector2 moveDirection = Vector2.zero;
        
#if UNITY_EDITOR

        private struct WireMesh
        {
            public Mesh mesh;
            public Vector3 position;
            public Quaternion rotation;
            public Vector3 scale;

            public WireMesh(Mesh m, Vector3 p, Quaternion r, Vector3 s)
            {
                mesh = m;
                position = p;
                rotation = r;
                scale = s;
            }
        }

#endif

        #endregion

        #region INTERFACE_IMPLEMENTATION

        public void SetLookAt(Vector3 target)
        {
            LookAtTarget = target;
        }

        public void SetMovement(Vector2 velocity)
        {
            //Not implemented!
        }

        #endregion

        #region MONOBEHAVIOUR

        protected virtual void Awake()
        {
            instantiatedModel = Instantiate(model, transform.position, Quaternion.identity, transform);
            animator = instantiatedModel.GetComponent<Animator>();
            animator.runtimeAnimatorController = controller;
            ikHandler = animator.gameObject.AddComponent<InverseKinematicsHandler>();
        }

        protected virtual void Update()
        {
            Debug.DrawRay(transform.position, new Vector3(lookAtDirection.x, 0.0f, lookAtDirection.y), Color.cyan);


            float angle = 0.0f;

            if (rotateToMoveDir)
            {
                if (moveDirection.magnitude >= Mathf.Epsilon)
                {
                    angle = Vector2.SignedAngle(Vector2.up, moveDirection.normalized * (Vector2.down + Vector2.right)) + 180.0f;
                    instantiatedModel.transform.rotation = Quaternion.Lerp(instantiatedModel.transform.rotation, Quaternion.Euler(0.0f, angle, 0.0f), rotationLerp);
                }
            }
            else
            {
                angle = Vector2.SignedAngle(Vector2.up, lookDirection * (Vector2.down + Vector2.right)) + 180.0f;
                instantiatedModel.transform.rotation = Quaternion.Lerp(instantiatedModel.transform.rotation, Quaternion.Euler(0.0f, angle, 0.0f), rotationLerp);
            }

            float sin = Mathf.Sin(angle * Mathf.Deg2Rad);
            float cos = Mathf.Cos(angle * Mathf.Deg2Rad);

            float nx = moveDirection.x;
            float ny = moveDirection.y;

            Vector2 velocityRotated = new Vector2(
                cos * nx - sin * ny,
                sin * nx + cos * ny
                );

            animator.SetFloat("Movement Speed", (velocityRotated.magnitude) * animationSpeedMultiplier);
            animator.SetFloat("Movement Forward", velocityRotated.y * animationBlendingMultiplier);
            animator.SetFloat("Movement Right", velocityRotated.x * animationBlendingMultiplier);

            ikHandler.lookAtTarget = transform.position + lookPivot + lookVector;
        }
        
#if UNITY_EDITOR
        
        private GameObject currentModel = null;
        private List<WireMesh> wires = new List<WireMesh>();

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying)
            {
                if (currentModel != model)
                {
                    if (model != null)
                    {
                        wires.Clear();
                        GameObject go = Instantiate(model);
                        foreach (Transform t in go.transform.GetAllChildren())
                        {
                            if (t.GetComponent<MeshRenderer>() != null)
                            {
                                wires.Add(new WireMesh(t.GetComponent<MeshFilter>().sharedMesh, t.localPosition, t.localRotation, t.localScale));
                            }
                            else if (t.GetComponent<SkinnedMeshRenderer>() != null)
                            {
                                wires.Add(new WireMesh(t.GetComponent<SkinnedMeshRenderer>().sharedMesh, t.localPosition, t.localRotation, t.localScale));
                            }
                        }
                        DestroyImmediate(go);
                    }
                    currentModel = model;
                }

                if (model != null)
                {
                    foreach (WireMesh m in wires)
                    {
                        Gizmos.color = color;
                        Gizmos.DrawMesh(m.mesh, 0, transform.position + m.position, transform.rotation * m.rotation, transform.localScale + m.scale);
                    }
                }
            }
        }

        private void OnValidate()
        {
            currentModel = null;
        }

#endif

        #endregion

        #region CUSTOM_METHODS



        #endregion
    }
}
