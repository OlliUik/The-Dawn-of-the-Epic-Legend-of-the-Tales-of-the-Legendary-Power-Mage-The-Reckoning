using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnManager : MonoBehaviour
{   
    //For checking what it's doing right now.
    public enum SpawnState { SPAWNING, WAITING, COUNTING }

    
    //Configure each wave 
    [System.Serializable]
    public class Wave
    {
        public string name;                 //Name of the wave.
        public GameObject[] enemy;          //Type of enemies that you want to spawn.
        public int count;                   //Numbers of enemies in each wave.
        public float rate;                  //It can be spawn more than 1 enemies per spawnPoint.

        //Countdown between wave.
        public void setCount(int newCount)
        {
            this.count = newCount;
        }
    }

    public List<GameObject> spawnPoints ;            //List of spawnPoints. MUST BE ADDED MANUALLY.
    public Wave[] waves;                                                     //Numbers of waves.
    private int nextWave = 0;                                                //index for waves.
    [SerializeField] private float timeBetweenWaves = 5f;                    //As the name said.
    [SerializeField] private float secondsRate = 1f;                          //seconds between the spawning.
    public float waveCountdown;                                              

    public float crystalMultiplier = 0.3f;                                   //How much the health of enemies when the player pick a crystal.

    //Display count down time.
    public float WaveCountDown
    {
        get { return waveCountdown; }
    }

    private float searchCountdown = 1f;                                      //Search time before start counting down to the next wave.

    public SpawnState state = SpawnState.COUNTING;                           //Default state of the SpawnManager.
    private GameObject genbuilder;                                           //For getting the level generator automatically.                                       
    [SerializeField] private float increaseHealth = 0.2f;                    //Ratio of the health that will be increased next wave.
    public float increasedStat = 1.0f;                                       //For showing the ratio of the current enemies stat.
    [SerializeField] private float distance = 0.0f;                          //Distance that mesh renderer will be disabled when the player is too far. Originally disable their core too.
    [SerializeField] private float spawnDistanceLimit = 100.0f;              //Original used for getting spawnPoints only in the range of the spawnManager.    

    /*
    [SerializeField] private bool spawnDistanceIsRectangular = false;         
    [SerializeField] private float spawnDistanceLimitX = 0.0f;
    [SerializeField] private float spawnDistanceLimitY = 0.0f;
    [SerializeField] private float spawnDistanceLimitZ = 0.0f;
    */

    public List<EnemyCore> enemies;                                          //For checking enemies in a spawnManager.
    //public List<GameObject> closeSpawn;                                    //Keep all the closeSpawn   
    private GameObject player = null;                                        //Automatically get player component.
    private LevelGenerator gen;                                              //Get levelgenerator components from a gameobject.
    private bool gotSpawnPoint = false;                                      //Check whether it got all the desired spawnpoints yet.
    private bool gotCloseSpawn = false;                                      //Originally, It use to check the filter of nearby spawnPoints within a spawnmanager range.
    private bool isUpStaged = false;                                         //Check whether the all the wave has ended or not.

    // Start is called before the first frame update
    void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player");


        genbuilder = GameObject.FindGameObjectWithTag("levelbuilder");
        if (genbuilder != null)
        {
            gen = genbuilder.GetComponent<LevelGenerator>();
        }
        else
        {
            Debug.Log("No level generator");
        }
        spawnPoints = new List<GameObject>();
        waveCountdown = timeBetweenWaves;
    }

    // Update is called once per frame
    void Update()
    {
        //Gen the levelgenerator component.
        if (gen == null)
        {
            genbuilder = GameObject.FindGameObjectWithTag("levelbuilder");
            if (genbuilder != null)
            {
                gen = genbuilder.GetComponent<LevelGenerator>();
            }
            else
            {
                Debug.LogWarning("No level generator");
            }
        }
        else
        {
            if (!gotSpawnPoint && gen.isDone && !gotCloseSpawn)
            {
                /*
                spawnPoints.AddRange(GameObject.FindGameObjectsWithTag("spawnPoint"));
                */

                /*
                foreach(GameObject child in spawnPoints)
                {
                    //CheckDistance(child);
                    closeSpawn.Add(child);
                }
                */

                    foreach (Transform child in transform)
                    {
                       spawnPoints.Add(child.gameObject);
                    }
              
               
                gotSpawnPoint = true;
                gotCloseSpawn = true;

                if (spawnPoints.Count == 0)
                {
                    Debug.LogError("No spawn point referenced.");
                }
            }
            else if (gotSpawnPoint)
            {
                if (state == SpawnState.WAITING)
                {
                    //check if enemies are still alive
                    if (!EnemyIsAlive())
                    {
                        //Begin a new round
                        Debug.Log("Wave Completed!");
                        WaveCompleted();
                    }
                    else
                    {
                        isUpStaged = false;
                        //Debug.Log("Not dead yet.");
                        for (int i = enemies.Count - 1; i >= 0; i--)
                        {

                            if (gen != null)
                            {
                                if (gen.isDone)
                                {
                                    if (enemies[i] != null)
                                    {
                                        //spawn when player is close to enemies
                                        checkPlayerDistance(enemies[i]);
                                    }
                                    else
                                    {
                                        enemies.Remove(enemies[i]);
                                        Debug.Log("No enemy");
                                    }
                                }

                            }
                            else if (enemies[i] != null)
                            {
                                checkPlayerDistance(enemies[i]);
                            }
                            else
                            {
                                enemies.RemoveAt(i);
                                Debug.Log("No enemy");
                            }
                            
                        }
                        return;
                    }

                }

                if (waveCountdown <= 0)
                {
                    if (state != SpawnState.SPAWNING)
                    {
                        if (nextWave != 0)
                        {
                            increasedStat += increaseHealth;
                            StartCoroutine(SpawnWave(waves[nextWave]));
                        }
                        else
                        {
                            StartCoroutine(SpawnWave(waves[nextWave]));
                        }
                    }
                }
                else
                {
                    waveCountdown -= Time.deltaTime;
                }
                
            }
        
        }



    }

    /*
    private void CheckDistance(GameObject child)
    {
        if (!spawnDistanceIsRectangular)
        {
            if ((Vector3.Distance(transform.transform.position, child.transform.position) < spawnDistanceLimit))
            {
                closeSpawn.Add(child.gameObject);
            }
        }
        else
        {
            if( (Mathf.Abs(transform.position.x - child.transform.position.x) <= spawnDistanceLimitX) &&
                (Mathf.Abs(transform.position.y - child.transform.position.y) <= spawnDistanceLimitY) &&
                (Mathf.Abs(transform.position.z - child.transform.position.z) <= spawnDistanceLimitZ)
                )
            {
                closeSpawn.Add(child.gameObject);
            }
        }
    }
    */

    //Turn off mesh renderer when enemy is too far away.
    void checkPlayerDistance(EnemyCore child)
    {   
        
        if (Vector3.Distance(player.transform.position, child.transform.position) < distance)
        {
            // child.enabled = true;
            //child.gameObject.GetComponent<NavMeshAgent>().enabled = true;
            //child.gameObject.GetComponent<EnemyNavigation>().enabled = true;
            //child.gameObject.GetComponentInChildren<EnemyAnimations>().enabled = true;
            //child.gameObject.GetComponentInChildren<EnemyAnimations>().gameObject.GetComponent<Animator>().enabled = true;
            foreach (SkinnedMeshRenderer render in child.GetComponentsInChildren<SkinnedMeshRenderer>()) { render.enabled = true; }

        }

        else
        {   
            //child.gameObject.GetComponent<NavMeshAgent>().enabled = false;
            //child.gameObject.GetComponent<EnemyNavigation>().enabled = false;
            foreach (SkinnedMeshRenderer render in child.GetComponentsInChildren<SkinnedMeshRenderer>()) { render.enabled = false; }
            //child.gameObject.GetComponentInChildren<EnemyAnimations>().enabled = false;
            //child.gameObject.GetComponentInChildren<EnemyAnimations>().gameObject.GetComponent<Animator>().enabled = false;
            //child.enabled = false;
        }
        
    }

    //Check whether enemies are alive or not.
    bool EnemyIsAlive()
    {
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0f)
        {
            searchCountdown = 1f;
            if(enemies.Count == 0)
            {
                return false;
            }
        }
        return true;
    }


    //Spawn enemies in a wave.
    IEnumerator SpawnWave(Wave _wave)
    {
        Debug.Log("Spawning Wave: " + _wave.name);
        state = SpawnState.SPAWNING;

        for (int i = 0; i < _wave.count + EnemyGlobalVariables.extraEnemyAmount ; i++)
        {
            SpawnEnemy(_wave.enemy);
            yield return new WaitForSeconds(secondsRate / _wave.rate);
        }

        state = SpawnState.WAITING;
        yield break;
    }

    //Spawn an enemy with increased stat.
    void SpawnEnemy(GameObject[] _enemy)
    {   
        if(_enemy != null)
        {
            int randomRange = Random.Range(0, _enemy.Length);
            Debug.Log("Spawning Enemy:" + _enemy[randomRange].name);
            GameObject spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
            GameObject baddies = Instantiate(_enemy[randomRange], spawnPoint.transform.position, spawnPoint.transform.rotation);
            Health tempHealth = baddies.GetComponent<Health>();
            if (tempHealth != null)
            {
                Debug.Log("Crystal increase: " + GlobalVariables.crystalsCollected * crystalMultiplier);
                Debug.Log("Increased Stat: " + increasedStat);
                tempHealth.maxHealth += EnemyGlobalVariables.enemyExtraHealth;
                tempHealth.maxHealth *= (increasedStat + (GlobalVariables.crystalsCollected * crystalMultiplier) + (GlobalVariables.angryBaddiesPoint * crystalMultiplier));
                tempHealth.health = tempHealth.maxHealth;
            }
            enemies.Add(baddies.GetComponent<EnemyCore>());
        }
        else
        {
            Debug.Log("Add some enemies, will ya?");
        }
    }
       

    //Proceed to the next wave. or start looping.
    void WaveCompleted()
    {
        Debug.Log("Wave Completed");

        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;

        
        if (nextWave + 1 > waves.Length - 1)
        {
            //Do something when the wave is done
            nextWave = 0;
            if (!isUpStaged)
            {
                EnemyGlobalVariables.StageUp();
                isUpStaged = true;
                Debug.Log(EnemyGlobalVariables.enemyExtraHealth + " " + EnemyGlobalVariables.extraEnemyAmount);
            }
            Debug.Log("All Waves complete! Looping");
        }
        else
        {
        
            nextWave++;
        }
    }

    /*
    public virtual void OnDrawGizmos()
    {

     
        Gizmos.color = Color.blue;
        if (!spawnDistanceIsRectangular)
        {
            Gizmos.DrawWireSphere(transform.position, spawnDistanceLimit);
        }
        else
        {
            Gizmos.DrawLine(transform.position - new Vector3(spawnDistanceLimitX,0,0) , transform.position + new Vector3(spawnDistanceLimitX, 0, 0));
            Gizmos.DrawLine(transform.position - new Vector3(0, spawnDistanceLimitY, 0), transform.position + new Vector3(0, spawnDistanceLimitY, 0));
            Gizmos.DrawLine(transform.position - new Vector3(0, 0, spawnDistanceLimitZ), transform.position + new Vector3(0, 0, spawnDistanceLimitZ));
        }
       
    }
    */
}
