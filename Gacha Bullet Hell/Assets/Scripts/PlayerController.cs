using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public PlayerProfile profile;

    private float fullSpeed;
    private float focusSpeed;

    private float currentSpeed;

    private PlayerShotProfile shotProfile;
    private GameObject playerBulletRefrence;
    private GameObject playerBombRefrence;

    private float maxTimer;
    private float timer;

    private Rigidbody2D rb;
    private SpriteRenderer ren;
    public GameObject hitbox;

    public bool focus;

    public GameObject dialogueSystem;

    //public float damageLevel = 1.0f;

    public bool respawning;
    private float respawnTimer;
    private float flashTimer;

    public static Vector3 globalPlayerPos;

    [HideInInspector]
    public PlayerStatController statController;

    [Header("Animation")]
    public bool animated;
    public float frameRate;
    public Sprite[] frames;
    private float animTimer;
    private int frameID;

    // Start is called before the first frame update
    void Start()
    {
        hitbox.SetActive(false);
        respawning = false;
        respawnTimer = 0;
        currentSpeed = fullSpeed;
        ren = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        statController = GetComponent<PlayerStatController>();

        Init();
    }

    public void Init()
    {
        fullSpeed = profile.moveSpeed;
        focusSpeed = profile.focusMoveSpeed;
        shotProfile = profile.pattern;
        frames = profile.anims;

        maxTimer = shotProfile.fireRate;

        playerBulletRefrence = Resources.Load<GameObject>("PlayerBullet");
        playerBombRefrence = Resources.Load<GameObject>("BombPrefab");
    }

    // Update is called once per frame
    void Update()
    {
        globalPlayerPos = transform.position;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = focusSpeed;
            maxTimer = shotProfile.focusFireRate;
            focus = true;
            ren.color = new Color(1, 1, 1, 0.3f);
            hitbox.SetActive(true);
        } else
        {
            currentSpeed = fullSpeed;
            maxTimer = shotProfile.fireRate;
            focus = false;
            ren.color = new Color(1, 1, 1, 1f);
            hitbox.SetActive(false);
        }

        if (respawnTimer > 0)
        {
            respawning = true;
            respawnTimer -= Time.deltaTime;

            if (flashTimer > 0)
            {
                flashTimer -= Time.deltaTime;
                ren.enabled = false;
            } else {
                ren.enabled = true;
                if (flashTimer > -0.1f)
                {
                    flashTimer -= Time.deltaTime;
                } else
                {
                    flashTimer = 0.1f;
                }
            }
        }
        else
        {
            respawning = false;
            ren.enabled = true;
        }


        rb.velocity = new Vector3(Input.GetAxisRaw("Horizontal") *
                currentSpeed, Input.GetAxisRaw("Vertical") * currentSpeed, 0);

        if (dialogueSystem.activeSelf)
        {
            respawnTimer = 0.1f;
            flashTimer = 0;
            return;
        }

        if (Input.GetKey(KeyCode.Z))
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                HandleBulletPulse();
                timer = maxTimer;
            }
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (PlayerStatController.bombs > 0)
            {
                Bomb();
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

        if (PlayerStatController.damageLevel > 3)
        {
            PlayerStatController.damageLevel = 3;
        }

        //if(transform.position.y < )

    }

    public void Bomb()
    {
        PlayerStatController.bombs -= 1;
        statController.GenerateBombsIcons();

        GameObject newBomb = Instantiate(playerBombRefrence);
        newBomb.transform.position = transform.position;
    }

    public void Death()
    {
        if (respawning == false)
        {
            respawning = true;
            PlayerStatController.lives -= 1;

            if (PlayerStatController.lives < 0)
            {
                SceneManager.LoadScene("Game Over");
            }

            statController.GenerateLivesIcons();

            respawnTimer = 2;
            flashTimer = 0.1f;


            float penalty = 1;
            float startingDamage = PlayerStatController.damageLevel;

            // new power level
            float newDamage = PlayerStatController.damageLevel - penalty;
            if (newDamage < 1)
            {
                newDamage = 1;
            }

            PlayerStatController.damageLevel = newDamage;

            // power drops
            float pickupsLost = startingDamage - penalty;
            if (pickupsLost < 1)
            {
                penalty = penalty + pickupsLost;
                penalty--;
            }

            
            SpawnPickups(Mathf.FloorToInt(penalty * 10));
            transform.position = new Vector3(0, -3, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "EnemyBullet")
        {
            Death();
        }
    }


    void HandleBulletPulse()
    {
        spreadType spread = spreadType.single;
        int degree = 0;

        if (focus == false)
        {
            spread = shotProfile.spreadType;
            degree = shotProfile.spreadDegree;
        } else
        {
            spread = shotProfile.focusSpreadType;
            degree = shotProfile.focusSpreadDegree;
        }

        switch (spread)
        {
            case spreadType.single:
                FireBullet(transform.position, 90);
                break;
            case spreadType.doublefocus:
                FireBullet(transform.position + new Vector3(0.1f, 0), 90);
                FireBullet(transform.position + new Vector3(-0.1f, 0), 90);
                break;
            case spreadType.triple:
                FireBullet(transform.position, 90);
                FireBullet(transform.position + new Vector3(0.1f, 0), 90 - degree);
                FireBullet(transform.position + new Vector3(-0.1f, 0), 90 + degree);
                break;
            case spreadType.quad:
                FireBullet(transform.position + new Vector3(0.1f, 0), 90);
                FireBullet(transform.position + new Vector3(-0.1f, 0), 90);
                FireBullet(transform.position + new Vector3(0.2f, -0.1f), 90 - degree);
                FireBullet(transform.position + new Vector3(-0.2f, -0.1f), 90 + degree);
                break;
            case spreadType.fivespread:
                FireBullet(transform.position, 90);
                FireBullet(transform.position + new Vector3(0.15f, 0), 90 - (int)(degree / 1.5));
                FireBullet(transform.position + new Vector3(-0.15f, 0), 90 + (int)(degree / 1.5));
                FireBullet(transform.position + new Vector3(0.25f, -0.1f), 90 - degree);
                FireBullet(transform.position + new Vector3(-0.25f, -0.1f), 90 + degree);
                break;
        }
    }

    void FireBullet(Vector3 pos, int rotation)
    {
        GameObject newBullet = Instantiate(playerBulletRefrence);

        PlayerBullet newBulletScript = newBullet.GetComponent<PlayerBullet>();

        newBullet.transform.position = pos;
        newBulletScript.angle = rotation;

        float vel = 0;
        float size = 0;
        float mult = 1;

        if (focus == false)
        {
            vel = shotProfile.bulletVelocity;
            size = shotProfile.size;
            mult = shotProfile.damageMultiplier;
        }
        else
        {
            vel = shotProfile.focusBulletVelocity;
            size = shotProfile.focusSize;
            mult = shotProfile.focusDamageMultiplier;
        }

        newBulletScript.velocity = vel;
        newBulletScript.scale = size;
        newBulletScript.icon = shotProfile.bulletIcon;
        newBulletScript.damage = PlayerStatController.damageLevel * mult;
        newBulletScript.pointTowards = shotProfile.pointTowards;
    }

    public void SpawnPickups(float ammount)
    {
        GameObject pickuprefrence = Resources.Load<GameObject>("Pickup");

        for (int i = 0; i < ammount; i++)
        {
            GameObject newPickup = Instantiate(pickuprefrence);
            newPickup.transform.position = transform.position;
            newPickup.GetComponent<Pickup>().spawnedFromPlayer = true;
        }
    }
}
