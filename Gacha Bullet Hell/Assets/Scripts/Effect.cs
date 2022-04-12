using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public float delay;

    public void Start()
    {
        Destroy(gameObject, delay);
    }
}
