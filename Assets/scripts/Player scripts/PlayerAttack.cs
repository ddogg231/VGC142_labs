using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    private Playercontroller _input;
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private GameObject BulletSpawnpoint;
    [SerializeField]
    private float bulletSpeed = 700F;
    public Transform w;
    // Start is called before the first frame update
    void Start()
    {
        _input = transform.root.GetComponent<Playercontroller>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_input.fire)
        {
            Shoot();
            _input.fire = false;
        }
    }
    
    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, BulletSpawnpoint.transform.position, transform.rotation);
        bullet.GetComponent<Rigidbody>().AddForce(transform.forward * bulletSpeed);
        //Destroy(bullet, 1f);
       // Debug.Log("FIRE!");
    }
}
