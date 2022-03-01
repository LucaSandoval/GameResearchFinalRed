using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Boss", menuName = "Bosses")]
public class BossProfile : ScriptableObject
{
    [Header("With (CHAR 1)")]
    public DialogueLine[] C1preBattleConversation;
    public DialogueLine[] C1easyPostBattleConversation;
    public DialogueLine[] C1hardPostBattleConversation;

    [Header("With (CHAR 2)")]
    public DialogueLine[] C2preBattleConversation;
    public DialogueLine[] C2easyPostBattleConversation;
    public DialogueLine[] C2hardPostBattleConversation;

    [Header("With (CHAR 3)")]
    public DialogueLine[] C3preBattleConversation;
    public DialogueLine[] C3easyPostBattleConversation;
    public DialogueLine[] C3hardPostBattleConversation;
}
