using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player", menuName = "Players")]
public class PlayerProfile : ScriptableObject
{
    public float moveSpeed;
    public float focusMoveSpeed;

    public PlayerShotProfile pattern;

    public Sprite[] anims;
}
