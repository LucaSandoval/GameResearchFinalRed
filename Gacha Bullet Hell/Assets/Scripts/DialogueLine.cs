using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public string characterName;
    public Sprite avatar;
    public string dialogueLine;

    [HideInInspector]
    public bool triggerFight;
}
