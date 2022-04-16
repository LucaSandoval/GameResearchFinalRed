using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressionController : MonoBehaviour
{
    public PlayerUnlock[] unlocks;
    public static bool[] playersUnlocked = new bool[6] { true, true, true, true, true, true };

    public static int selectedPlayer = 4;

    public Text playerNameText;

    public Text livesText;
    public Text bombsText;
    public Text powerText;
    public Text bompPText;
    public Text grazeText;

    public Text playerDescriptionsText;
    public Image playerBigIcon;

    public Sprite[] playerBigIcons;
    [TextArea(3,10)]
    public string[] playerDescriptions;

    public void Start()
    {
        for (int i = 0; i < unlocks.Length; i++)
        {
            unlocks[i].unlocked = playersUnlocked[i];
        }

        livesText.text = "Lives: " + PlayerStatController.lives;
        bombsText.text = "Bombs: " + PlayerStatController.bombs;
        powerText.text = "Power: " + PlayerStatController.damageLevel.ToString("0.0");
        bompPText.text = "Point: " + PlayerStatController.bombPoints + "/" + PlayerStatController.bombPointMax;
        grazeText.text = "Graze: " + PlayerStatController.graze;
    }

    public void Update()
    {
        for (int i = 0; i < unlocks.Length; i++)
        {
            if (selectedPlayer == i)
            {
                unlocks[i].selected = true;
                playerNameText.text = unlocks[i].playerName;
                playerDescriptionsText.text = playerDescriptions[i];
                playerBigIcon.sprite = playerBigIcons[i];
            } else
            {
                unlocks[i].selected = false;
            }
        }
    }

    public void SetSelection(int id)
    {
        if (unlocks[id].unlocked == true)
        {
            selectedPlayer = id;
        }
    }
}
