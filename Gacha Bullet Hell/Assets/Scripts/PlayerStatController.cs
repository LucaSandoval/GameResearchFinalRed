using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatController : MonoBehaviour
{
    public float lives;
    public float damageLevel;
    public float graze;

    public Text powerText;
    public Text grazeText;

    public GameObject livesParent;
    private GameObject lifeIconRef;

    private List<GameObject> livesIconsList;

    public void Start()
    {
        //lives = 3;

        livesIconsList = new List<GameObject>();

        lifeIconRef = Resources.Load<GameObject>("LifeIcon");
        GenerateLivesIcons();
    }
    
    public void Update()
    {

        float dam = damageLevel;
        dam = Mathf.Round(dam * 10.0f) * 0.1f;
        powerText.text = "Power: " + dam;

        grazeText.text = "Graze: " + graze;
    }

    public void GenerateLivesIcons()
    {
        for (int i = 0; i < livesIconsList.Count; i++)
        {
            Destroy(livesIconsList[i]);
        }

        livesIconsList = new List<GameObject>();

        for (int i = 0; i < lives; i++)
        {
            GameObject newLifeIcon = Instantiate(lifeIconRef);
            newLifeIcon.transform.SetParent(livesParent.transform);
            livesIconsList.Add(newLifeIcon);
        }
    }
}
