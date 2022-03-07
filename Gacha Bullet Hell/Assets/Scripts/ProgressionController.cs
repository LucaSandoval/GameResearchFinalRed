using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressionController : MonoBehaviour
{
    public PlayerUnlock[] unlocks;
    public static bool[] playersUnlocked = new bool[3] { true, true, false };

    public static int selectedPlayer;

    public Text playerNameText;

    public Text livesText;
    public Text bombsText;
    public Text powerText;
    public Text bompPText;
    public Text grazeText;

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
