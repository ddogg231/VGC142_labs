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
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("EnemyShots") && collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.health--;
        }
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("wall"))
        {
            Destroy(gameObject);
           // Debug.Log("ground hit");
        }
        if (collision.gameObject.CompareTag("Player"))  
        {
            collision.GetComponent<Playercontroller>().TakeDamage(damage);
            //GameManager.Instance.health--;
            Destroy(gameObject);
            Debug.Log("player hit");
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            //collision.gameObject.GetComponent<EnemyController>().TakeDamage(damage);
            collision.gameObject.GetComponent<EnemyController>().TakeDamage(damage);
            Destroy(gameObject);
            Debug.Log("enemy hit");
        }
       // Destroy(gameObject);
    }


}