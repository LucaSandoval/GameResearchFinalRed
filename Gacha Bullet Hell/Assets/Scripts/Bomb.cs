using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float growSpeed;
    public float delay;
    private SpriteRenderer ren;
    private float alpha;

    public void Start()
    {
        alpha = 1;
        ren = GetComponent<SpriteRenderer>();
        Destroy(gameObject, delay);
    }

    public void Update()
    {
        alpha -= Time.deltaTime;
        ren.color = new Color(ren.color.r, ren.color.g, ren.color.b, alpha);
        transform.localScale += new Vector3(growSpeed * Time.deltaTime, growSpeed * Time.deltaTime, 0);
    }
}
