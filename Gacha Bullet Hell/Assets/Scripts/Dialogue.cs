using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Dialogue : MonoBehaviour
{
    public GameObject dialogueSystem;
    public GameObject textBox;
    public Text writtenText;
    public Text writtenName;
    public GameObject shownAvatar;

    public GameObject fadeToWhite;

    private string message;
    private Sprite avatar;
    private string charName;

    private float typeTime;
    private bool typing = false;
    private int lineIndex;
    private DialogueLine[] lines;


    [HideInInspector]
    public GameObject waveObject;

    public void TypeThis(DialogueLine[] targetLines)
    {
        lines = targetLines;
        message = lines[lineIndex].dialogueLine;
        avatar = lines[lineIndex].avatar;
        charName = lines[lineIndex].characterName;
        typeTime = 0.05f;

        for (int i = 44; i < message.Length; i--)
        {
            if (message[i] == ' ')
            {
                message = message.Remove(i, 1);
                message = message.Insert(i, "\n");
                i += 44;
            }
        }

        if (typing == false)
        {
            writtenText.text = "";
            textBox.SetActive(true);

            StartCoroutine(TypeText());
        }
    }

    private void Update()
    {
        if (dialogueSystem.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Z) && typing)
            {
                StopAllCoroutines();
                writtenText.text = message;
                typing = false;
            }
            else if (Input.GetKeyDown(KeyCode.Z) && !typing)
            {
                if(lineIndex < lines.Length - 1) // types next line
                {
                    lineIndex++;
                    TypeThis(lines);
                }
                else // quits dialogue
                {
                    QuitDialogue();
                }
            }
            //else if (Input.GetKeyDown(KeyCode.X))
            //{
            //    QuitDialogue();
            //}
        }
    }

    void QuitDialogue()
    {
        writtenText.text = "";

        // spawns boss if first line is tagged to do so
        if (lines[0].triggerFight)
        {
            waveObject.SendMessage("SpawnBoss");
        }
        else // ends level because its not the text before the battle
        {
            if (SceneManage.level == 6)
            {
                StartCoroutine(WinGame());
            }
            else
            {
                this.SendMessage("EndLevel");
            }
        }

        lineIndex = 0;
        dialogueSystem.SetActive(false);
    }

    //CREDIT TO http://wiki.unity3d.com/index.php/AutoType?_ga=2.28672252.1760231856.1570400781-1881874195.1512603304
    IEnumerator TypeText()
    {
        typing = true;

        int lineCounter = 0;
        float RealTypeTime;

        // change avatar
        shownAvatar.GetComponent<Image>().sprite = avatar;

        // change name
        writtenName.text = charName;

        // type message
        foreach (char c in message.ToCharArray())
        {
            writtenText.text += c;

            if (c == ',' || c == '.' || c == '?' || c == '!' || c == ':' || c == '(')
            {
                RealTypeTime = typeTime;
            }
            else
            {
                RealTypeTime = typeTime / 2;
                if (c == '\n')
                {
                    RealTypeTime = 0;
                    lineCounter++;
                }
            }
            yield return 0;
            yield return Wait(RealTypeTime);
        }
        typing = false;
    }

    IEnumerator Wait(float waitTime)
    {
        float counter = 0;

        while (counter < waitTime)
        {
            //Increment Timer until counter >= waitTime
            counter += Time.unscaledDeltaTime;
            //Wait for a frame so that Unity doesn't freeze
            yield return null;
        }
    }

    IEnumerator WinGame()
    {
        fadeToWhite.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        fadeToWhite.SetActive(true);
        for (int i = 0; i < 30; i++)
        {
            fadeToWhite.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, (i / 30f));
            yield return new WaitForSeconds(0.05f);
        }
        SceneManager.LoadScene("HappyEnding");
    }
}
