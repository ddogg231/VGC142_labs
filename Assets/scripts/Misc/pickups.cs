using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickups : MonoBehaviour
{
        public enum Pickuptype
        {
            
        }

        public Pickuptype currentPickup;
        public AudioClip picksound;
        public object PickupType { get; private set; }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                

               

                if (picksound)

                Destroy(gameObject);
            }
        }
    }

