using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    [HideInInspector] public bool isDone = false;

    [Tooltip("Increase this value, if you have more materials being used.")]
    [SerializeField] private int materialTempCount = 50;
    [SerializeField][Range(0, 65534)] private int maxVertexCount = 65534;
    [SerializeField] private bool generateToSceneRoot = false;

    private LevelGenerator generator = null;

    private void Awake()
    {
        GameObject[] go = GameObject.FindGameObjectsWithTag("levelbuilder");

        if (go != null)
        {
            foreach (GameObject g in go) 
            {
                if (g.activeInHierarchy)
                {
                    generator = g.GetComponent<LevelGenerator>();
                    if (generator != null) 
                    {
                        break;
                    }
                }
            }
        }

        if (generator != null)
        {
            //Debug.Log("Starting coroutine WaitForLevelGeneration.");
            StartCoroutine(WaitForLevelGeneration());
        }
        else 
        {
            Debug.LogError(this + " Couldn't find LevelGenerator script!");
        }
    }

    IEnumerator WaitForLevelGeneration() 
    {
        while (!generator.isDone)
        {
            //Debug.Log("Waiting while generation has finished...");
            yield return null;
        }
        //Debug.Log("Can now combine meshes!");
        CombineMeshes();
        yield break;
    }

    private void OnDestroy()
    {
        //Debug.Log("GameObject being destroyed, stopping coroutines...");
        StopAllCoroutines();
    }

    private void CombineMeshes()
    {
        //Get all the meshes
        
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        MeshRenderer[] meshRenderers = new MeshRenderer[meshFilters.Length];

        //Get all the materials and initialize arrays

        MeshByMaterial[] meshByMaterials = new MeshByMaterial[materialTempCount];

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

        //Copy the previous array's contents to a trimmed (smaller) array

        int materialCount = 0;

        foreach (MeshByMaterial mbm in meshByMaterials)
        {
            if (mbm.material != null)
            {
                materialCount++;
            }
        }

        MeshByMaterial[] meshByMaterialsCopy = new MeshByMaterial[materialCount];

        for (int i = 0; i < meshByMaterialsCopy.Length; i++)
        {
            meshByMaterialsCopy[i].material = meshByMaterials[i].material;
            int mCount = 0;

            foreach (Mesh m in meshByMaterials[i].meshList)
            {
                if (m != null)
                {
                    mCount++;
                }
            }

            meshByMaterialsCopy[i].meshList = new Mesh[mCount];
            meshByMaterialsCopy[i].position = new Vector3[mCount];
            meshByMaterialsCopy[i].rotation = new Quaternion[mCount];
            meshByMaterialsCopy[i].scale = new Vector3[mCount];

            for (int j = 0; j < mCount; j++)
            {
                meshByMaterialsCopy[i].meshList[j] = meshByMaterials[i].meshList[j];
                meshByMaterialsCopy[i].position[j] = meshByMaterials[i].position[j];
                meshByMaterialsCopy[i].rotation[j] = meshByMaterials[i].rotation[j];
                meshByMaterialsCopy[i].scale[j] = meshByMaterials[i].scale[j];
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
        GameObject[] newObjects = new GameObject[meshCount];
        GameObject[] materialObjects = new GameObject[meshByMaterialsCopy.Length];

        int spawnCount = 0;

        for (int x = 0; x < meshByMaterialsCopy.Length; x++)
        {
            MeshByMaterial mbm = meshByMaterialsCopy[x];
            
            GameObject[] reservedObjects = new GameObject[mbm.meshList.Length];
            for (int j = 0; j < reservedObjects.Length; j++)
            {
                reservedObjects[j] = newObjects[j + spawnCount];
            }
            spawnCount += reservedObjects.Length;

            materialObjects[x] = new GameObject(mbm.material.name);
            materialObjects[x].transform.parent = parent.transform;

            for (int y = 0; y < reservedObjects.Length; y++)
            {
                if (mbm.meshList[y] != null)
                {
                    reservedObjects[y] = new GameObject(mbm.meshList[y].name);
                    reservedObjects[y].transform.position = mbm.position[y];
                    reservedObjects[y].transform.rotation = mbm.rotation[y];
                    reservedObjects[y].transform.localScale = mbm.scale[y];
                    reservedObjects[y].transform.parent = materialObjects[x].transform;

                    MeshFilter filter = reservedObjects[y].AddComponent<MeshFilter>();
                    MeshRenderer renderer = reservedObjects[y].AddComponent<MeshRenderer>();

                    filter.mesh = mbm.meshList[y];
                    renderer.material = mbm.material;
                }
            }

            for (int y = 0; x < newObjects.Length; y++)
            {
                if (newObjects[y] == null)
                {
                    for (int z = 0; z < reservedObjects.Length; z++)
                    {
                        newObjects[y + z] = reservedObjects[z];
                    }
                    break;
                }
            }
        }

        //Combine meshes

        spawnCount = 0;

        for (int x = 0; x < meshByMaterialsCopy.Length; x++)
        {
            int iteration = 0;
            int vertexCount = 0;

            List<List<GameObject>> objects = new List<List<GameObject>>();

            for (int y = 0; y < meshByMaterialsCopy[x].meshList.Length; y++)
            {
                vertexCount += meshByMaterialsCopy[x].meshList[y].vertexCount;
                if (vertexCount >= maxVertexCount)
                {
                    vertexCount = 0;
                    iteration++;
                }
                if (objects.Count < iteration + 1)
                {
                    objects.Add(new List<GameObject>());
                }
                objects[iteration].Add(newObjects[spawnCount]);
                spawnCount++;
            }

            //foreach (List<GameObject> list in objects)
            //{
            //    Debug.Log("Material: " + meshByMaterialsCopy[x].material + " List length: " + list.Count);
            //}

            for (int y = 0; y < objects.Count; y++)
            {
                CombineInstance[] combine = new CombineInstance[objects[y].Count];

                int z = 0;
                foreach (GameObject go in objects[y])
                {
                    MeshFilter filter = go.GetComponent<MeshFilter>();
                    combine[z].mesh = filter.mesh;
                    combine[z].transform = go.transform.localToWorldMatrix;
                    z++;
                }

                GameObject finished = new GameObject("Final Mesh | " + z + " | " + meshByMaterialsCopy[x].material.name);
                finished.transform.parent = parent.transform;

                MeshFilter finishedFilter = finished.AddComponent<MeshFilter>();
                MeshRenderer finishedRenderer = finished.AddComponent<MeshRenderer>();

                finishedFilter.mesh = new Mesh();
                finishedFilter.mesh.CombineMeshes(combine, true, true, false);
                finishedFilter.mesh.name = Random.Range(0, 9001).ToString();
                finishedRenderer.material = meshByMaterialsCopy[x].material;
            }
        }

        //Disable old objects & destroy temporary objects

        foreach (GameObject obj in materialObjects)
        {
            Destroy(obj);
        }

        foreach (MeshRenderer rend in meshRenderers)
        {
            rend.enabled = false;
        }

        //Move gameobjects under script's transform if "generateToSceneRoot" is false

        if (!generateToSceneRoot) 
        {
            for (int i = parent.transform.childCount - 1; i >= 0; i--)
            {
                parent.transform.GetChild(i).parent = transform;
            }

            //foreach (Transform tf in parent.transform)
            //{
            //    tf.parent = transform;
            //}

            Destroy(parent);
        }

        isDone = true;
        //Debug.Log(this + " Meshes combined successfully!");
    }
}
