 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tankshell : MonoBehaviour
{
    private System.Action<Vector3> explosionCallback;

    public void SetExplosionCallback(System.Action<Vector3> callback)
    {
        explosionCallback = callback;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (explosionCallback != null)
        {
            // Call the explosion callback with the collision point
            explosionCallback.Invoke(collision.contacts[0].point);
        }

        Destroy(gameObject);
    }
   
}
