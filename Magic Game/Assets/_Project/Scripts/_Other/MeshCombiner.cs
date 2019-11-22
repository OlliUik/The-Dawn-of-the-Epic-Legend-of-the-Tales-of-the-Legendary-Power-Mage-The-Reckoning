using UnityEngine;
//using System.Collections.Generic;

public class MeshCombiner : MonoBehaviour
{
    [System.Serializable]
    public struct MeshByMaterial
    {
        public Material material;
        public Mesh[] meshList;
        public Vector3[] position;
        public Quaternion[] rotation;
        public Vector3[] scale;

        public MeshByMaterial(Material mat, Mesh[] meshes, Vector3[] pos, Quaternion[] rot, Vector3[] scl)
        {
            material = mat;
            meshList = meshes;
            position = pos;
            rotation = rot;
            scale = scl;
        }
    }

    [SerializeField][Range(0, 65534)] private int maxVertexCount = 65534;
    [SerializeField] private MeshByMaterial[] meshByMaterials = null;
    [SerializeField] private GameObject[] newObjects = null;
    
    private void Start()
    {
        //Get all the meshes
        
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        MeshRenderer[] meshRenderers = new MeshRenderer[meshFilters.Length];

        //Get all the materials and initialize arrays

        meshByMaterials = new MeshByMaterial[50];

        for (int i = 0; i < meshFilters.Length; i++)
        {
            meshRenderers[i] = meshFilters[i].GetComponent<MeshRenderer>();

            foreach (Material mat in meshRenderers[i].sharedMaterials)
            {
                bool foundMaterial = false;
                foreach (MeshByMaterial mbm in meshByMaterials)
                {
                    if (mbm.material == mat)
                    {
                        foundMaterial = true;
                        break;
                    }
                }
                if (!foundMaterial)
                {
                    for (int j = 0; j < meshByMaterials.Length; j++)
                    {
                        if (meshByMaterials[j].material == null)
                        {
                            meshByMaterials[j].material = mat;
                            meshByMaterials[j].meshList = new Mesh[meshFilters.Length];
                            meshByMaterials[j].position = new Vector3[meshFilters.Length];
                            meshByMaterials[j].rotation = new Quaternion[meshFilters.Length];
                            meshByMaterials[j].scale = new Vector3[meshFilters.Length];
                            break;
                        }
                    }
                }
            }
        }

        //Create and assign all meshes with same material into lists
        
        for (int x = 0; x < meshFilters.Length; x++)
        {
            for (int y = 0; y < meshRenderers[x].sharedMaterials.Length; y++)
            {
                for (int z = 0; z < meshByMaterials.Length; z++)
                {
                    if (meshByMaterials[z].material == meshRenderers[x].sharedMaterials[y])
                    {
                        Mesh mesh = meshFilters[x].mesh.GetSubmesh(y);
                        mesh.name = meshFilters[x].mesh.name;

                        for (int w = 0; w < meshByMaterials[z].meshList.Length; w++)
                        {
                            if (meshByMaterials[z].meshList[w] == null)
                            {
                                meshByMaterials[z].meshList[w] = mesh;
                                meshByMaterials[z].position[w] = meshFilters[x].transform.position;
                                meshByMaterials[z].rotation[w] = meshFilters[x].transform.rotation;
                                meshByMaterials[z].scale[w] = meshFilters[x].transform.localScale;
                                break;
                            }
                        }
                    }
                }
            }
        }

        //Count the amount of meshes

        int meshCount = 0;

        for (int i = 0; i < meshByMaterials.Length; i++)
        {
            if (meshByMaterials[i].meshList != null)
            {
                foreach (Mesh mesh in meshByMaterials[i].meshList)
                {
                    if (mesh != null)
                    {
                        meshCount++;
                    }
                }
            }
        }

        //Create gameobjects for the meshes
        
        GameObject parent = new GameObject("Combined Meshes - " + gameObject.name);
        parent.transform.parent = null;
        newObjects = new GameObject[meshCount];

        int spawnCount = 0;

        foreach (MeshByMaterial mbm in meshByMaterials)
        {
            if (spawnCount >= meshCount)
            {
                break;
            }
            
            if (mbm.material != null)
            {
                int iterationCount = 0;

                GameObject materialObject = new GameObject(mbm.material.name);
                materialObject.transform.parent = parent.transform;

                foreach (Mesh mesh in mbm.meshList)
                {
                    if (mesh != null)
                    {
                        newObjects[spawnCount] = new GameObject(mesh.name);
                        newObjects[spawnCount].transform.position = mbm.position[iterationCount];
                        newObjects[spawnCount].transform.rotation = mbm.rotation[iterationCount];
                        newObjects[spawnCount].transform.localScale = mbm.scale[iterationCount];
                        newObjects[spawnCount].transform.parent = materialObject.transform;

                        MeshFilter filter = newObjects[spawnCount].AddComponent<MeshFilter>();
                        MeshRenderer renderer = newObjects[spawnCount].AddComponent<MeshRenderer>();

                        filter.mesh = mesh;
                        renderer.material = mbm.material;
                        
                        spawnCount++;
                        iterationCount++;
                    }
                }

                Debug.Log(this + " Iteration count: " + iterationCount + " Mesh count: " + meshCount);

                //Combine the meshes

                int vertexCount = 0;
                int currentIteration = 0;
                int iterated = 0;
                bool iterate = true;

                while (iterate)
                {
                    iterated = 0;
                    CombineInstance[] combine = new CombineInstance[iterationCount - currentIteration];

                    for (int i = 0; i < combine.Length; i++)
                    {
                        vertexCount += newObjects[spawnCount + currentIteration].GetComponent<MeshFilter>().mesh.vertexCount;
                        if (vertexCount >= maxVertexCount)
                        {
                            vertexCount = 0;
                            break;
                        }
                        combine[i].mesh = newObjects[spawnCount + currentIteration].GetComponent<MeshFilter>().mesh;
                        combine[i].transform = newObjects[spawnCount + currentIteration].transform.localToWorldMatrix;
                        currentIteration++;
                        iterated++;
                    }

                    CombineInstance[] combineCopy = new CombineInstance[iterated];

                    for (int i = 0; i < iterated; i++)
                    {
                        combineCopy[i] = combine[i];
                    }

                    GameObject finished = new GameObject("Final Mesh | " + currentIteration + " | " + mbm.material.name);
                    finished.transform.parent = parent.transform;

                    MeshFilter finishedFilter = finished.AddComponent<MeshFilter>();
                    MeshRenderer finishedRenderer = finished.AddComponent<MeshRenderer>();

                    finishedFilter.mesh = new Mesh();
                    finishedFilter.mesh.CombineMeshes(combineCopy, true, true, false);
                    finishedFilter.mesh.name = Random.Range(0, 9001).ToString();
                    finishedRenderer.material = mbm.material;
                    
                    if (currentIteration >= iterationCount)
                    {
                        iterate = false;
                    }
                }

                //Destroy the object holding temporary child objects
                Destroy(materialObject);
            }
        }

        //Disable the old & temporary objects

        //foreach (GameObject obj in newObjects)
        //{
        //    obj.SetActive(false);
        //}

        foreach (MeshRenderer rend in meshRenderers)
        {
            rend.enabled = false;
        }

        Debug.Log(this + " Meshes combined successfully!");
    }
}
