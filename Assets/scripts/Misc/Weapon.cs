using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    [HideInInspector]
    public Rigidbody rb;
    [HideInInspector]
    public Transform SpawnPoint;

    public Rigidbody projectilePrefab;

    public float projectileSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //set weapons spawnpoint
        SpawnPoint = transform.GetChild(0).transform;
    }

    public void Fire()
    {
        // do nothing if no weapon
        if (!projectilePrefab) return;

        Rigidbody temp = Instantiate(projectilePrefab, SpawnPoint.position, SpawnPoint.rotation);
        temp.AddForce(transform.forward * projectileSpeed);
    }
            
}
