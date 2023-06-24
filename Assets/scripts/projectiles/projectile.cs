using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class projectile : MonoBehaviour
{
    public float lifetime;
    
    public int damage;
    
    
    // Start is called before the first frame update
    void Start()
    {
        if (lifetime <= 0)
            lifetime = 2.0f;

        Destroy(gameObject, lifetime);
    }
    public void OnTriggerEnter(Collider other)
    {
       if (other.gameObject.CompareTag("EnemyShots") && other.gameObject.CompareTag("Player"))
       {
           GameManager.Instance.health--;
       }
       if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("wall"))
       {
           Destroy(gameObject);
           // Debug.Log("ground hit");
       }
       
       if (other.gameObject.CompareTag("Player"))
       {
           Playercontroller playerController = other.gameObject.GetComponent<Playercontroller>();
           if (playerController != null)
           {
               playerController.TakeDamage(damage);
               Destroy(gameObject);
               Debug.Log("player hit");
           }
       }

        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemyController enemyController = other.gameObject.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                enemyController.TakeDamage(damage);
                Destroy(gameObject);
                Debug.Log("enemy hit");
            }
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            TankTurret turret = other.gameObject.GetComponent<TankTurret>();
            if (turret != null)
            {
                turret.TakeDamage(damage);
                Destroy(gameObject);
                Debug.Log("turret hit");
            }
        }
    }



}