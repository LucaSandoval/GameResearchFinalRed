using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Shot", menuName = "Player Shots")]
public class PlayerShotProfile : ScriptableObject
{
    public Sprite bulletIcon;

    [Header("Primary Fire")]
    public float fireRate;
    public float bulletVelocity;
    public int spreadDegree;
    public float size;
    public float damageMultiplier;
    public spreadType spreadType;

    [Header("Focus Fire")]
    public float focusFireRate;
    public float focusBulletVelocity;
    public int focusSpreadDegree;
    public float focusSize;
    public float focusDamageMultiplier;
    public spreadType focusSpreadType;

    [Header("Misc.")]
    public bool pointTowards;
}

public enum spreadType
{
    single,
    triple,
    quad,
    fivespread,
    doublefocus
}
