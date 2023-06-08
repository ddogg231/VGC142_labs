using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Weapon : MonoBehaviour
{
    private PlayerAttack playerAttack;
    [HideInInspector]
    public Rigidbody rb;

    [HideInInspector]
    public Transform weaponAttachPoint;

    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private Transform BulletSpawnpoint;
    [SerializeField]
    private float bulletSpeed = 700F;

   

    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody>();
        //set weapons spawnpoint
        weaponAttachPoint = transform.GetChild(0).transform;
    }

    public void Fire()
    {
        Debug.Log("FIRED!");
        GameObject bullet = Instantiate(bulletPrefab, BulletSpawnpoint.position, transform.rotation);
        bullet.GetComponent<Rigidbody>().AddForce(transform.forward * bulletSpeed);
        
    }

    
}