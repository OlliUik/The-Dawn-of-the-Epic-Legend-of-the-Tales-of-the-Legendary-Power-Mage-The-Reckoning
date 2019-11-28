using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private bool isBreakable = false;
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

    private void OnCollisionEnter(Collision collision)
    {
        if (isBreakable && collision.gameObject.layer == 9)
        {
            chestRandomizer.crystalChests.Remove(gameObject.transform.parent.gameObject);
            gameObject.SetActive(false);
        }
    }
}
