using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatController : MonoBehaviour
{
    public static float lives = 5;
    public static float bombs = 2;
    public static float damageLevel = 1.0f;
    public static float bombPoints = 0;
    public static float bombPointMax = 50;
    public static float graze = 0;

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
        //bombPointMax = 50;

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

        if (bombPoints >= bombPointMax)
        {
            bombPoints = 0;
            bombs += 1;
            GenerateBombsIcons();
            bombPointMax += 25;
        }
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
