using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RunTimeGenTest : MonoBehaviour
{

    public GameObject Room;
    public bool isDone;
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(buildNavMesh());
        Room.SetActive(true);
    }


}
