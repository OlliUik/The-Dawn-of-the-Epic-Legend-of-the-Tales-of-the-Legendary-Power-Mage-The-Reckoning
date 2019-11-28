using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestRandomizer : MonoBehaviour
{
    //DISABLE ALWAYS BEFORE PLAY!!!

    #region VARIABLES

    public GameObject normalChest = null;
    public GameObject crystalChest = null;
    public List<GameObject> chests = new List<GameObject>();
    public List<GameObject> crystalChests = new List<GameObject>();
    public bool isSearching = false;

    [SerializeField] private int crystalChestCount = 6;
    private LevelGenerator generator = null;
    private GenerationLoop loop = null;
    private List<GameObject> tempList = new List<GameObject>();
    private int crystalCount = 0;
    private int crystalsFromLevel = 0;
    [SerializeField] private float timer = 1f;

    #endregion

    #region UNITY_FUNCTIONS

    private void Start()
    {
        crystalCount = GlobalVariables.crystalsCollected;
        loop = GetComponent<GenerationLoop>();
        generator = FindObjectOfType<LevelGenerator>();
    }

    private void Update()
    {
        if (generator == null)
        {
            generator = FindObjectOfType<LevelGenerator>();
        }

        if (loop.isGenerating)
        {
            crystalChests.Clear();
            chests.Clear();
        }

        if (isSearching)
        {
            timer -= Time.deltaTime;

            if (timer < 0)
            {
                Shuffle(chests);

                if (chests.Count >= crystalChestCount)
                {
                    Pick(crystalChestCount);
                }

                Change();
                isSearching = false;
            }
        }

        else
        {
            timer = 1f;
        }

        crystalsFromLevel = GlobalVariables.crystalsCollected;

        if (crystalsFromLevel >= (crystalCount + 3))
        {
            ChangeBack();
        }
    }

    #endregion

    #region CUSTOM_FUNCTIONS

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

        tempList.Clear();
    }

    private void ChangeBack()
    {
        foreach (GameObject chest in crystalChests)
        {
            GameObject current = Instantiate(normalChest, chest.transform.position, chest.transform.rotation, gameObject.transform);
            current.SetActive(chest.activeInHierarchy);
            chests.Add(current);
            Destroy(chest);
        }

        crystalChests.Clear();
    }

    #endregion
}