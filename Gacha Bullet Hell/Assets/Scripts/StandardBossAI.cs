using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StandardBossAI : MonoBehaviour
{
    public string bossName;
    public bossStates state;

    public int patternId;
    public bossPatterns[] patterns;

    private Rigidbody2D rb;
    private SpriteRenderer ren;

    private int movePointId;
    public Vector3[] movePoints;

    private Vector3 currentMovePos;
    private Vector3 randomMovePos;

    private float moveTimer;

    private float patternTimer;

    public int patternAttackId;
    EnemyPattern currentAttackPattern;

    private float shotTimer;
    private float maxShotTimer;

    public float burstTimer;
    private float burstTimerMax;

    private float firePauseMax;
    private float firePauseTimer;

    private GameObject shotPrefab;

    private bool coolDown;

    private GameObject healthBar;
    private Slider healthBarRefrence;

    private EnemyHealth healthScript;

    [Header("Random Movement")]
    public float xMax;
    public float xMin;
    public float yMin;
    public float yMax;

    [System.Serializable]
    public struct bossPatterns
    {
        public float duration;
        public float speed;
        public float moveInterval;
        public bossStates thisState;

        public EnemyPattern[] attacks;
    }

    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        ren = GetComponent<SpriteRenderer>();
        shotPrefab = Resources.Load<GameObject>("EnemyBullet");
        healthBar = Resources.Load<GameObject>("BossHealthBar");
        healthScript = GetComponent<EnemyHealth>();

        currentMovePos = movePoints[0];
        patternId = 0;
        movePointId = 0;

        patternTimer = patterns[0].duration;
        patternAttackId = 0;

        //Copy this everywhere
        maxShotTimer = patterns[patternId].attacks[patternAttackId].fireRate;
        shotTimer = maxShotTimer;

        burstTimerMax = patterns[patternId].attacks[patternAttackId].attackLength;
        burstTimer = burstTimerMax;

        firePauseMax = patterns[patternId].attacks[patternAttackId].restPeriod;
        firePauseTimer = firePauseMax;


        //Health bar
        GameObject newHealthBar = Instantiate(healthBar);
        newHealthBar.transform.SetParent(GameObject.Find("BossHealthParent").transform);
        newHealthBar.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        healthBarRefrence = newHealthBar.GetComponent<Slider>();
        healthBarRefrence.maxValue = healthScript.health;
        newHealthBar.transform.GetChild(0).GetComponent<Text>().text = bossName;
        newHealthBar.transform.localScale = new Vector2(1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        StateMachine();
        if (state != bossStates.entrance)
        {
            HandleCycles();
        }

        healthBarRefrence.value = healthScript.health;
    }

    public void StateMachine()
    {
        switch (state)
        {
            case bossStates.entrance:

                if (transform.position.y > 0)
                {
                    rb.velocity = Vector2.down * patterns[patternId].speed * 4;
                } else
                {
                    state = patterns[patternId].thisState; 
                }
                break;
            case bossStates.attack:

                //Boss not yet at move point
                if (transform.position != movePoints[movePointId])
                {
                    rb.velocity = (currentMovePos - transform.position) * patterns[patternId].speed;
                }

                break;
            case bossStates.wander:

                //Boss not yet at move point
                if (transform.position != randomMovePos)
                {
                    rb.velocity = (randomMovePos - transform.position) * patterns[patternId].speed;
                }

                break;
        }
    }

    public void HandleCycles()
    {

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

        if (firePauseTimer > 0)
        {
            firePauseTimer -= Time.deltaTime;
            coolDown = false;
        }
        else
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
            patternAttackId += 1;
            if (patternAttackId > patterns[patternId].attacks.Length - 1)
            {
                patternAttackId = 0;
            }
            maxShotTimer = patterns[patternId].attacks[patternAttackId].fireRate;
            shotTimer = maxShotTimer;

            burstTimerMax = patterns[patternId].attacks[patternAttackId].attackLength;
            burstTimer = burstTimerMax;

            firePauseMax = patterns[patternId].attacks[patternAttackId].restPeriod;
            firePauseTimer = firePauseMax;
        }

        if (moveTimer > 0)
        {
            moveTimer -= Time.deltaTime;
        }
        else
        {

            randomMovePos = new Vector3(Random.Range(xMin, xMax), Random.Range(yMin, yMax));

            movePointId += 1;

            if (movePointId > movePoints.Length - 1)
            {
                movePointId = 0;
            }

            currentMovePos = movePoints[movePointId];
            moveTimer = patterns[patternId].moveInterval;
        }

        if (patternTimer > 0)
        {
            patternTimer -= Time.deltaTime;
        }
        else
        {
            patternId += 1;
            if (patternId > patterns.Length - 1)
            {
                patternId = 0;
            }

            patternAttackId = 0;
            state = patterns[patternId].thisState;
            patternTimer = patterns[patternId].duration;

            maxShotTimer = patterns[patternId].attacks[patternAttackId].fireRate;
            shotTimer = maxShotTimer;

            burstTimerMax = patterns[patternId].attacks[patternAttackId].attackLength;
            burstTimer = burstTimerMax;

            firePauseMax = patterns[patternId].attacks[patternAttackId].restPeriod;
            firePauseTimer = firePauseMax;
        }
    }

    public void HandleBulletPulse()
    {
        switch (patterns[patternId].attacks[patternAttackId].shotType)
        {
            case enemyShotType.single:
                FireBullet(transform.position, -90);
                break;
            case enemyShotType.triple:
                FireBullet(transform.position, -90);
                FireBullet(transform.position + new Vector3(-0.1f, 0, 0), -90 - patterns[patternId].attacks[patternAttackId].spreadAngle);
                FireBullet(transform.position + new Vector3(0.1f, 0, 0), -90 + patterns[patternId].attacks[patternAttackId].spreadAngle);
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
            case enemyShotType.largeRingOffset:

                int idk = 22;

                FireBullet(transform.position, -90 + idk);
                FireBullet(transform.position, -45 + idk);
                FireBullet(transform.position, 0 + idk);
                FireBullet(transform.position, 45 + idk);
                FireBullet(transform.position, 90 + idk);
                FireBullet(transform.position, -135 + idk);
                FireBullet(transform.position, -180 + idk);
                FireBullet(transform.position, -225 + idk);
                break;
        }
    }

    private void OnDestroy()
    {
        Destroy(healthBarRefrence.gameObject);
    }

    public void FireBullet(Vector3 pos, int rotation)
    {
        GameObject newBullet = Instantiate(shotPrefab);
        newBullet.transform.position = pos;
        EnemyBullet script = newBullet.GetComponent<EnemyBullet>();

        script.velocity = patterns[patternId].attacks[patternAttackId].bulletVelocity;
        script.size = patterns[patternId].attacks[patternAttackId].size;
        script.drift = patterns[patternId].attacks[patternAttackId].drift;
        script.tracking = patterns[patternId].attacks[patternAttackId].tracking;
        script.angle = rotation;
        script.icon = patterns[patternId].attacks[patternAttackId].bulletImage;
        script.trackingDelay = patterns[patternId].attacks[patternAttackId].trackingDelay;
        script.accelerate = patterns[patternId].attacks[patternAttackId].accelerate;
        script.pointTowards = patterns[patternId].attacks[patternAttackId].pointTowards;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
    }
}

public enum bossStates 
{ 
    entrance,
    wander,
    attack,
    snap
}
