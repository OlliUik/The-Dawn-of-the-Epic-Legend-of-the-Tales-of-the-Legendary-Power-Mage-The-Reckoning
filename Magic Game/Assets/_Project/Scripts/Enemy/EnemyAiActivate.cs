using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAiActivate : MonoBehaviour

{

    public LevelGenerator builder;

    [SerializeField] GameObject enemiesGroup;

    public List<EnemyMagicRanged> enemies;
    // Start is called before the first frame update
    void Start()
    {
        builder.GetComponent<LevelGenerator>();

        foreach (Transform child in enemiesGroup.transform)
        {
            enemies.Add(child.GetComponent<EnemyMagicRanged>());
            Debug.Log(child.name.ToString());
        }
    }

    // Update is called once per frame
    void Update()
    {


        foreach (EnemyMagicRanged child in enemies)
        {
            if (builder.isDone)
            {
                if (child != null )
                        {
                            child.gameObject.SetActive(true);
                        }
                 }
            }
        
    }
}
