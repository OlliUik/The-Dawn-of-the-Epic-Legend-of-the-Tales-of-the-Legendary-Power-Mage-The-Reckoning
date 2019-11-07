using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlternativeSpawner : MonoBehaviour

{

    public enum SpawnState { SPAWNING, WAITING, COUNTING}

    [System.Serializable]
    public class Wave
    {
        public string name;
        public Transform [] enemy;
        public int count;
        public float rate;
    }

    public Transform[] spawnPoints;

    public Wave[] waves;
    private int nextWave = 0;

    public float timeBetweenWaves = 5f;
    public float waveCountdown;

    private float searchCountdown = 1f; 

    private SpawnState state = SpawnState.COUNTING;

    // Start is called before the first frame update
    void Start()
    {   
        if(spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn point referenced.");
        }
        waveCountdown = timeBetweenWaves;
    }

    // Update is called once per frame
    void Update()
    {   
        if(state == SpawnState.WAITING)
        {
            //check if enemies are still alive
            if(!EnemyIsAlive())
            {
                //Begin a new round
                Debug.Log("Wave Complete");
                
            }
            else
            {
                return;
            }
    
        }

        if(waveCountdown <= 0 )
        {
            if(state != SpawnState.SPAWNING)
            {
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
        }
        else
        {
            waveCountdown -= Time.deltaTime;
        }
    }

    bool EnemyIsAlive()
    {
        searchCountdown -= Time.deltaTime;
        if(searchCountdown <= 0f)
        {
            searchCountdown = 1f;
            if (GameObject.FindGameObjectsWithTag("Enemy") == null)
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

    void SpawnEnemy(Transform [] _enemy)
    {
        int randomRange = Random.Range(0, _enemy.Length);
        Debug.Log("Spawning Enemy:" +  _enemy[randomRange].name);
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Transform enemyWizard = Instantiate(_enemy[randomRange], spawnPoint.position, spawnPoint.rotation);
        enemyWizard.gameObject.SetActive(true);
    }

    void WaveCompleted()
    {
        Debug.Log("Wave Complete");

        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;

        if(nextWave + 1 > waves.Length - 1)
        {
            nextWave = 0;
            Debug.Log("All Waves complete! Looping");
        }
        else
        {

        }
        nextWave++;
    }
}
