using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Weapon : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody rb;

    [HideInInspector]
    public float dropForce;

    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private Transform bulletSpawnpoint;
    [SerializeField]
    private float bulletSpeed = 700F;
    public float fireRate = 3;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Fire()
    {
        Debug.Log("Fired!");
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnpoint.position, transform.rotation);
        bullet.GetComponent<Rigidbody>().AddForce(transform.forward * bulletSpeed);
    }
}
//public class Weapon : MonoBehaviour
//{
//    private PlayerAttack playerAttack;
//    [HideInInspector]
//    public Rigidbody rb;
//
//    [HideInInspector]
//    public Transform weaponAttachPoint;
//
//    [SerializeField]
//    private GameObject bulletPrefab;
//    [SerializeField]
//    private Transform BulletSpawnpoint;
//    [SerializeField]
//    private float bulletSpeed = 700F;
//    public float fireRate = 3;
//   
//
//    // Start is called before the first frame update
//    void Start()
//    {
//
//        rb = GetComponent<Rigidbody>();
//        //set weapons spawnpoint
//        weaponAttachPoint = transform.GetChild(0).transform;
//    }
//
//    public void Fire()
//    {
//        Debug.Log("FIRED!");
//        GameObject bullet = Instantiate(bulletPrefab, BulletSpawnpoint.position, transform.rotation);
//        bullet.GetComponent<Rigidbody>().AddForce(transform.forward * bulletSpeed);
//        
//    }
//
//    
//}