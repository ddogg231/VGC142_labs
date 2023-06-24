using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomMovement : MonoBehaviour
{
    public NavMeshAgent agent;
    public float range; // Radius of the sphere
    public float detectionRange; // Range to detect the player

    public Transform centrePoint; // Centre of the area the agent wants to move around in

    private Transform player;
    private bool isPlayerInRange;
    

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (isPlayerInRange)
        {
            // Stop moving
            agent.isStopped = true;
            
        }
        else if (agent.remainingDistance <= agent.stoppingDistance)
        {
            // Done with the current path, generate a new random point
            Vector3 point;
            if (RandomPoint(centrePoint.position, range, out point))
            {
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); // So you can see with gizmos
                agent.SetDestination(point);
            }
        }

        // Check if the player is within the detection range
        if (Vector3.Distance(transform.position, player.position) <= detectionRange)
        {
            isPlayerInRange = true;
        }
        else
        {
            isPlayerInRange = false;
        }
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    
}

//public class RandomMovement : MonoBehaviour //don't forget to change the script name if you haven't
//{
//    public NavMeshAgent agent;
//    public float range; //radius of sphere
//
//    public Transform centrePoint; //centre of the area the agent wants to move around in
//    //instead of centrePoint you can set it as the transform of the agent if you don't care about a specific area
//
//    void Start()
//    {
//        agent = GetComponent<NavMeshAgent>();
//    }
//
//
//    void Update()
//    {
//        if (agent.remainingDistance <= agent.stoppingDistance) //done with path
//        {
//            Vector3 point;
//            if (RandomPoint(centrePoint.position, range, out point)) //pass in our centre point and radius of area
//            {
//                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); //so you can see with gizmos
//                agent.SetDestination(point);
//            }
//        }
//
//    }
//    bool RandomPoint(Vector3 center, float range, out Vector3 result)
//    {
//
//        Vector3 randomPoint = center + Random.insideUnitSphere * range;  
//        NavMeshHit hit;
//        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)) 
//        {
//            
//            result = hit.position;
//            return true;
//        }
//
//        result = Vector3.zero;
//        return false;
//    }
//
//
//}
//