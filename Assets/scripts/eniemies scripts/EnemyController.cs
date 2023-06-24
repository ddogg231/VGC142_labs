using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    Animator anim;
    public LayerMask WhatIsPlayer;

    public int currenthealth;
    public int maxHealth;
    public float timeBetweenAttacks;
    public Healthbar healthbar;
    public float sightRange;
    public float attackRange;
    public bool playerInSightRange;
    public bool playerInAttackRange;
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private GameObject BulletSpawnpoint;
    [SerializeField]
    private float bulletSpeed = 700F;
    public float projectilespeed;

    public Transform powerupSpawnPoint;
    public GameObject[] powerUpPrefab;

    public Transform[] waypoints;
    int waypointIndex;

    public float distThreshold;

    public enum EnemyState
    {
        Patrol,
        Chase,
        Attack
    }
    public EnemyState currentState = EnemyState.Patrol;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        currenthealth = maxHealth;

        if (projectilespeed <= 0)
            projectilespeed = 15.0f;

        if (!powerupSpawnPoint || powerUpPrefab.Length == 0)
            Debug.Log("Please set up default values on " + gameObject.name);

        if (distThreshold <= 0) distThreshold = 0.5f;

        if (waypoints.Length == 0) waypoints = GameObject.FindGameObjectsWithTag("Patrol").Select(obj => obj.transform).ToArray();

        SetNextWaypoint();
    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, WhatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, WhatIsPlayer);

        if (currentState == EnemyState.Patrol)
        {
            if (!agent.pathPending && agent.remainingDistance < distThreshold)
                SetNextWaypoint();

            if (playerInSightRange)
            {
                currentState = EnemyState.Chase;
                agent.SetDestination(player.position);
            }
        }
        else if (currentState == EnemyState.Chase)
        {
            if (!playerInSightRange)
            {
                currentState = EnemyState.Patrol;
                SetNextWaypoint();
            }

            if (playerInAttackRange)
            {
                currentState = EnemyState.Attack;
                StartCoroutine(AttackPlayer());
            }
        }
    }

    private void SetNextWaypoint()
    {
        waypointIndex = (waypointIndex + 1) % waypoints.Length;
        agent.SetDestination(waypoints[waypointIndex].position);
        
    }

    private IEnumerator AttackPlayer()
    {
        while (playerInAttackRange)
        {
            GameObject bullet = Instantiate(bulletPrefab, BulletSpawnpoint.transform.position, transform.rotation);
            bullet.GetComponent<Rigidbody>().AddForce(transform.forward * bulletSpeed);
            yield return new WaitForSeconds(timeBetweenAttacks);
        }

        currentState = EnemyState.Chase;
    }

    void Death()
    {
        anim.SetTrigger("Dead");
        agent.speed = 0f;
    }

    public void TakeDamage(int damage)
    {
        currenthealth -= damage;
        healthbar.SetHealth(currenthealth);
        if (currenthealth <= 0)
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
