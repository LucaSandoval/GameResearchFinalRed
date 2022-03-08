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
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "DespawnZone")
        {
            
            Destroy(gameObject);
        }
    }
}

