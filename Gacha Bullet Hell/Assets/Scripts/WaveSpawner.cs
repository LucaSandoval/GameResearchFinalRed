using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public int waveNumber;
    private float waveTimer;
    public Transform bossSpawn;
    public Wave[] waves;

    private float pulseTimer;
    private float pulseMax;

    public bool gameRunning;

    public Transform[] spawnZones;

    private bool bossSpawned;

    private SoundManager soundManager;


    [System.Serializable]
    public struct Wave
    {
        public bool bossWave;
        public string bossTheme;
        public GameObject bossPrefab;
        public float waveTime;
        public float spawnRate;
        public Enemy[] enemiesList;
    }

    [System.Serializable]
    public struct Enemy
    {
        public GameObject enemyPrefab;
        public float yMax;
        public float yMin;
        public int chance;
    }

    void Start()
    {
        gameRunning = true;
        soundManager = GetComponent<SoundManager>();
        waveNumber = 0;
        waveTimer = waves[waveNumber].waveTime;
        pulseMax = waves[waveNumber].spawnRate;
    }

    void Update()
    {
        if (gameRunning == true)
        {
            if (waves[waveNumber].bossWave == true)
            {
                if (bossSpawned == false)
                {
                    bossSpawned = true;
                    GameObject newBoss = Instantiate(waves[waveNumber].bossPrefab);
                    newBoss.transform.position = bossSpawn.position;

                    soundManager.Play(waves[waveNumber].bossTheme);
                }
            }
            else
            {
                if (waveTimer > 0)
                {
                    waveTimer -= Time.deltaTime;
                }
                else
                {
                    waveNumber += 1;
                    waveTimer = waves[waveNumber].waveTime;
                    pulseMax = waves[waveNumber].spawnRate;
                }

                if (pulseTimer > 0)
                {
                    pulseTimer -= Time.deltaTime;
                }
                else
                {
                    SpawnEnemies();
                    pulseTimer = pulseMax;
                }
            } 
        }
    }

    public void SpawnEnemies()
    {
        foreach(Enemy e in waves[waveNumber].enemiesList)
        {
            int dice = Random.Range(0, 100);

            if (dice <= e.chance)
            {
                GameObject newEnemy = Instantiate(e.enemyPrefab);

                int dice2 = Random.Range(0, 2);

                if (dice2 == 0)
                {
                    newEnemy.GetComponent<StandardEnemyAI>().direction = 1;
                    //Debug.Log("test");
                    newEnemy.transform.position = spawnZones[0].position;
                } else
                {
                    newEnemy.GetComponent<StandardEnemyAI>().direction = -1;
                    newEnemy.transform.position = spawnZones[1].position;
                }

                
                newEnemy.transform.position += new Vector3(0, Random.Range(e.yMin, e.yMax), 0);
            }
        }
    }
}
