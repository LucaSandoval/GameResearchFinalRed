using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText;
    public static float score;
    public static float scoreToAdd;

    private PlayerStatController stat;

    public static lifeBonus[] lifeBonuses = new lifeBonus[6] 
    { new lifeBonus(1000, false), new lifeBonus(3000, false), new lifeBonus(5000, false),
    new lifeBonus(7000, false), new lifeBonus(9000, false), new lifeBonus(11000, false)};

    public void Start()
    {
        stat = GameObject.Find("Player").GetComponent<PlayerStatController>();
    }

    [System.Serializable]
    public struct lifeBonus
    {

        public float pointValue;
        public bool unlocked;

        public lifeBonus(float point, bool unlocked)
        {
            this.pointValue = point;
            this.unlocked = unlocked;
        } 
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = score.ToString("000000"); 

        for (int i = 0; i < lifeBonuses.Length; i++)
        {
            if (lifeBonuses[i].unlocked == false && score >= lifeBonuses[i].pointValue)
            {
                PlayerStatController.lives += 1;
                stat.GenerateLivesIcons();
                lifeBonuses[i].unlocked = true;
                Debug.Log("life added");
            }
        }
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
