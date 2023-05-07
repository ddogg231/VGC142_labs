using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemyAi : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;

    public LayerMask WhatIsGround, WhatIsPlayer;

    public float health;

    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    public float sightRange, attackRange;
    public bool playerInsightRange, playerInattackRange;

    Rigidbody rb;

    public projectile projectilePrefab;
    public float projectilespeed;

    public CharacterController playerInstance;
    public Transform spawnPoint;

    public UnityEvent onProjectileSpawned;

    void start()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();

        rb = GetComponent<Rigidbody>();
        if (projectilespeed <= 0)
            projectilespeed = 15.0f;

        if (!spawnPoint || !projectilePrefab)
            Debug.Log("Pease set up default values on" + gameObject.name);
    }

    private void Update()
    {
        playerInsightRange = Physics.CheckSphere(transform.position, sightRange, WhatIsPlayer);
        playerInattackRange = Physics.CheckSphere(transform.position, attackRange, WhatIsPlayer);

         if (playerInsightRange && playerInattackRange) AttackPlayer();
        
    }

    private void AttackPlayer()
    {
        if (!alreadyAttacked)
        {
            rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 12f, ForceMode.Impulse);
            rb.AddForce(transform.up * 8f, ForceMode.Impulse);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }

        onProjectileSpawned?.Invoke();
    }
   
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

   /* public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);

    }*/

    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);

        }

    }
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}

