using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectSwitcher : MonoBehaviour
{
    [SerializeField]
    private GameObject a, b;

    //a = wall
    //b = doorway

    void Update()
    {
        if (a.activeInHierarchy)
        {
            b.SetActive(false);
        }

        if (!a.activeInHierarchy)
        {
            b.SetActive(true);
        }
    }
}