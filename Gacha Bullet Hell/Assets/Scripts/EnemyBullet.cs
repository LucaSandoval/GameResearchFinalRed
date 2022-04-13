using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private Rigidbody2D rb;
    public float velocity;
    public float size;
    public float angle;
    public float drift;
    public bool tracking;
    public bool accelerate;
    public float trackingDelay;
    private bool doneWithTrackingDelay;
    public bool pointTowards;

    Vector3 thisDrift;

    Vector3 lastPlayerPos;

    private bool grazed;

    private SpriteRenderer ren;
    public Sprite icon;

    private PlayerController playerController;
    private float grazeTimer;
    private bool canGraze;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ren = GetComponent<SpriteRenderer>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        //Destroy(gameObject, 20);

        transform.localScale = new Vector3(size, size, size);

        thisDrift = new Vector3(Random.Range(-drift, drift), 0, 0);

        lastPlayerPos = (GameObject.Find("Player").transform.position - transform.position).normalized;

        ren.sprite = icon;
        doneWithTrackingDelay = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (tracking == false)
        {
            MoveBullet();
        } else
        {
            if (trackingDelay > 0)
            {
                trackingDelay -= Time.deltaTime;
                MoveBullet();
            } else
            {
                if (doneWithTrackingDelay == false)
                {
                    lastPlayerPos = (GameObject.Find("Player").transform.position - transform.position).normalized;
                    doneWithTrackingDelay = true;
                } 
                rb.velocity = lastPlayerPos * velocity;
            }    
        }

        if (accelerate == true)
        {
            velocity += Time.deltaTime;
        }

        if (grazeTimer > 0)
        {
            grazeTimer -= Time.deltaTime;
            canGraze = false;
        } else
        {
            canGraze = true;
        }

        if (Vector3.Distance(transform.position, PlayerController.globalPlayerPos) <= 0.5f)
        {
            if (canGraze)
            {
                PlayerStatController.graze += 1;
                grazeTimer += 0.4f;
            }
        }

        if (pointTowards)
        {
            Vector2 moveDirection = rb.velocity;
            if (moveDirection != Vector2.zero)
            {
                float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }
    }

    public void MoveBullet()
    {
        Vector3 dir = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right;
        rb.velocity = (dir + thisDrift) * velocity;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "DespawnZone" || other.tag == "Bomb")
        {
            Destroy(gameObject);
        }
    }
}

public enum enemyShotType
{
    single,
    triple,
    quad,
    smallRing,
    largeRing,
    largeRingOffset,
    smallRingOffset,
    massiveRing,
    massiveRingOffset
}

