using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAi : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    Animator animator;
    public LayerMask WhatIsGround, WhatIsPlayer;

    public float health;
    public float maxHealth;
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
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    public bool playerInSightRange, playerInAttackRange;

    public GameObject[] powerUpPrefab;
    public GameObject[] waypoints;
    public Transform target;
    int waypointIndex;


    public float distThreshhold;

    public EnemyState currentState = EnemyState.Patrol;
    public bool Run;
    public bool Dead;

    private Healthbar healthBar;
    public enum EnemyState
    {
        Chase, Patrol
    }
    int errorCounter = 0;

    void start()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        Debug.Log("animator not grabed");

        healthBar = GetComponent<Healthbar>();
        if (projectilespeed <= 0)
            projectilespeed = 15.0f;

        if (!spawnPoint || !projectilePrefab)
            Debug.Log("Pease set up default values on" + gameObject.name);

        if (distThreshhold <= 0) distThreshhold = 0.5f;

        if (waypoints.Length <= 0) waypoints = GameObject.FindGameObjectsWithTag("Patrol");
        if(currentState == EnemyState.Chase)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            if (target) agent.SetDestination(target.position);
        }
        if (distThreshhold <= 0) distThreshhold = 0.5f;
        try
        {


            if (!animator) throw new UnassignedReferenceException("Model not set on" + name);
        }
        catch (UnassignedReferenceException e)
        {
            Debug.Log(e.Message);
            errorCounter++;
        }
        finally
        {
            Debug.Log("The script ran with " + errorCounter.ToString() + " errors");
        }
    }

    private void Update()
    {
        playerInsightRange = Physics.CheckSphere(transform.position, sightRange, WhatIsPlayer);
        playerInattackRange = Physics.CheckSphere(transform.position, attackRange, WhatIsPlayer);

        if (!target) return;
        if (currentState == EnemyState.Patrol)
        {
            Debug.DrawLine(transform.position, target.position, Color.red);

            if (agent.remainingDistance < distThreshhold)
            {
                waypointIndex++;
                waypointIndex %= waypoints.Length;

                target = waypoints[waypointIndex].transform;
                
                
            }
        }
        if (currentState == EnemyState.Chase)
        {
            if (!target.CompareTag("Patrol")) target = GameObject.FindGameObjectWithTag("Player").transform;
        }
        if (target) agent.SetDestination(target.position);
 
    }
   


    public virtual void TakeDamage(float damage)
    {
        health -= damage;
       if (healthBar != null)
       {
           healthBar.SetHealth(health, maxHealth);
       }
       
        if (health <= 0)
        {
            agent.speed = 0;
        
            //Destroy(gameObject);
             Die();
        }

    }
    public void Die()
    {
        // Play death animation
       // animator.SetBool("Dead", true);

        // Wait for the death animation to finish or a specific time period
        StartCoroutine(DeathSequence());
    }

    private IEnumerator DeathSequence()
    {
        // Wait for the duration of the death animation or a specific time period
        yield return new WaitForSeconds(1f);

        Instantiate(powerUpPrefab[Random.Range(0, powerUpPrefab.Length)], spawnPoint.transform.position, Quaternion.identity);
        
        yield return new WaitForSeconds(2.0f);

        Destroy(gameObject);
       // Debug.Log("Die started");
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}