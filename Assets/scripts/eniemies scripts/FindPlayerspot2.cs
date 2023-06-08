using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class FindPlayerspot2 : MonoBehaviour
{
    public EnemyController EnemyController;
    public Transform player;
    public bool canSeePlayer;

    public GameObject playerRef;
    public float radius;
    [Range(0, 360)]
    public float angle;

    public LayerMask targetMask;
    public LayerMask obstructionMask;
    public float maxDistance = 20f;

    public SkinnedMeshRenderer meshRenderer;
    public Material[] materials;
    private bool playerLookingAtEnemy;
    void Start()
    {
        StartCoroutine(FindPlayer());
        StartCoroutine(FOVRoutine());
    }
  
    private IEnumerator FindPlayer()
    {
        yield return new WaitForSeconds(1f); // Wait for 1 second

        playerRef = GameObject.FindGameObjectWithTag("Player");
        player = playerRef.transform;

        Debug.Log("playerRef: " + playerRef); // Debug statement
        Debug.Log("Player: " + player); // Debug statement

        EnemyController = GetComponent<EnemyController>();
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
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (distanceToTarget <= maxDistance && !Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
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

    void Update()
    {
        if (canSeePlayer)
        {
            if (!EnemyController) return;

            if (EnemyController.currentState == EnemyController.EnemyState.Patrol)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, player.position);
                if (distanceToPlayer <= radius)
                {
                    meshRenderer.material = materials[0];
                    EnemyController.currentState = EnemyController.EnemyState.Chase;
                }
            }
            else if (EnemyController.currentState == EnemyController.EnemyState.Chase)
            {
                Vector3 directionToPlayer = (player.position - transform.position).normalized;
                float dotProduct = Vector3.Dot(transform.forward, directionToPlayer);

                if (dotProduct >= 0.5f)
                {
                    EnemyController.agent.SetDestination(player.position);

                    if (EnemyController.playerInAttackRange)
                    {
                        EnemyController.currentState = EnemyController.EnemyState.Attack;
                        StartCoroutine(AttackPlayer());
                    }
                }
                else
                {
                    EnemyController.currentState = EnemyController.EnemyState.Patrol;
                }
            }
        }
        else
        {
            if (EnemyController && EnemyController.currentState == EnemyController.EnemyState.Chase)
                EnemyController.currentState = EnemyController.EnemyState.Patrol;

            meshRenderer.material = materials[1];
        }
    }

    private IEnumerator AttackPlayer()
    {
        while (canSeePlayer && EnemyController.currentState == EnemyController.EnemyState.Attack)
        {
            // Attack player code here
            yield return null;
        }
    }
}
