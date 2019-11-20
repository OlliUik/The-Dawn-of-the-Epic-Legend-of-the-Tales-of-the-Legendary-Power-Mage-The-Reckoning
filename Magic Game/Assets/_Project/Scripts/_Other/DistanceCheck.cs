using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DistanceCheck : MonoBehaviour
{
    [SerializeField] private float distance = 0.0f;
    [SerializeField] private GameObject[] childrenToHide = null;
    [SerializeField] private NavMeshSurface surface;
    [SerializeField] private GameObject disableBlock;
    [SerializeField] private LevelGenerator gen;

    private GameObject player = null;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {   
        
        foreach (GameObject child in childrenToHide)
        {   
            if(gen.isDone)
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
