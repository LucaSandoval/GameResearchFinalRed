using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManage : MonoBehaviour
{
    public static int level = 1;
    public static bool challenge = false;

    public static float globalPower = 1.0f;
    public static float globalBPoints = 0;
    public static float globalBPointMax = 50;
    public static float globalLives = 5;
    public static float globalBombs = 3;
    public static float globalGraze = 0;

    void NormalLevel()
    {
        challenge = false;
        SceneManager.LoadScene("Level");
    }

    void ChallengeLevel()
    {
        challenge = true;
        SceneManager.LoadScene("Level");
    }

    void EndLevel()
    {
        level++;
        SceneManager.LoadScene("DifficultySelect");
    }
}
