using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask WhatIsGround, WhatIsPlayer;

    public float health;

    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    public float sightRange, attackRange;
    public bool playerInsightRange, playerInattackRange;

    Rigidbody rb;
    public Material material;
    public float distanceThreshold = 5f;
    public float invisibleTransparency = 0.5f;
    public float fadeTime = 0.5f;
    private float currentTransparency;
    private float targetTransparency;
    private float elapsedTime;
    private bool isFading;
    private Vector3 directionToPlayer;

    

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        playerInsightRange = Physics.CheckSphere(transform.position, sightRange, WhatIsPlayer);
        playerInattackRange = Physics.CheckSphere(transform.position, attackRange, WhatIsPlayer);

        if (!playerInsightRange && !playerInattackRange) Partoling();
        if (playerInsightRange && !playerInattackRange) ChasePlayer();
        if (playerInsightRange && playerInattackRange) AttackPlayer();
        // Calculate the direction from the Boo to the player
        directionToPlayer = player.transform.position - transform.position;

        // Calculate the dot product of the Boo's forward vector and the direction to the player
        float dotProduct = Vector3.Dot(transform.forward, directionToPlayer.normalized);

        // If the player is in front of the Boo and within the distance threshold, make the Boo visible
        if (dotProduct > 0 && directionToPlayer.magnitude <= distanceThreshold)
        {
            targetTransparency = 1f;
        }
        // Otherwise, make the Boo invisible
        else
        {
            targetTransparency = invisibleTransparency;
        }

        // If the Boo's target transparency is different from its current transparency, start fading
        if (targetTransparency != currentTransparency)
        {
            isFading = true;
        }

        // If the Boo is currently fading, update its transparency
        if (isFading)
        {
            // Calculate the new transparency using a lerp function based on the elapsed time and fade time
            float t = Mathf.Clamp01(elapsedTime / fadeTime);
            currentTransparency = Mathf.Lerp(currentTransparency, targetTransparency, t);

            // Update the Boo's material color with the new transparency
            Color color = material.color;
            color.a = currentTransparency;
            material.color = color;

            // Update the elapsed time
            elapsedTime += Time.deltaTime;

            // If the elapsed time has exceeded the fade time, stop fading
            if (elapsedTime >= fadeTime)
            {
                isFading = false;
                elapsedTime = 0f;
                currentTransparency = targetTransparency;
            }
        }
    }

    private void Partoling()
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
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(player);

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
