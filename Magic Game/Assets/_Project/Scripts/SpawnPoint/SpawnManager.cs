﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnManager : MonoBehaviour
{
    public enum SpawnState { SPAWNING, WAITING, COUNTING }

    [System.Serializable]
    public class Wave
    {
        public string name;
        public GameObject[] enemy;
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
    [SerializeField] private float timeBetweenWaves = 5f;
    public float waveCountdown;

    public float crystalMultiplier = 0.3f;

    public float WaveCountDown
    {
        get { return waveCountdown; }
    }

    private float searchCountdown = 1f;

    public SpawnState state = SpawnState.COUNTING;
    public GameObject genbuilder;
    [SerializeField] private float increaseHealth = 0.2f;
    public float increasedStat = 1.0f;
    [SerializeField] private float distance = 0.0f;
    [SerializeField] private float spawnDistanceLimit = 100.0f;

    public List<EnemyCore> enemies;
    public List<GameObject> closeSpawn;
    private GameObject player = null;
    public LevelGenerator gen;
    private bool gotSpawnPoint = false;
    private bool gotCloseSpawn = false;

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


        
        waveCountdown = timeBetweenWaves;
    }

    // Update is called once per frame
    void Update()
    {

        if (gen == null)
        {
            genbuilder = GameObject.FindGameObjectWithTag("levelbuilder");
            if (genbuilder != null)
            {
                gen = genbuilder.GetComponent<LevelGenerator>();
            }
            else
            {
                Debug.Log("No level generator");
            }
        }
        else
        {
            if (!gotSpawnPoint && gen.isDone && !gotCloseSpawn)
            {   
                
                spawnPoints.AddRange(GameObject.FindGameObjectsWithTag("spawnPoint"));
                foreach(GameObject child in spawnPoints)
                {
                    if((Vector3.Distance(transform.transform.position, child.transform.position) < spawnDistanceLimit))
                    {
                        closeSpawn.Add(child.gameObject);
                    }
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


    void checkPlayerDistance(EnemyCore child)
    {
        //Debug.Log(Vector3.Distance(player.transform.position, child.transform.position));
        if (Vector3.Distance(player.transform.position, child.transform.position) < distance)
        {
            //Debug.Log(child.gameObject.name + " should be enabled");
            child.enabled = true;
            child.gameObject.GetComponent<NavMeshAgent>().enabled = true;
            child.gameObject.GetComponent<EnemyNavigation>().enabled = true;
            child.gameObject.GetComponentInChildren<EnemyAnimations>().enabled = true;
            child.gameObject.GetComponentInChildren<EnemyAnimations>().gameObject.GetComponent<Animator>().enabled = true;
            foreach (SkinnedMeshRenderer render in child.GetComponentsInChildren<SkinnedMeshRenderer>()) { render.enabled = true; }

        }

        else
        {   
            //Debug.Log(child.gameObject.name + " should be disabled");
            child.gameObject.GetComponent<NavMeshAgent>().enabled = false;
            child.gameObject.GetComponent<EnemyNavigation>().enabled = false;
            foreach (SkinnedMeshRenderer render in child.GetComponentsInChildren<SkinnedMeshRenderer>()) { render.enabled = false; }
            child.gameObject.GetComponentInChildren<EnemyAnimations>().enabled = false;
            child.gameObject.GetComponentInChildren<EnemyAnimations>().gameObject.GetComponent<Animator>().enabled = false;
            child.enabled = false;
        }
    }

    bool EnemyIsAlive()
    {
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0f)
        {
            searchCountdown = 1f;
            //if (GameObject.FindGameObjectWithTag("Enemy") == null)
            if(enemies.Count == 0)
            {
                return false;
            }
        }
        return true;
    }

    IEnumerator SpawnWave(Wave _wave)
    {
        Debug.Log("Spawning Wave: " + _wave.name);
        state = SpawnState.SPAWNING;

        for (int i = 0; i < _wave.count; i++)
        {
            SpawnEnemy(_wave.enemy);
            yield return new WaitForSeconds(1f / _wave.rate);
        }

        state = SpawnState.WAITING;
        yield break;
    }

    void SpawnEnemy(GameObject[] _enemy)
    {
        int randomRange = Random.Range(0, _enemy.Length);
        Debug.Log("Spawning Enemy:" + _enemy[randomRange].name);
        GameObject spawnPoint = closeSpawn[Random.Range(0, closeSpawn.Count)];
        GameObject baddies = Instantiate(_enemy[randomRange], spawnPoint.transform.position, spawnPoint.transform.rotation);
        Health tempHealth = baddies.GetComponent<Health>();
        if (tempHealth != null)
        {
            Debug.Log("Crystal increase: " + GlobalVariables.crystalsCollected * crystalMultiplier);
            Debug.Log("Increased Stat: " + increasedStat);
            tempHealth.maxHealth *= (increasedStat + (GlobalVariables.crystalsCollected * crystalMultiplier) + (GlobalVariables.angryBaddiesPoint * crystalMultiplier));
            tempHealth.health = tempHealth.maxHealth;
        }
        enemies.Add(baddies.GetComponent<EnemyCore>());
    }

    void WaveCompleted()
    {
        Debug.Log("Wave Completed");

        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;

        if (nextWave + 1 > waves.Length - 1)
        {
            nextWave = 0;
            Debug.Log("All Waves complete! Looping");
        }
        else
        {
            nextWave++;
        }
    }


    public virtual void OnDrawGizmos()
    {


        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, spawnDistanceLimit);

    }
}
