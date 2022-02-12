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

    Vector3 thisDrift;

    Vector3 lastPlayerPos;

    private bool grazed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 20);

        transform.localScale = new Vector3(size, size, size);

        thisDrift = new Vector3(Random.Range(-drift, drift), 0, 0);

        lastPlayerPos = (GameObject.Find("Player").transform.position - transform.position).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        if (tracking == false)
        {
            Vector3 dir = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right;


            rb.velocity = (dir + thisDrift) * velocity;
        } else
        {
            rb.velocity = lastPlayerPos * velocity;
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "DespawnZone")
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
    largeRing
}

