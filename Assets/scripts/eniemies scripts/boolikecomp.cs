using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boolikecomp : MonoBehaviour
{
    
    public GameObject player;

    public Material material;
    public float distanceThreshold = 5f;
    public float invisibleTransparency = 0.5f;
    public float fadeTime = 0.5f;
    private float currentTransparency;
    private float targetTransparency;
    private float elapsedTime;
    private bool isFading;
    private Vector3 directionToPlayer;

    // Start is called before the first frame update
    void Start()
    {
        // Set the initial transparency of the Boo's material
        currentTransparency = material.color.a;
        targetTransparency = currentTransparency;
    }

    // Update is called once per frame
    void Update()
    {
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
}
