using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class FindPlayerspot : MonoBehaviour
{
    public NavMeshAgent enemy;
    public Transform Player;
    public bool canSeePlayer;

    public GameObject playerRef;
    public float radius;
    [Range(0, 360)]
    public float angle;

    public LayerMask targetMask;
    public LayerMask obstructionMask;
    public float maxDistance = 20f;



    private Transform playerTransform;
    public SkinnedMeshRenderer meshRenderer;
    public Material[] materials;
   

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FindPlayer());
        StartCoroutine(FOVRoutine());
       


        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
       
    }

    private IEnumerator FindPlayer()
    {
        yield return new WaitForSeconds(1f); // wait for 1 second

        playerRef = GameObject.FindGameObjectWithTag("Player");
        Player = playerRef.transform;

        Debug.Log("playerRef: " + playerRef); // Debug statement
        Debug.Log("Player: " + Player); // Debug statement
        Debug.Log($"Player instance: {GameManager.Instance.playerInstance}");
    }

    
    private IEnumerator FOVRoutine()
    {

        float delay = 0.2f;
        WaitForSeconds wait = new WaitForSeconds(delay);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
           
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distancetoTarget = Vector3.Distance(transform.position, target.position);

                if (distancetoTarget <= maxDistance && !Physics.Raycast(transform.position, directionToTarget, distancetoTarget, obstructionMask))
                    canSeePlayer = true;
                else
                    canSeePlayer = false;
            }
            else
                canSeePlayer = false;
        }
        else if (canSeePlayer)
            canSeePlayer = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (canSeePlayer == true)
        {
            
            float distanceToPlayer = Vector3.Distance(transform.position, Player.position);
            if (distanceToPlayer <= radius)
            {
                meshRenderer.material = materials[0];
                enemy.SetDestination(Player.position);
            }
            else
            {
                enemy.ResetPath();

            }
            
                
        }
        if (canSeePlayer == false)
        {
            meshRenderer.material = materials[1];
        }

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

}
/*public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;*/