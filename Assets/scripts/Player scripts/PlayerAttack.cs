using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    private Playercontroller _input;
  
    public Transform weaponAttachPoint;
    private Weapon weapon;

    public float weaponDropForce;

   

    // Start is called before the first frame update
    void Start()
    {
        _input = transform.root.GetComponent<Playercontroller>();
        weapon = null;

        if (!weaponAttachPoint)
            weaponAttachPoint = GameObject.FindGameObjectWithTag("AttachPoint").transform;

        if (weaponDropForce <= 0)
            weaponDropForce = 10f;
    }

    // Update is called once per frame
   private void Update()
    {

        if (_input.FireAction.triggered)
        {

            _input.fire = true;

            weapon = GetComponentInChildren<Weapon>();
            if (weapon != null)
            {
                weapon.Fire();
            }

            _input.fire = false;
            Debug.Log("got this far");
        }
         
         if(_input.ThrowAction.triggered)
         {
            _input.Throw = true;

             Throw();

             _input.Throw = false;
         }
    }

    public void AssignWeapon(Weapon newWeapon)
    {
        weapon = newWeapon;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!weapon && other.gameObject.CompareTag("Weapon"))
        {
            weapon = other.gameObject.GetComponent<Weapon>();
            
            weapon.rb.isKinematic = true;
            weapon.gameObject.transform.SetParent(weaponAttachPoint);
            weapon.gameObject.transform.SetPositionAndRotation(weaponAttachPoint.position, weaponAttachPoint.rotation);
            Physics.IgnoreCollision(weapon.gameObject.GetComponent<Collider>(), GetComponent<Collider>(), true);
    
           
            Debug.Log("weapon picked up");
        }
    }

// private void OnCollisionEnter(Collision collision)
// {
//     if (!weapon && collision.gameObject.CompareTag("Weapon")) 
//     {
//         weapon = collision.gameObject.GetComponent<Weapon>();
//
//         if (!weapon) return;
//
//         weapon.rb.isKinematic = true;
//         weapon.gameObject.transform.SetParent(weaponAttachPoint);
//         weapon.gameObject.transform.SetPositionAndRotation(weaponAttachPoint.position, weaponAttachPoint.rotation);
//         Physics.IgnoreCollision(weapon.gameObject.GetComponent<Collider>(), GetComponent<Collider>(), true);
//     }
//     Debug.Log("weapon hit");
// }

    public void Throw()
    {
        if(_input.Throw && weapon)
        {
            
            weaponAttachPoint.DetachChildren();
            StartCoroutine(EnableCollisions(2));

            weapon.rb.isKinematic = false;
            weapon.rb.AddForce(weapon.transform.forward * weaponDropForce, ForceMode.Impulse);
        }
    }
    
    IEnumerator EnableCollisions(float timeToDisable)
    {
        yield return new WaitForSeconds(timeToDisable);
        Physics.IgnoreCollision(weapon.gameObject.GetComponent<Collider>(), GetComponent<Collider>(), true);
        
        weapon = null;
    }
}
