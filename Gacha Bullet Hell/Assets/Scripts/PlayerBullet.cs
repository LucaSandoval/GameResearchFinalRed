using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float velocity;
    public SpriteRenderer ren;
    private Rigidbody2D rb;
    public Sprite icon;
    public float angle;
    public float scale;
    public float damage;
    public bool pointTowards;
    public void Start()
    {
        ren = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        ren.sprite = icon;
        Destroy(gameObject, 20);

        transform.localScale = new Vector3(scale, scale, scale);
    }

    public void Update()
    {

        Vector3 dir = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right;

        rb.velocity = dir * velocity;

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

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "DespawnZone")
        {
            
            Destroy(gameObject);
        }
    }
}

