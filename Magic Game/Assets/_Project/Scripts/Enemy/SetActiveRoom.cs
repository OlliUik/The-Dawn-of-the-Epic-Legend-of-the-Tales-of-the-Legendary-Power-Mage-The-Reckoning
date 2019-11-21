using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SetActiveRoom : MonoBehaviour
{

    public GameObject Room;
    public bool isDone;
       
   //Enable the room prefab
    void Start()
    {
        //Room.SetActive(true);
        genNavMesh();
    }

    public void genNavMesh()
    {
        GetComponent<NavMeshSurface>().BuildNavMesh();
        isDone = true;
    }

}
