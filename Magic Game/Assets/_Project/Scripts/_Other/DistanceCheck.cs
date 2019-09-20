using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DistanceCheck : MonoBehaviour
{
    [SerializeField]
    private float distance;

    [SerializeField]
    private GameObject[] childrenToHide;

    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        foreach (GameObject child in childrenToHide)
        {
            if (Vector3.Distance(player.transform.position, child.transform.position) < distance)
            {
                child.SetActive(true);
            }

            else
            {
               child.SetActive(false);
            }
        }
    }
}
