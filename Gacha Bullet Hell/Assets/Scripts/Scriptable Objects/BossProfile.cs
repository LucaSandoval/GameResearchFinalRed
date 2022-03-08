using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Boss", menuName = "Bosses")]
public class BossProfile : ScriptableObject
{
    public int playerUnlockID;

    [Header("With Sirena")]
    public DialogueLine[] C1preBattleConversation;
    public DialogueLine[] C1easyPostBattleConversation;
    public DialogueLine[] C1hardPostBattleConversation;

    [Header("With Dot")]
    public DialogueLine[] C2preBattleConversation;
    public DialogueLine[] C2easyPostBattleConversation;
    public DialogueLine[] C2hardPostBattleConversation;

    [Header("With Renji")]
    public DialogueLine[] C3preBattleConversation;
    public DialogueLine[] C3easyPostBattleConversation;
    public DialogueLine[] C3hardPostBattleConversation;
}
