using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public GameObject[] levelObjects;
    public GameObject[] challengeLevelObjects;
    public Transform[] spawnZones;
    public Transform bossSpawn;
    public Transform enemyParent;
    public GameObject stageText;
    public Transform originPoint;
    public GameObject bgObjectParent;

    private WaveSpawner currentLevelScript;
    private BGController currentBGScript;
    private SoundManager soundManager;

    public GameObject dialogue;
    private Dialogue dialogueScript;

    public void Start()
    {
        soundManager = GetComponent<SoundManager>();
        dialogueScript = GetComponent<Dialogue>();
        

        //Each of this gameObject's children represent each level and difficulty.
        foreach (GameObject level in levelObjects)
        {
            level.SetActive(false);
        }

        //Figure out which of the children (levels) to enable, based on the current level
        //ID from SceneManage.css, and subtrack 1 to map it to the list of children (zero-indexed.)
        int levelID = SceneManage.level - 1;
        

        //As long as that level has a corresponding gameObject child, enable it. 
        if (levelID <= levelObjects.Length - 1)
        {
            if (SceneManage.challenge == false)
            {
                levelObjects[levelID].SetActive(true);
                currentLevelScript = levelObjects[levelID].GetComponent<WaveSpawner>();
                currentBGScript = levelObjects[levelID].GetComponent<BGController>();

                dialogueScript.waveObject = levelObjects[levelID];
            } else
            {
                challengeLevelObjects[levelID].SetActive(true);
                currentLevelScript = challengeLevelObjects[levelID].GetComponent<WaveSpawner>();
                currentBGScript = challengeLevelObjects[levelID].GetComponent<BGController>();

                dialogueScript.waveObject = challengeLevelObjects[levelID];
            }

            currentLevelScript.spawnZones = spawnZones;
            currentLevelScript.bossSpawn = bossSpawn;
            currentLevelScript.soundManager = soundManager;
            currentLevelScript.levelControlObj = this.gameObject;
            currentLevelScript.dialogueSystem = dialogue;
            currentLevelScript.enemyParent = enemyParent;
            currentLevelScript.stageText = stageText;
            currentBGScript.originPoint = originPoint;
            currentBGScript.bgObjectParent = bgObjectParent;
        }
    }
}
