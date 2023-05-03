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

    public GameObject Enemyskin;
    SkinnedMeshRenderer[] SkinnedMeshRenderer;

    public Material[] skin;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FindPlayer());
        StartCoroutine(FOVRoutine());
        Renderer renderer = Enemyskin.GetComponent<Renderer>();
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

    // menos pesado al engine 
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
            Enemyskin.GetComponent<MeshRenderer>().material = skin[1];
            float distanceToPlayer = Vector3.Distance(transform.position, Player.position);
            if (distanceToPlayer <= radius)
            {
                enemy.SetDestination(Player.position);
            }
            else
            {
                enemy.ResetPath();

                
            }
            
                
        }
        if (canSeePlayer == false)
        {
            
        }

    }

   
}
