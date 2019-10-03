using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject a = null;
    [SerializeField] private GameObject b = null;

    //a = wall, Doorway-script
    //b = wall with hole

    private void Update()
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