using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DistanceCheck : MonoBehaviour
{
    [SerializeField] private float distance = 0.0f;
    [SerializeField] private List<GameObject> childrenToHide = null;
    //[SerializeField] private NavMeshSurface surface;
    [SerializeField] private GameObject disableBlock;
    [SerializeField] private GameObject generator;

    
    [SerializeField] private float spawnDistanceLimitX = 0.0f;
    [SerializeField] private float spawnDistanceLimitY = 0.0f;
    [SerializeField] private float spawnDistanceLimitZ = 0.0f;
  

    private LevelGenerator gen;

    private GameObject player = null;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        generator = GameObject.FindGameObjectWithTag("levelbuilder");
        foreach (Transform child in transform)
        {
            if(child.tag.Equals("disableBlock"))
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
    }

    /*
    public virtual void OnDrawGizmos()
    {

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distance);

    }
    */

    public virtual void OnDrawGizmos()
    {

        
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position - new Vector3(spawnDistanceLimitX,0,0) , transform.position + new Vector3(spawnDistanceLimitX, 0, 0));
        Gizmos.DrawLine(transform.position - new Vector3(0, spawnDistanceLimitY, 0), transform.position + new Vector3(0, spawnDistanceLimitY, 0));
        Gizmos.DrawLine(transform.position - new Vector3(0, 0, spawnDistanceLimitZ), transform.position + new Vector3(0, 0, spawnDistanceLimitZ));
       
      

    }

    private void Update()
    {   
        if(generator != null && player != null && childrenToHide != null)
        {
            foreach (GameObject child in childrenToHide)
            {
                if (gen.isDone)
                {
                    //if (Vector3.Distance(player.transform.position, child.transform.position) < distance)
                    CheckDistance(child);
                    /*
                    {
                        child.SetActive(true);
                        disableBlock.SetActive(false);
                    }
                    else
                    {
                        child.SetActive(false);
                        disableBlock.SetActive(true);
                    }
                    */
                }

            }
        }
      
       
    }


   
   private void CheckDistance(GameObject child)
   {
        if((Mathf.Abs(player.transform.position.x - child.transform.position.x) <= spawnDistanceLimitX) &&
          (Mathf.Abs(player.transform.position.y - child.transform.position.y) <= spawnDistanceLimitY) &&
          (Mathf.Abs(player.transform.position.z - child.transform.position.z) <= spawnDistanceLimitZ))
       {
            Debug.Log(transform.position);
            Debug.Log("------------------ " + Mathf.Abs(transform.position.x - child.transform.position.x) + "," + Mathf.Abs(transform.position.y - child.transform.position.y) + "," + Mathf.Abs(transform.position.z - child.transform.position.z));
            Debug.Log("++++++++++++++++++ " + spawnDistanceLimitX + "," + spawnDistanceLimitY + "," + spawnDistanceLimitZ);
            child.SetActive(true);
            disableBlock.SetActive(false);
       }
        else
        {
            child.SetActive(false);
            disableBlock.SetActive(true);
        }
       
   }
  
}
