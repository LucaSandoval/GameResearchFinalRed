using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health;
    public float damageReward;
    public float pointReward;

    public bool isBoss;

    private GameObject explosionEffect;

    [Header("Animation")]
    public bool animated;
    public float frameRate;
    public Sprite[] frames;
    private float animTimer;
    private int frameID;

    private SpriteRenderer ren;

    private float damageFade;
    private Color defColor;

    private GameObject waveSpawner;

    private void Start()
    {
        ren = GetComponent<SpriteRenderer>();


        if (!isBoss)
        {
            explosionEffect = Resources.Load<GameObject>("explosion");
        } else
        {
            explosionEffect = Resources.Load<GameObject>("boss_explosion");
        }


        defColor = ren.color;
        frameID = 0;
        animTimer = frameRate;
    }

    private void Update()
    {
        if (damageFade > 0)
        {
            ren.color = new Color(0.933f, 0.516f, 0.545f);
            damageFade -= Time.deltaTime;
        }
        else
        {
            ren.color = defColor;
        }

        if (damageFade > 0.1f)
        {
            damageFade = 0.1f;
        }

        if (health <= 0)
        {
            GameObject newEffect = Instantiate(explosionEffect);
            newEffect.transform.position = transform.position;


            SpawnPickups(damageReward, pickupType.power);
            SpawnPickups(pointReward, pickupType.point);
            Destroy(gameObject);
            if(waveSpawner != null)
            {
                // triggers boss defeat dialogue 4 seconds after death to allow time for pickups
                waveSpawner.GetComponent<WaveSpawner>().Invoke("BossDeath", 4f);
            }
        }

        if (animated == true)
        {
            if (animTimer > 0)
            {
                animTimer -= Time.deltaTime;
            }
            else
            {
                animTimer = frameRate;

                frameID += 1;

                if (frameID > frames.Length - 1)
                {
                    frameID = 0;
                }
                ren.sprite = frames[frameID];
            }   
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PlayerBullet")
        {
            damageFade += 0.02f;
            health -= other.GetComponent<PlayerBullet>().damage;
            Destroy(other.gameObject);
        }   
    }

    public void SpawnPickups(float ammount, pickupType type)
    {
        GameObject pickuprefrence = null;
        if (type == pickupType.power)
        {
            pickuprefrence = Resources.Load<GameObject>("Pickup");
        } else if (type == pickupType.point)
        {
            pickuprefrence = Resources.Load<GameObject>("Point");
        }

        for (int i = 0; i < ammount; i++)
        {
            GameObject newPickup = Instantiate(pickuprefrence);
            newPickup.transform.position = transform.position;
        }
    }

    public void SetWaveSpawner(GameObject target)
    {
        waveSpawner = target;
    }
}
