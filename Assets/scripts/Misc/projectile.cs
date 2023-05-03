using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class projectile : MonoBehaviour
{
    public float lifetime;

    [HideInInspector]
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        if (lifetime <= 0)
            lifetime = 2.0f;

        GetComponent<Rigidbody>().velocity = new Vector3(speed, 0);
        Destroy(gameObject, lifetime);
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("EnemyShots") && collision.gameObject.CompareTag("Player"))
            GameManager.Instance.health--;

        if (collision.gameObject.CompareTag("ground") || collision.gameObject.CompareTag("wall"))
            Destroy(gameObject);

        if (collision.gameObject.CompareTag("player"))  
        {
            GameManager.Instance.health--;
            Destroy(gameObject);
            Debug.Log("player hit");
        }

        if (collision.gameObject.CompareTag("Enemy"))
            collision.gameObject.GetComponent<EnemyAi>().TakeDamage(1);
        Debug.Log("enemy hit");

        Destroy(gameObject);
    }


}