using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPickups : MonoBehaviour
{
    public GameObject[] spawnpoint;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(spawnpoint[Random.Range(0, spawnpoint.Length)], this.transform);
    }

    
}
