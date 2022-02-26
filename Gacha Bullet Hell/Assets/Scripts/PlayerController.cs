using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float fullSpeed;
    public float focusSpeed;

    private float currentSpeed;

    public PlayerShotProfile shotProfile;
    private GameObject playerBulletRefrence;

    private float maxTimer;
    private float timer;

    private Rigidbody2D rb;
    private SpriteRenderer ren;
    public GameObject hitbox;

    public bool focus;

    //public float damageLevel = 1.0f;

    public bool respawning;
    private float respawnTimer;
    private float flashTimer;

    public static Vector3 globalPlayerPos;

    [HideInInspector]
    public PlayerStatController statController;

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
        maxTimer = shotProfile.fireRate;

        playerBulletRefrence = Resources.Load<GameObject>("PlayerBullet");
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

            rb.velocity = Vector3.zero;
            transform.position = new Vector3(0, -3, 0);

            if (flashTimer > 0)
            {
                flashTimer -= Time.deltaTime;
                ren.enabled = false;
            } else {
                ren.enabled = true;
                if (flashTimer > -0.2f)
                {
                    flashTimer -= Time.deltaTime;
                } else
                {
                    flashTimer = 0.2f;
                }
            }
        }
        else
        {
            respawning = false;
            ren.enabled = true;
        }

        if (respawning == false)
        {

            rb.velocity = new Vector3(Input.GetAxisRaw("Horizontal") *
                currentSpeed, Input.GetAxisRaw("Vertical") * currentSpeed, 0);

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
        }

        
    }

    public void Death()
    {
        if (respawning == false)
        {
            respawning = true;
            statController.lives -= 1;
            statController.GenerateLivesIcons();

            respawnTimer = 1;
            flashTimer = 0.2f;


            float penalty = 10;
            float startingDamage = statController.damageLevel;

            float newDamage = statController.damageLevel - penalty;
            if (newDamage < 0)
            {
                newDamage = 0;
            }

            statController.damageLevel = newDamage;

            float pickupsLost = startingDamage - penalty;
            if (pickupsLost < 0)
            {
                penalty = penalty + pickupsLost;
            }

            SpawnPickups(penalty);
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
        newBulletScript.damage = statController.damageLevel * mult;
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
