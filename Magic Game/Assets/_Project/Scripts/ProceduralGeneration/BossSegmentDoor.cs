using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSegmentDoor : MonoBehaviour
{
    [SerializeField] private GameObject openDoor = null;
    [SerializeField] private GameObject closedDoor = null;

    private int crystalCount = 0;
    private int crystalsFromLevel = 0;

    private void Start()
    {
        crystalCount = GlobalVariables.crystalsCollected;
    }

    private void Update()
    {
        crystalsFromLevel = GlobalVariables.crystalsCollected;

        if (crystalsFromLevel >= (crystalCount + 3))
        {
            closedDoor.SetActive(false);
            openDoor.SetActive(true);
        }

        else
        {
            closedDoor.SetActive(true);
            openDoor.SetActive(false);
        }
    }
}