using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private Vector3 expellVector;
    private Vector3 expellDir;
    public pickupState state;

    private Rigidbody2D rb;

    private float timer = 0.2f;

    private Vector3 playerPos;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        state = pickupState.expell;

        //Picks a random point on the circumferance of a circle around 
        //spawn point. 
        float xPos = 1.5f * Mathf.Cos(Random.Range(-360, 360));
        float yPos = 1.5f * Mathf.Sin(Random.Range(-360, 360));
        expellVector = new Vector3(xPos, yPos, 0);

        expellDir = expellVector;
        
    }

    public void Update()
    {
        playerPos = (GameObject.Find("Player").transform.position - transform.position).normalized;

        if (timer > 0 && state == pickupState.expell)
        {
            timer -= Time.deltaTime;
        } else
        {
            if (state != pickupState.attract)
            state = pickupState.falling;
        }

        switch (state)
        {
            case pickupState.expell:
                rb.velocity = expellDir * 2;
                break;
            case pickupState.falling:
                rb.velocity = Vector2.down * 1;
                break;
            case pickupState.attract:
                rb.velocity = playerPos * 4;
                break;
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Destroy(gameObject);
        } else if (other.tag == "PickupRange")
        {
            state = pickupState.attract;
        }
    }
}

public enum pickupState
{
    expell,
    falling,
    attract
}
