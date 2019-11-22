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

    public virtual void OnDrawGizmos()
    {

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distance);

    }


    private void Update()
    {   
        if(generator != null && player != null && childrenToHide != null)
        {
            foreach (GameObject child in childrenToHide)
            {
                if (gen.isDone)
                {
                    if (Vector3.Distance(player.transform.position, child.transform.position) < distance)
                    {
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
        }
      
       
    }
}
