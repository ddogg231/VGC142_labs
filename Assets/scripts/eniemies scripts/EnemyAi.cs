using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{

    public Material[] Skin;
    public NavMeshAgent agent;
    public Transform player;

    public LayerMask WhatIsGround, WhatIsPlayer;

    public float health;

    /*public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;*/

    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    public float sightRange, attackRange;
    public bool playerInsightRange, playerInattackRange;

    Rigidbody rb;

    public CharacterController playerInstance;


    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        playerInsightRange = Physics.CheckSphere(transform.position, sightRange, WhatIsPlayer);
        playerInattackRange = Physics.CheckSphere(transform.position, attackRange, WhatIsPlayer);
        if (playerInsightRange && playerInattackRange) AttackPlayer();

        

    }

    /*private void Partoling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, WhatIsGround))
            walkPointSet = true;
        Debug.DrawRay(walkPoint, transform.forward, Color.red);


    }

    private void ChasePlayer()
    {
        if (playerInsightRange == true)
        {

            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer <= sightRange)
            {
                agent.SetDestination(player.position);
            }
            else
            {
                agent.ResetPath();
            }

        }
    }*/

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

    }
   
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
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
