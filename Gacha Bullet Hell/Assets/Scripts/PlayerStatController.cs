using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatController : MonoBehaviour
{
    public float lives;
    public float bombs;
    public float damageLevel;
    public float bombPoints;
    private float bombPointMax;
    public float graze;

    public Text powerText;
    public Text grazeText;
    public Text bombPointText;

    public GameObject livesParent;
    public GameObject bombsParent;
    private GameObject lifeIconRef;
    private GameObject bombIconRef;

    private List<GameObject> livesIconsList;
    private List<GameObject> bombsIconsList;


    public void Start()
    {
        //lives = 3;
        bombPointMax = 50;

        livesIconsList = new List<GameObject>();
        bombsIconsList = new List<GameObject>();

        lifeIconRef = Resources.Load<GameObject>("LifeIcon");
        bombIconRef = Resources.Load<GameObject>("BombIcon");
        GenerateLivesIcons();
        GenerateBombsIcons();
    }
    
    public void Update()
    {

        float dam = damageLevel;
        dam = Mathf.Round(dam * 10.0f) * 0.1f;
        powerText.text = "Power: " + dam;

        grazeText.text = "Graze: " + graze;

        bombPointText.text = "Point: " + bombPoints + "/" + bombPointMax;
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
            newLifeIcon.transform.localScale = new Vector2(1, 1);
        }
    }

    public void GenerateBombsIcons()
    {
        for (int i = 0; i < bombsIconsList.Count; i++)
        {
            Destroy(bombsIconsList[i]);
        }

        bombsIconsList = new List<GameObject>();

        for (int i = 0; i < bombs; i++)
        {
            GameObject newBombIcon = Instantiate(bombIconRef);
            newBombIcon.transform.SetParent(bombsParent.transform);
            bombsIconsList.Add(newBombIcon);
            newBombIcon.transform.localScale = new Vector2(1, 1);
        }
    }
}
