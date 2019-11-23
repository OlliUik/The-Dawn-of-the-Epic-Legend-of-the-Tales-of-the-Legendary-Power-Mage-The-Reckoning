using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIActivateGen : MonoBehaviour

{

    public LevelGenerator builder = null;
    //public SetActiveRoom room;
    public List<EnemyMagicRanged> enemies = null;

    [SerializeField] GameObject enemiesGroup = null;
    [SerializeField] private float distance = 0.0f;


    private GameObject player = null;
    // Start is called before the first frame update
    void Start()
    {
        builder.GetComponent<LevelGenerator>();
        //room.GetComponent<SetActiveRoom>();
        player = GameObject.FindGameObjectWithTag("Player");


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
            //if (room.isDone)
            {
                if (child != null )
                {   
                    //spawn when player is close to enemies
                    if(Vector3.Distance(player.transform.position, child.transform.position) < distance)
                    {
                        child.gameObject.SetActive(true);
                    }
                }
            }
        }
        
    }
}
