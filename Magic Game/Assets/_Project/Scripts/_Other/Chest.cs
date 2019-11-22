using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private ChestRandomizer chestRandomizer = null;
    private LevelGenerator levelGenerator = null;

    private void Start()
    {
        levelGenerator = FindObjectOfType<LevelGenerator>();
        chestRandomizer = FindObjectOfType<ChestRandomizer>();
    }

    private void Update()
    {
        if (chestRandomizer.isSearching)
        {
            chestRandomizer.chests.Add(gameObject);
            Destroy(this);
        }
    }
}
