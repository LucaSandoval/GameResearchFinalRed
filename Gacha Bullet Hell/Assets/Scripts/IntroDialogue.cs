using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroDialogue : MonoBehaviour
{
    public Text writtenText;
    public BossProfile introduction;

    public GameObject fadeToWhite;

    private string message;
    private DialogueLine[] lines;
    private float typeTime;
    private bool typing = false;
    private int lineIndex;

    public void Start()
    {
        lines = introduction.C1preBattleConversation;
        TypeThis();
    }

    public void TypeThis()
    {
        message = lines[lineIndex].dialogueLine;
        typeTime = 0.05f;

        for (int i = 0; i < message.Length; i++)
        {
            if (message[i] == '+')
            {
                message = message.Remove(i, 1);
                message = message.Insert(i, "\n");
            }
        }

        if (typing == false)
        {
            writtenText.text = "";

            StartCoroutine(TypeText());
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && typing)
        {
            StopAllCoroutines();
            writtenText.text = message;
            typing = false;
        }
        else if (Input.GetKeyDown(KeyCode.Z) && !typing)
        {
            if (lineIndex < lines.Length - 1) // types next line
            {
                lineIndex++;
                TypeThis();
            }
            else // quits dialogue
            {
                QuitDialogue();
            }
        }
    }

    void QuitDialogue()
    {
        if (fadeToWhite.activeSelf)
        {
            return;
        }
        StartCoroutine(StartGame());
    }

    //CREDIT TO http://wiki.unity3d.com/index.php/AutoType?_ga=2.28672252.1760231856.1570400781-1881874195.1512603304
    IEnumerator TypeText()
    {
        typing = true;

        int lineCounter = 0;
        float RealTypeTime;

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

    IEnumerator StartGame()
    {
        fadeToWhite.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        fadeToWhite.SetActive(true);
        for (int i = 0; i < 30; i++)
        {
            fadeToWhite.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, (i / 30f));
            yield return new WaitForSeconds(0.05f);
        }
        SceneManager.LoadScene("DifficultySelect");
    }
}
