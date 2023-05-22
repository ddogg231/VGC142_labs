using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickups : MonoBehaviour
{
    public enum Pickuptype
    {
     Jump = 0,
     life = 1,
     Speed = 2,
    }

    public Pickuptype currentPickup;
        
    public object PickupType { get; private set; }

    private void OnTriggerEnter(Collider collision)
    {
      if (collision.gameObject.CompareTag("Player"))
      {
        Playercontroller temp = collision.gameObject.GetComponent<Playercontroller>();
   
        switch (currentPickup)
        {
          case Pickuptype.Jump:
              collision.gameObject.GetComponent<Playercontroller>().StartJumpForceChange();
              break;

          case Pickuptype.life:
              collision.gameObject.GetComponent<Playercontroller>().health++;
              break;

          case Pickuptype.Speed:
              temp.StartSpeedChange();
              break;

        }
       Destroy(gameObject);
      }
    }
}

