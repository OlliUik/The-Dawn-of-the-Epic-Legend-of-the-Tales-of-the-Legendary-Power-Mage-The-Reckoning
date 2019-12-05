using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DistanceCheck : MonoBehaviour
{
    private List<GameObject> childrenToHide = null;            // List of the gameobject that will be hidden.
    private GameObject disableBlock;                           // A block for making NavMesh not generate on a certain area.
    private GameObject generator;                              // Get the generator gameobject automically.


    [SerializeField] private float spawnDistanceLimitX = 0.0f;                  // Checking X axis between player and a prefab that script want to hide.
    [SerializeField] private float spawnDistanceLimitY = 0.0f;                  // Checking Y axis between player and a prefab that script want to hide.
    [SerializeField] private float spawnDistanceLimitZ = 0.0f;                  // Checking Z axis between player and a prefab that script want to hide.

    private LevelGenerator gen;                                                 // Get levelgenerator component from the gameobject.
    private GameObject player = null;                                           // Get component of the player
    private bool meshCombiningDone = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        generator = GameObject.FindGameObjectWithTag("levelbuilder");
        foreach (Transform child in transform)
        {
            if (child.tag.Equals("disableBlock"))
            {
                disableBlock = child.gameObject;
            }
        }
        foreach (Transform child in transform)
        {
            if (child.gameObject.name != "Chests")
            {
                childrenToHide.Add(child.gameObject);
            }
        }

        gen = FindObjectOfType<LevelGenerator>();
        StartCoroutine(WaitForMeshCombining());
    }
    
    //Checking of the script visually.
    public virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position - new Vector3(spawnDistanceLimitX, 0, 0), transform.position + new Vector3(spawnDistanceLimitX, 0, 0));
        Gizmos.DrawLine(transform.position - new Vector3(0, spawnDistanceLimitY, 0), transform.position + new Vector3(0, spawnDistanceLimitY, 0));
        Gizmos.DrawLine(transform.position - new Vector3(0, 0, spawnDistanceLimitZ), transform.position + new Vector3(0, 0, spawnDistanceLimitZ));
    }


    private void Update()
    {
        if (generator != null && player != null && childrenToHide != null)
        {
            if (meshCombiningDone)
            {
                foreach (GameObject child in childrenToHide)
                {
                    if (gen.isDone)
                    {
                        CheckDistance(child);
                    }

                }
            }
        }
    }

    private void CheckDistance(GameObject child)
    {
        if ((Mathf.Abs(player.transform.position.x - child.transform.position.x) <= spawnDistanceLimitX) &&
          (Mathf.Abs(player.transform.position.y - child.transform.position.y) <= spawnDistanceLimitY) &&
          (Mathf.Abs(player.transform.position.z - child.transform.position.z) <= spawnDistanceLimitZ))
        {
            /*
            Debug.Log(transform.position);
            Debug.Log("------------------ " + Mathf.Abs(transform.position.x - child.transform.position.x) + "," + Mathf.Abs(transform.position.y - child.transform.position.y) + "," + Mathf.Abs(transform.position.z - child.transform.position.z));
            Debug.Log("++++++++++++++++++ " + spawnDistanceLimitX + "," + spawnDistanceLimitY + "," + spawnDistanceLimitZ);
            */
            child.SetActive(true);
            disableBlock.SetActive(false);
        }
        else
        {
            child.SetActive(false);
            disableBlock.SetActive(true);
        }

    }

    IEnumerator WaitForMeshCombining() 
    {
        while (!meshCombiningDone)
        {
            if (gen.isDone)
            {
                meshCombiningDone = true;
                foreach (GameObject child in childrenToHide)
                {
                    if (child.GetComponent<MeshCombiner>() != null)
                    {
                        if (!child.GetComponent<MeshCombiner>().isDone)
                        {
                            yield return null;
                            break;
                        }
                    }
                }
            }
            else 
            {
                yield return null;
            }
        }
    }

}
