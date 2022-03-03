using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public string stageSong;
    public int waveNumber;
    private float waveTimer;
    public Transform bossSpawn;
    public Transform enemyParent; // makes it easy to count enemies on screen so that its known when its safe to display dialogue
    public GameObject dialogueSystem;
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
        public BossProfile bossProfile;
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
        soundManager.Play(stageSong);
    }

    void Update()
    {
        if (gameRunning == true)
        {
            if (waves[waveNumber].bossWave == true)
            {
                if (dialogueSystem.activeSelf == false && bossSpawned == false && enemyParent.childCount == 0)
                {
                    soundManager.FadeOutSong(stageSong);
                    dialogueSystem.SetActive(true);

                    // tags first line in conversation to trigger boss fight after conversation
                    DialogueLine[] dialogue = waves[waveNumber].bossProfile.C1preBattleConversation;
                    dialogue[0].triggerFight = true;

                    // ! - CHANGE CONVERSATION TO REFLECT DIFFERENT PLAYER CHARACTERS
                    this.SendMessage("TypeThis", dialogue);
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

    public void SpawnBoss()
    {
        if (bossSpawned == false)
        {
            bossSpawned = true;
            GameObject newBoss = Instantiate(waves[waveNumber].bossPrefab);
            newBoss.transform.position = bossSpawn.position;
            newBoss.SendMessage("SetWaveSpawner", this.gameObject);

            soundManager.Play(waves[waveNumber].bossTheme);
        }
    }

    public void BossDeath()
    {
        soundManager.FadeOutSong(waves[waveNumber].bossTheme);
        dialogueSystem.SetActive(true);

        // ! - CHANGE CONVERSATION TO REFLECT DIFFERENT PLAYER CHARACTERS
        if (SceneManage.challenge)
        {
            this.SendMessage("TypeThis", waves[waveNumber].bossProfile.C1hardPostBattleConversation);
        }
        else
        {
            this.SendMessage("TypeThis", waves[waveNumber].bossProfile.C1easyPostBattleConversation);
        }
    }

    public void SpawnEnemies()
    {
        foreach(Enemy e in waves[waveNumber].enemiesList)
        {
            int dice = Random.Range(0, 100);

            if (dice <= e.chance)
            {
                GameObject newEnemy = Instantiate(e.enemyPrefab, enemyParent);

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
