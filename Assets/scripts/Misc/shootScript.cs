using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class shootScript : MonoBehaviour
{
    Rigidbody rb;
    public UnityEvent onProjectileSpawned;
    public float projectilespeed;
    public Transform spawnPoint;
    

    public projectile projectilePrefab;

    public object OnProjectileSpawned { get; internal set; }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (projectilespeed <= 0)
            projectilespeed = 15.0f;

        if (!spawnPoint || !projectilePrefab)
            Debug.Log("Pease set up default values on" + gameObject.name);
    }


    public void fire()
    {

           // projectile curprojectile = Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation);
           // curprojectile.speed = projectilespeed;
  
        onProjectileSpawned?.Invoke();
    }
}

