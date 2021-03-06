using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    public string stageSong;
    public string stageName;
    public int waveNumber;
    private float waveTimer;
    [HideInInspector]
    public Transform bossSpawn;
    [HideInInspector]
    public Transform enemyParent; // makes it easy to count enemies on screen so that its known when its safe to display dialogue
    
    [HideInInspector]
    public GameObject dialogueSystem;

    [HideInInspector]
    public GameObject levelControlObj;
    public Wave[] waves;

    private float pulseTimer;
    private float pulseMax;

    public bool gameRunning;

    [HideInInspector]
    public Transform[] spawnZones;

    private bool bossSpawned;

    [HideInInspector]
    public SoundManager soundManager;

    [HideInInspector]
    public GameObject stageText;


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
        //stageText = GameObject.Find("StageName");
        gameRunning = true;
        //soundManager = GetComponent<SoundManager>();
        waveNumber = 0;
        waveTimer = waves[waveNumber].waveTime;
        pulseMax = waves[waveNumber].spawnRate;
        soundManager.Play(stageSong);

        stageText.GetComponent<Text>().text = stageName;
        stageText.transform.GetChild(0).GetComponent<Text>().text = "Stage " + SceneManage.level;
        stageText.SetActive(true);

    }

    void Update()
    {
        if (gameRunning == true)
        {
            if (waves[waveNumber].bossWave == true)
            {
                if (dialogueSystem.activeSelf == false && bossSpawned == false && enemyParent.childCount == 0)
                {
                    //soundManager.FadeOutSong(stageSong);
                    dialogueSystem.SetActive(true);

                    DialogueLine[] dialogue;
                    switch (ProgressionController.selectedPlayer)
                    {
                        case 0:
                            dialogue = waves[waveNumber].bossProfile.C1preBattleConversation;
                            break;
                        case 1:
                            dialogue = waves[waveNumber].bossProfile.C2preBattleConversation;
                            break;
                        case 2:
                            dialogue = waves[waveNumber].bossProfile.C3preBattleConversation;
                            break;
                        case 3:
                            dialogue = waves[waveNumber].bossProfile.C4preBattleConversation;
                            break;
                        case 4:
                            dialogue = waves[waveNumber].bossProfile.C5preBattleConversation;
                            break;
                        case 5:
                            dialogue = waves[waveNumber].bossProfile.C6preBattleConversation;
                            break;

                        default:
                            dialogue = waves[waveNumber].bossProfile.C1preBattleConversation;
                            break;
                    }

                    // tags first line in conversation to trigger boss fight after conversation
                    dialogue[0].triggerFight = true;

                    levelControlObj.SendMessage("TypeThis", dialogue);
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

            soundManager.Pause(stageSong);
            soundManager.Play(waves[waveNumber].bossTheme);
        }
    }

    public void BossDeath()
    {
        soundManager.FadeOutSong(waves[waveNumber].bossTheme);
        dialogueSystem.SetActive(true);

        //Unlock proper character if boss defeated
        if (SceneManage.challenge == true)
        {
            ProgressionController.playersUnlocked[waves[waveNumber].bossProfile.playerUnlockID] = true;
        }

        if (SceneManage.challenge)
        {
            switch (ProgressionController.selectedPlayer)
            {
                case 0:
                    levelControlObj.SendMessage("TypeThis", waves[waveNumber].bossProfile.C1hardPostBattleConversation);
                    break;
                case 1:
                    levelControlObj.SendMessage("TypeThis", waves[waveNumber].bossProfile.C2hardPostBattleConversation);
                    break;
                case 2:
                    levelControlObj.SendMessage("TypeThis", waves[waveNumber].bossProfile.C3hardPostBattleConversation);
                    break;
                case 3:
                    levelControlObj.SendMessage("TypeThis", waves[waveNumber].bossProfile.C4hardPostBattleConversation);
                    break;
                case 4:
                    levelControlObj.SendMessage("TypeThis", waves[waveNumber].bossProfile.C5hardPostBattleConversation);
                    break;
                case 5:
                    levelControlObj.SendMessage("TypeThis", waves[waveNumber].bossProfile.C6hardPostBattleConversation);
                    break;

                default:
                    levelControlObj.SendMessage("TypeThis", waves[waveNumber].bossProfile.C1hardPostBattleConversation);
                    break;
            }
        }
        else
        {
            switch (ProgressionController.selectedPlayer)
            {
                case 0:
                    levelControlObj.SendMessage("TypeThis", waves[waveNumber].bossProfile.C1easyPostBattleConversation);
                    break;
                case 1:
                    levelControlObj.SendMessage("TypeThis", waves[waveNumber].bossProfile.C2easyPostBattleConversation);
                    break;
                case 2:
                    levelControlObj.SendMessage("TypeThis", waves[waveNumber].bossProfile.C3easyPostBattleConversation);
                    break;
                case 3:
                    levelControlObj.SendMessage("TypeThis", waves[waveNumber].bossProfile.C4easyPostBattleConversation);
                    break;
                case 4:
                    levelControlObj.SendMessage("TypeThis", waves[waveNumber].bossProfile.C5easyPostBattleConversation);
                    break;
                case 5:
                    levelControlObj.SendMessage("TypeThis", waves[waveNumber].bossProfile.C6easyPostBattleConversation);
                    break;

                default:
                    levelControlObj.SendMessage("TypeThis", waves[waveNumber].bossProfile.C1easyPostBattleConversation);
                    break;
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
