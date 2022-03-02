using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private Vector3 expellVector;
    private Vector3 expellDir;
    public pickupState state;

    private Rigidbody2D rb;

    private float timer;

    private Vector3 playerPos;

    public pickupType pickupType;
    public bool spawnedFromPlayer;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        state = pickupState.expell;

        if (spawnedFromPlayer == false)
        {
            timer = 0.2f;
            //Picks a random point on the circumferance of a circle around 
            //spawn point. 
            float xPos = 1.5f * Mathf.Cos(Random.Range(-360, 360));
            float yPos = 1.5f * Mathf.Sin(Random.Range(-360, 360));
            expellVector = new Vector3(xPos, yPos, 0);
        } else
        {
            float rand = Random.Range(0, 90);

            timer = 0.6f;
            float xPos = 1.5f * Mathf.Cos(rand);
            float yPos = 1.5f * Mathf.Sin(90);
            expellVector = new Vector3(xPos, yPos, 0);
        }

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

        if (Vector3.Distance(transform.position, PlayerController.globalPlayerPos) <= 1.5f &&
            state != pickupState.expell)
        {
            state = pickupState.attract;
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            switch (pickupType)
            {
                case pickupType.power:
                    other.GetComponent<PlayerController>().statController.damageLevel += 0.1f;
                    break;
                case pickupType.point:
                    other.GetComponent<PlayerController>().statController.bombPoints += 1f;
                    break;
            }

            Destroy(gameObject);
        }
    }
}

public enum pickupState
{
    expell,
    falling,
    attract
}

public enum pickupType
{
    power,
    point,
    oneup
}
