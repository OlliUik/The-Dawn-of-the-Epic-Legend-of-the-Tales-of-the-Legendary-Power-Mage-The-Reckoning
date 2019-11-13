using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestRandomizer : MonoBehaviour
{
    public GameObject crystalChest = null;
    public List<GameObject> chests = new List<GameObject>();

    [SerializeField] private int crystalChestCount = 0;
    [SerializeField] private List<GameObject> crystalChests = new List<GameObject>();
    private List<GameObject> tempList = new List<GameObject>();

    private void Start()
    {
        Shuffle(chests);
        Pick(crystalChestCount);
        Change();
    }

    private List<GameObject> Shuffle (List<GameObject> list)
    {
        System.Random rnd = new System.Random();
        GameObject current;
        
        int count = chests.Count;

        for (int i = 0; i < count; i++)
        {
            int r = i + (int)(rnd.NextDouble() * (count - i));
            current = list[r];
            list[r] = list[i];
            list[i] = current;
        }

        return list;
    }

    private void Pick(int amount)
    {
        int count = chests.Count;

        for (int i = 0; i < amount; i++)
        {
            GameObject current = chests[Random.Range(0, chests.Count)];
            tempList.Add(current);
            chests.Remove(current);
        }
    }

    private void Change()
    {
        foreach (GameObject chest in tempList)
        {
            if (crystalChest != null)
            {
                GameObject current = Instantiate(crystalChest, chest.transform.position, chest.transform.rotation, gameObject.transform);
                crystalChests.Add(current);
                Destroy(chest);
            }
        }

        tempList = null;
    }
}