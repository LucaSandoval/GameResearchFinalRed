using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardEnemyAI : MonoBehaviour
{
    public int direction;
    public float trend;
    private float enemyTrend;

    public float speed;

    private Rigidbody2D rb;

    private float shotTimer;
    private float maxShotTimer;

    public float burstTimer;
    private float burstTimerMax;

    public EnemyPattern[] bulletPatterns;
    private int bulletIndex = 0;

    private EnemyPattern shotPattern;
    private GameObject shotPrefab;

    private bool coolDown;

    private SpriteRenderer ren;

    private float firePauseMax;
    private float firePauseTimer;

    public void Start()
    {
        shotPattern = bulletPatterns[0];
        shotPrefab = Resources.Load<GameObject>("EnemyBullet");
        rb = GetComponent<Rigidbody2D>();
        ren = GetComponent<SpriteRenderer>();
        maxShotTimer = shotPattern.fireRate;
        shotTimer = maxShotTimer;

        burstTimerMax = shotPattern.attackLength;
        burstTimer = burstTimerMax;
        firePauseMax = shotPattern.restPeriod;
        firePauseTimer = firePauseMax;

        enemyTrend = Random.Range(-trend, trend);

        if (direction == 1)
        {
            ren.flipX = false;
        } else
        {
            ren.flipX = true;
        }
    }

    public void Update()
    {
        rb.velocity = new Vector2(direction * speed, enemyTrend);

        if (firePauseTimer > 0)
        {
            firePauseTimer -= Time.deltaTime;
            coolDown = false;
        } else
        {
            coolDown = true;
            firePauseTimer -= Time.deltaTime;

            if (firePauseTimer < firePauseMax * -1)
            {
                firePauseTimer = firePauseMax;
            }
        }

        if (burstTimer > 0)
        {
            burstTimer -= Time.deltaTime;
        }
        else
        {
            //Pick the next bullet pattern in its list
            bulletIndex += 1;
            if (bulletIndex > bulletPatterns.Length - 1)
            {
                bulletIndex = 0;
            }
            shotPattern = bulletPatterns[bulletIndex];


            burstTimer = shotPattern.attackLength;

            shotTimer = shotPattern.fireRate;

            firePauseMax = shotPattern.restPeriod;
            maxShotTimer = shotPattern.fireRate;
            firePauseTimer = firePauseMax;
            coolDown = false;
        }

        if (coolDown == false)
        {
            if (shotTimer > 0)
            {
                shotTimer -= Time.deltaTime;
            }
            else
            {
                shotTimer = maxShotTimer;
                HandleBulletPulse();
            }
        }
    }

    public void HandleBulletPulse()
    {
        switch (shotPattern.shotType)
        {
            case enemyShotType.single:
                FireBullet(transform.position, -90);
                break;
            case enemyShotType.triple:
                FireBullet(transform.position, -90);
                FireBullet(transform.position + new Vector3(-0.1f, 0,0), -90 - shotPattern.spreadAngle);
                FireBullet(transform.position + new Vector3(0.1f, 0, 0), -90 + shotPattern.spreadAngle);
                break;
            case enemyShotType.smallRing:
                FireBullet(transform.position, -90);
                FireBullet(transform.position, 0);
                FireBullet(transform.position, 90);
                FireBullet(transform.position, -180);
                break;
            case enemyShotType.smallRingOffset:

                int idk = 45;
                FireBullet(transform.position, -90 + idk);
                FireBullet(transform.position, 0 + idk);
                FireBullet(transform.position, 90 + idk);
                FireBullet(transform.position, -180 + idk);
                break;
            case enemyShotType.largeRing:
                FireBullet(transform.position, -90);
                FireBullet(transform.position, -45);
                FireBullet(transform.position, 0);
                FireBullet(transform.position, 45);
                FireBullet(transform.position, 90);
                FireBullet(transform.position, -135);
                FireBullet(transform.position, -180);
                FireBullet(transform.position, -225);
                break;
            case enemyShotType.massiveRing:

                FireBullet(transform.position, 0);
                FireBullet(transform.position, 22.5f);
                FireBullet(transform.position, 45);
                FireBullet(transform.position, 67.5f);
                FireBullet(transform.position, 90);
                FireBullet(transform.position, 112.5f);
                FireBullet(transform.position, 135);
                FireBullet(transform.position, 157.5f);
                FireBullet(transform.position, 180);
                FireBullet(transform.position, 202.5f);
                FireBullet(transform.position, 225);
                FireBullet(transform.position, 247.5f);
                FireBullet(transform.position, 270);
                FireBullet(transform.position, 292.5f);
                FireBullet(transform.position, 315);
                FireBullet(transform.position, 337.5f);
                FireBullet(transform.position, 360);
                break;
        }
    }

    public void FireBullet(Vector3 pos, float rotation)
    {
        GameObject newBullet = Instantiate(shotPrefab);
        newBullet.transform.position = pos;
        EnemyBullet script = newBullet.GetComponent<EnemyBullet>();

        script.velocity = shotPattern.bulletVelocity;
        script.size = shotPattern.size;
        script.drift = shotPattern.drift;
        script.tracking = shotPattern.tracking;
        script.angle = rotation;
        script.icon = shotPattern.bulletImage;
        script.trackingDelay = shotPattern.trackingDelay;
        script.accelerate = shotPattern.accelerate;
        script.pointTowards = shotPattern.pointTowards;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "DespawnZone")
        {
            Destroy(gameObject);
        }
    }
}
