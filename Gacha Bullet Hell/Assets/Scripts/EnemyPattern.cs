using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Shot", menuName = "Enemy Shots")]
public class EnemyPattern : ScriptableObject
{
    public Sprite bulletImage;

    public float fireRate;
    //public float ammo;
    public float attackLength;
    public float restPeriod;
    public int spreadAngle;
    public float bulletVelocity;
    public float size;
    public float drift;
    public bool tracking;
    public enemyShotType shotType;
}