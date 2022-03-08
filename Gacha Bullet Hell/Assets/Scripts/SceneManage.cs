using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManage : MonoBehaviour
{
    public static int level = 1;
    public static bool challenge = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            PlayerStatController.damageLevel = 5.0f;
        }
    }

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
