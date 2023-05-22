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
    Animator anim;
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
    public Transform powerupSpawnPoint;

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

    [SerializeField] Healthbar healthBar;
    public enum EnemyState
    {
        Chase, Patrol
    }
    int errorCounter = 0;

    void Start()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        Debug.Log("animator not grabed");
        health = maxHealth;
        healthBar = GetComponentInChildren<Healthbar>();
        healthBar.UpdateHealthBar(health, maxHealth);
        if (projectilespeed <= 0)
            projectilespeed = 15.0f;

        if (!powerupSpawnPoint || !projectilePrefab)
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
            if (!anim) throw new UnassignedReferenceException("Model not set on" + name);
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



    void Death()
    {
        anim.SetTrigger("Dead");
        agent.speed = 0f;
    }
    public virtual void TakeDamage(float damage)
    {
        health -= damage;


        if (health <= 0)
        {
            Death();

            StartCoroutine(DeathSequence());
        }
    }


    private IEnumerator DeathSequence()
    {

        yield return new WaitForSeconds(1f);

        Instantiate(powerUpPrefab[Random.Range(0, powerUpPrefab.Length)], powerupSpawnPoint.transform.position, Quaternion.identity);

        yield return new WaitForSeconds(2.0f);

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