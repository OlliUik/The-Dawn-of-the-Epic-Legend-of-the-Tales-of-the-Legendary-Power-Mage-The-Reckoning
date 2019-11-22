using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAiActivate : MonoBehaviour

{

    public GameObject room;
    //public GameObject enemiesGroup;
    public List<GameObject> enemies;
    [SerializeField] private float distance = 0.0f;

    private LevelGenerator gen;

    private GameObject player = null;
    // Start is called before the first frame update
    void Start()
    {
        room = GameObject.FindGameObjectWithTag("levelbuilder");
        if(room != null)
        {
            gen = room.GetComponent<LevelGenerator>();
        }
        else
        {
            Debug.Log("No spawning");
        }
        player = GameObject.FindGameObjectWithTag("Player");
        enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        
    }

    public virtual void OnDrawGizmos()
    {

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distance);

    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject child in enemies)
        {
            //if (builder.isDone)
            if (gen.isDone && room != null)
            {
                if (child != null )
                {   
                    //spawn when player is close to enemies
                    if(Vector3.Distance(player.transform.position, child.transform.position) < distance)
                    {
                        child.gameObject.SetActive(true);
                    }

                    else
                    {
                        child.gameObject.SetActive(false);

                    }
                }
            }
            else if (child != null && room == null)             
             {
                    //spawn when player is close to enemies
                    if (Vector3.Distance(player.transform.position, child.transform.position) < distance)
                    {
                        child.gameObject.SetActive(true);
                    }

                    else
                    {
                        child.gameObject.SetActive(false);

                    }
             }
        }
        
    }
}
