using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public enum SpawnState { SPAWNING, WAITING, COUNTING}

    [System.Serializable]
    public class Wave
    {
        public string name;
        public GameObject [] enemy;
        public int count;
        public float rate;


        public void setCount(int newCount)
        {
            this.count = newCount;
        }
    }

    public List<GameObject> spawnPoints = new List<GameObject>();
    public Wave[] waves;
    private int nextWave = 0;
    public float timeBetweenWaves = 5f;
    public float waveCountdown;

    public float crystalMultiplier = 0.3f;

    public float WaveCountDown
    {
        get { return waveCountdown; }
    }

    private float searchCountdown = 1f; 

    private SpawnState state = SpawnState.COUNTING;
    public GameObject genbuilder;
    [SerializeField] private float increaseHealth = 0.2f;
    public float increasedStat = 1.0f;
    [SerializeField] private float distance = 0.0f;
    public List<GameObject> enemies;
    private GameObject player = null;
    private LevelGenerator gen;



    // Start is called before the first frame update
    void Start()
    {

        spawnPoints.AddRange(GameObject.FindGameObjectsWithTag("spawnPoint"));

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


        if (spawnPoints.Count == 0)
        {
            Debug.LogError("No spawn point referenced.");
        }
        waveCountdown = timeBetweenWaves;
    }

    // Update is called once per frame
    void Update()
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
                //Debug.Log("Not dead yet.");
                return;
            }

        }

        if (waveCountdown <= 0)
        {
            if (state != SpawnState.SPAWNING)
            { if (nextWave != 0)
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


        foreach (GameObject child in enemies)
        {
            /*
            if (gen != null)
            {
              if(gen.isDone)
                {
                    if (child != null)
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
            */
            //if(child != null)
            /*
            if (Resources.FindObjectsOfTypeAll<EnemyCore>() != null)
            {
                if (child != null)
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
            */
        }

    }

    bool EnemyIsAlive()
    {
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0f)
        {
            searchCountdown = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                return false;
            }
        }       
        return true;
    }

    IEnumerator SpawnWave(Wave _wave)
    {
        Debug.Log("Spawning Wave: "+ _wave.name);
        state = SpawnState.SPAWNING;

        for (int i = 0; i < _wave.count; i++)
        {   
            SpawnEnemy(_wave.enemy);
            yield return new WaitForSeconds(1f / _wave.rate);
        }

        state = SpawnState.WAITING;
        yield break;
    }

    void SpawnEnemy(GameObject [] _enemy)
    {
        int randomRange = Random.Range(0, _enemy.Length);
        Debug.Log("Spawning Enemy:" +  _enemy[randomRange].name);
        GameObject spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        GameObject baddies = Instantiate(_enemy[randomRange], spawnPoint.transform.position, spawnPoint.transform.rotation);
        Health tempHealth = baddies.GetComponent<Health>();
        if(tempHealth != null )
        {
            Debug.Log("Crystal increase: " + GlobalVariables.crystalsCollected * crystalMultiplier);
            Debug.Log("Increased Stat: " + increasedStat);
            tempHealth.maxHealth *= (increasedStat + (GlobalVariables.crystalsCollected*crystalMultiplier) + (GlobalVariables.angryBaddiesPoint*crystalMultiplier));
            tempHealth.health = tempHealth.maxHealth;
        }
        enemies.Add(baddies);
    }

    void WaveCompleted()
    {
        Debug.Log("Wave Completed");

        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;

        if(nextWave + 1 > waves.Length - 1)
        {
            nextWave = 0;
            Debug.Log("All Waves complete! Looping");
        }
        else
        {
            nextWave++;
        }      
    }
}
