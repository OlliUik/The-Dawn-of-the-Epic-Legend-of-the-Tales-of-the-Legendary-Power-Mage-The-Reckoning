using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectSwitcher : MonoBehaviour
{
    [SerializeField]
    private GameObject a, b;

    //a = wall, Doorway-script
    //b = wall with doorway, no Doorway-script

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