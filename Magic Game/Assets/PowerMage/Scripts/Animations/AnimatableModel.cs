//using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PowerMage
{
    public class AnimatableModel : MonoBehaviour
    {
        public GameObject model = null;
        public RuntimeAnimatorController controller = null;

        [SerializeField] private bool blenderFix = true;

        private GameObject instantiatedModel = null;
        private Animator animator = null;

        private void Awake()
        {
            instantiatedModel = Instantiate(model, transform.position, blenderFix ? Quaternion.Euler(-90.0f, 0.0f, 0.0f) : Quaternion.identity, transform);
            animator = instantiatedModel.GetComponent<Animator>();
            animator.runtimeAnimatorController = controller;
        }
        
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
                        foreach (Transform t in go.transform)
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
                        Gizmos.color = Color.blue;
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

    }
}
