using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boolikecomp : MonoBehaviour
{
    
    public GameObject player;

    public Material material;

    public float distanceThreshold = 5f;

    public float invisibleTransparency = 0.5f;
    

    private float currentTransparency;
    private float targetTransparency;
 

    private Vector3 directionToPlayer;

 
    void Start()
    {
      
        currentTransparency = material.color.a;
        targetTransparency = currentTransparency;
    }

    // Update is called once per frame
    void Update()
    {
       
        directionToPlayer = player.transform.position - transform.position;

        
        float dotProduct = Vector3.Dot(transform.forward, directionToPlayer.normalized);

        if (dotProduct > 0 && directionToPlayer.magnitude <= distanceThreshold)
        {
            targetTransparency = 1f;
        }
        else
        {
            targetTransparency = invisibleTransparency;
        }

        if (targetTransparency != currentTransparency)
        {
            
        }

       
    }
}
