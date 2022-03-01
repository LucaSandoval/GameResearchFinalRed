using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health;
    public float reward;
    private SpriteRenderer ren;

    private float damageFade;
    private Color defColor;

    private GameObject waveSpawner;

    private void Start()
    {
        ren = GetComponent<SpriteRenderer>();
        defColor = ren.color;
    }

    private void Update()
    {
        if (damageFade > 0)
        {
            ren.color = new Color(0.933f, 0.516f, 0.545f);
            damageFade -= Time.deltaTime;
        }
        else
        {
            ren.color = defColor;
        }

        if (damageFade > 0.1f)
        {
            damageFade = 0.1f;
        }

        if (health <= 0)
        {
            SpawnPickups(reward);
            Destroy(gameObject);
            if(waveSpawner != null)
            {
                // triggers boss defeat dialogue 4 seconds after death to allow time for pickups
                waveSpawner.GetComponent<WaveSpawner>().Invoke("BossDeath", 4f);
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PlayerBullet")
        {
            damageFade += 0.02f;
            health -= other.GetComponent<PlayerBullet>().damage;
            Destroy(other.gameObject);
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

    public void SetWaveSpawner(GameObject target)
    {
        waveSpawner = target;
    }
}
