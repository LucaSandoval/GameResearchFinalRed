using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardEnemyAI : MonoBehaviour
{
    public int direction;
    public float trend;
    private float enemyTrend;

    public float speed;
    public float health;
    public float reward;

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

    private float damageFade;
    private Color defColor;
    private SpriteRenderer ren;

    private float firePauseMax;
    private float firePauseTimer;

    public void Start()
    {
        shotPattern = bulletPatterns[0];
        shotPrefab = Resources.Load<GameObject>("EnemyBullet");
        rb = GetComponent<Rigidbody2D>();
        ren = GetComponent<SpriteRenderer>();
        defColor = ren.color;
        maxShotTimer = shotPattern.fireRate;
        shotTimer = maxShotTimer;

        burstTimerMax = shotPattern.attackLength;
        burstTimer = burstTimerMax;
        firePauseMax = shotPattern.restPeriod;
        firePauseTimer = firePauseMax;

        enemyTrend = Random.Range(-trend, trend);
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

        if (health <= 0)
        {
            SpawnPickups(reward);
            Destroy(gameObject);
        }

        if (damageFade > 0)
        {
            ren.color = new Color(0.933f, 0.516f, 0.545f);
            damageFade -= Time.deltaTime;
        } else
        {
            ren.color = defColor;
        }

        if (damageFade > 0.1f)
        {
            damageFade = 0.1f;
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
        }
    }

    public void FireBullet(Vector3 pos, int rotation)
    {
        GameObject newBullet = Instantiate(shotPrefab);
        newBullet.transform.position = pos;
        EnemyBullet script = newBullet.GetComponent<EnemyBullet>();

        script.velocity = shotPattern.bulletVelocity;
        script.size = shotPattern.size;
        script.drift = shotPattern.drift;
        script.tracking = shotPattern.tracking;
        script.angle = rotation;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PlayerBullet")
        {
            damageFade += 0.1f;
            health -= other.GetComponent<PlayerBullet>().damage;
            Destroy(other.gameObject);
        } else if (other.tag == "DespawnZone")
        {
            Destroy(gameObject);
        }
    }

    public void SpawnPickups(float ammount)
    {
        GameObject pickuprefrence = Resources.Load<GameObject>("Pickup");

        for (int i = 0; i < ammount; i++)
        {
            GameObject newPickup = Instantiate(pickuprefrence);
            newPickup.transform.position = transform.position;
        }
    }
}
