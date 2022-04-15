using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGController : MonoBehaviour
{
    public Color bgCOLOR;


    [HideInInspector]
    public Transform originPoint;
    [HideInInspector]
    public GameObject bgObjectParent;
    public bgLayer[] layers;
    public bool active;

    private float[] timers;
    private float[] timerMaxes;

    [System.Serializable]
    public struct bgLayer
    {
        public Sprite[] sprites;
        public float spawnRate;
        public float moveSpeed;
        public float scale;
        public int layerOrder;
        //public float alpha;
        public Color color;
    }

    private void Start()
    {
        Camera mainCam = Camera.main;
        mainCam.backgroundColor = bgCOLOR;
        active = true;
        InitLayers();
    }

    public void InitLayers()
    {
        //For each layer of objects that will be spawned,
        //it needs its own timer, which is generated here.
        timers = new float[layers.Length];
        timerMaxes = new float[layers.Length];

        for (int i = 0; i < layers.Length; i++)
        {
            timers[i] = 0;
            timerMaxes[i] = layers[i].spawnRate;
        }
    }

    public void Update()
    {
        //Debug.Log("test");
        if (active == true)
        {
            for (int i = 0; i < layers.Length; i++)
            {
                timers[i] += Time.deltaTime;

                if (timers[i] >= timerMaxes[i])
                {
                    timers[i] = 0;
                    SpawnBGObject(i);
                }
            }
        }
    }

    public void SpawnBGObject(int id)
    {
        GameObject newSprite = new GameObject();
        newSprite.AddComponent<SpriteRenderer>();
        newSprite.AddComponent<BGObject>();

        GameObject newBGObj = Instantiate(newSprite, bgObjectParent.transform);
        newBGObj.transform.position = originPoint.position + new Vector3(Random.Range(-10, 10), 0, 0);
        newBGObj.transform.localScale = new Vector3(layers[id].scale, layers[id].scale, 0);

        SpriteRenderer newRen = newBGObj.GetComponent<SpriteRenderer>();

        newRen.sprite = layers[id].sprites[Random.Range(0, layers[id].sprites.Length)];
        newRen.color = layers[id].color;
        newRen.sortingOrder = layers[id].layerOrder;

        newBGObj.GetComponent<BGObject>().speed = layers[id].moveSpeed;
    }
}
