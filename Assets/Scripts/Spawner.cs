using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject ToSpawn;

    public float Interval = 1f;
    
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Spawn", Interval, Interval);
    }

    void Spawn()
    {
        Instantiate(ToSpawn, transform);
    }
}
