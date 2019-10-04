﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAiActivate : MonoBehaviour

{

    public LevelGenerator builder;

    public GameObject enemy;
    // Start is called before the first frame update
    void Start()
    {
        builder.GetComponent<LevelGenerator>();
    }

    // Update is called once per frame
    void Update()
    {
       if(builder.isDone && enemy != null)
        {
            enemy.gameObject.SetActive(true);
        }
    }
}