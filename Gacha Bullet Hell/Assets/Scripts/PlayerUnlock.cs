using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUnlock : MonoBehaviour
{
    [HideInInspector]
    public Image icon;
    public bool unlocked;
    public bool selected;
    public string playerName;
    //public int playerID;

    public Sprite unlockImage;

    private GameObject highlight;
    private GameObject hover;

    public void Start()
    {
        icon = GetComponent<Image>();
        highlight = transform.GetChild(0).gameObject;
        hover = transform.GetChild(1).gameObject;

    }

    public void Update()
    {

        if (unlocked == true)
        {
            icon.sprite = unlockImage;
        }

        if (selected == true)
        {
            highlight.SetActive(true);
        } else
        {
            highlight.SetActive(false);
        }
    }

    public void HighLight()
    {
        hover.SetActive(true);
    }

    public void UnHighLight()
    {
        hover.SetActive(false);
    }
 
}
