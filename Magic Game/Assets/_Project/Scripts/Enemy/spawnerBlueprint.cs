using UnityEngine;
using System.Collections;

/*
 *WaveSpawner spawn wave of enemy until the game end. 
 * @author Theeruth Borisuth ,Charin Tantrakul.
 */
public class spawnerBlueprint : MonoBehaviour
{

    public enum SpawnState
    {
        SPAWNING, WAITING, COUNTING
    };


    [System.Serializable]
    public class Wave
    {
        public string name;
        public Transform enemy;
        public int count;
        public float rate;

        public void setCount(int newCount)
        {
            this.count = newCount;
        }
    }

    public Wave waves;
    public int waveSize;
    private int enemySize = 1;
    private int nextWave = 0;
    public int NextWave
    {
        get { return nextWave + 1; }
    }

    public Transform[] spawnPoints;

    public float timeBetweenWaves = 5f;
    private float waveCountdown;

    public float WaveCountDown
    {
        get { return waveCountdown; }
    }

    private float searchCountdown = 1f;

    private SpawnState state = SpawnState.COUNTING;

    public SpawnState State
    {
        get { return state; }
    }

    /*
     * Define waveCountdown time.
     */
    void Start()
    {

        if (spawnPoints.Length == 0)
        {
            Debug.Log("No spawn points referenced");
        }

        waveCountdown = timeBetweenWaves;
    }

    /*
     * Check enemy in scene if no one survive start new wave.
     */
    void Update()
    {
        if (state == SpawnState.WAITING)
        {
            //Check if enemies are still alive
            if (!EnemyIsAlive())
            {
                //Begin a new round
                WaveCompleted();
            }
            else
            {
                return;
            }
        }

        if (waveCountdown <= 0)
        {
            if (state != SpawnState.SPAWNING)
            {
                // Start spawning wave
                StartCoroutine(SpawnWave(waves));
                if (nextWave + 1 <= waveSize)
                {
                    waves.setCount(waves.count + enemySize);
                }
            }
        }
        else
        {
            waveCountdown -= Time.deltaTime;
        }
    }

    /*
     * Start next wave.
     */
    void WaveCompleted()
    {
        Debug.Log("Wave Completed!");

        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;
        if (nextWave + 1 > waveSize)
        {
            nextWave = 0;
            Debug.Log("All wave Completed! Start Looping...");
        }
        else
        {
            nextWave++;
        }
    }

    /*
     * Check enemy in current wave there are still alive or not.
     */
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

    /*
     * Spawn enemy by receive number of enemy from Wave.count .
     */
    IEnumerator SpawnWave(Wave _wave)
    {
        Debug.Log("Spawning Wave: " + _wave.name);
        state = SpawnState.SPAWNING;

        //Spawn
        for (int i = 0; i < _wave.count; i++)
        {
            SpawnEnemy(_wave.enemy);
            yield return new WaitForSeconds(1f / _wave.rate);
        }

        state = SpawnState.WAITING;

        yield break;
    }

    /*
     * Create enemy's Transform.
     */
    void SpawnEnemy(Transform _enemy)
    {
        //Spawn enemy
        Debug.Log("Spawn Enemy: " + _enemy.name);

        Transform _sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(_enemy, _sp.position, _sp.rotation);
    }





}