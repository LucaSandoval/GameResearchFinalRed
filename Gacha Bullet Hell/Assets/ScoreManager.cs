using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText;
    public static float score;
    public static float scoreToAdd;

    // Update is called once per frame
    void Update()
    {
        scoreText.text = score.ToString("00000000"); 
    }

    private void FixedUpdate()
    {
        if (scoreToAdd > 0)
        {
            score += 1;
            scoreToAdd -= 1;
        }
    }
}
