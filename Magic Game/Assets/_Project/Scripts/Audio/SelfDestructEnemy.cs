using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestructEnemy : MonoBehaviour
{

    [SerializeField] EnemyCore enemy;
// Start is called before the first frame update
     void Start()
    {
        enemy = GetComponentInParent<EnemyCore>();
    }

    // Update is called once per frame
    void Update()
    {   
        if(enemy == null)
        {
            Destroy(gameObject, 4); //5 is how many seconds you want before the object deletes itself
        }
    }
}
