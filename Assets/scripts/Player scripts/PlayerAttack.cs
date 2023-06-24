using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerAttack : MonoBehaviour
{
    private Playercontroller _input;

    public Transform weaponAttachPoint;
    public Weapon activeWeapon; // The currently active weapon
    public List<Weapon> weapons;
    public Weapon weapon;
    public Weapon droppedWeapon;

    private bool isFiring = false; 
    public bool canChangeWeapon = true;
    public float weaponChangeCooldown = 0.2f;

    public float weaponPickupCooldown = 2f;
    private bool canPickupWeapon = true;
    public int maxWeapons = 2;

    // Start is called before the first frame update
    void Start()
    {
        _input = transform.root.GetComponent<Playercontroller>();
        weapons = new List<Weapon>();

        if (!weaponAttachPoint)
            weaponAttachPoint = GameObject.FindGameObjectWithTag("AttachPoint").transform;

        
    }

    // Update is called once per frame
    private void Update()
    {
        if (_input.FireAction.triggered && !isFiring)
        {
            if (activeWeapon != null)
            {
                StartCoroutine(FireCoroutine(activeWeapon));
            }

            Debug.Log("Fired");
        }

        if (_input.ThrowAction.triggered)
        {
            if (weapons.Count > 0)
            {
                DropActiveWeapon();
                _input.selectedWeapon = Mathf.Clamp(_input.selectedWeapon, 0, weapons.Count - 1);

                StartCoroutine(WeaponPickupCooldown());
            }
            else if (weapon != null)
            {
                DropWeapon();
                
            }
        }

        float scrollValue = _input.ScrollAction.ReadValue<float>();
        if (scrollValue > 0 && canChangeWeapon)
        {
            _input.SelectNextWeapon();
        }
        else if (scrollValue < 0 && canChangeWeapon)
        {
            _input.SelectPreviousWeapon();
        }

        
    }
    public void SelectWeapon(int index)
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            if (i == index)
            {
                weapons[i].gameObject.SetActive(true);
                activeWeapon = weapons[i];
            }
            else
            {
                weapons[i].gameObject.SetActive(false);
            }
        }
    }
    public void AssignWeapon(Weapon newWeapon)
    {
        weapons.Add(newWeapon);

        if (activeWeapon != null)
        {
            DropActiveWeapon();
        }
        if (weapons.Count > 1)
        {
            weapons[1].gameObject.SetActive(false);
        }

        activeWeapon = newWeapon;
        newWeapon.gameObject.transform.SetParent(weaponAttachPoint);
        newWeapon.gameObject.transform.SetPositionAndRotation(weaponAttachPoint.position, weaponAttachPoint.rotation);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (weapons.Count < maxWeapons && canPickupWeapon  && !weapon && other.gameObject.CompareTag("Weapon"))
        {
            weapon = other.gameObject.GetComponent<Weapon>();
            
            weapon.rb.isKinematic = true;
            weapon.gameObject.transform.SetParent(weaponAttachPoint);
            weapon.gameObject.transform.SetPositionAndRotation(weaponAttachPoint.position, weaponAttachPoint.rotation);
            Physics.IgnoreCollision(weapon.gameObject.GetComponent<Collider>(), GetComponent<Collider>(), true);

            AssignWeapon(weapon);
            Debug.Log("weapon picked up");
            if (weapons.Count == maxWeapons)
            {
                // Disable further weapon pickups if the maximum limit is reached
                canPickupWeapon = false;
            }
        }
        
    }
    private void DropActiveWeapon()
    {
        if (activeWeapon != null)
        {
            weapons.Remove(activeWeapon);
            activeWeapon.transform.SetParent(null);
            activeWeapon.rb.isKinematic = false;
            activeWeapon.rb.AddForce(transform.forward * activeWeapon.dropForce, ForceMode.Impulse);
            activeWeapon = null;
           
        }
    }
    private void DropWeapon()
    {
        if (weapon != null)
        {
            droppedWeapon = weapon;
            weapon = null;
            droppedWeapon.transform.SetParent(null);
            droppedWeapon.rb.isKinematic = false;
            droppedWeapon.rb.AddForce(transform.forward * droppedWeapon.dropForce, ForceMode.Impulse);
            droppedWeapon = null;
        }
    }
    private IEnumerator FireCoroutine(Weapon weapon)
    {
        isFiring = true;
        weapon.Fire();
        yield return new WaitForSeconds(weapon.fireRate);
        isFiring = false;
    }
    private IEnumerator WeaponPickupCooldown()
    {
        canPickupWeapon = false;
        yield return new WaitForSeconds(weaponPickupCooldown);
        canPickupWeapon = true;
    }
}
//public class PlayerAttack : MonoBehaviour
//{
//    private Playercontroller _input;
//  
//    public Transform weaponAttachPoint;
//    private Weapon weapon;
//    
//    private List<Weapon> weapons;
//    public float weaponDropForce;
//    private bool isFiring = false;
//
//
//    // Start is called before the first frame update
//    void Start()
//    {
//        _input = transform.root.GetComponent<Playercontroller>();
//        weapon = null;
//        
//        weapons = new List<Weapon>();
//
//        if (!weaponAttachPoint)
//            weaponAttachPoint = GameObject.FindGameObjectWithTag("AttachPoint").transform;
//
//        if (weaponDropForce <= 0)
//            weaponDropForce = 10f;
//    }
//
//    // Update is called once per frame
//   private void Update()
//    {
//
//        if (_input.FireAction.triggered && !isFiring)
//        {
//
//            
//
//            weapon = GetComponentInChildren<Weapon>();
//            if (weapon != null)
//            {
//                _input.fire = true;
//                Weapon activeWeapon = weapons[_input.selectedWeapon];
//                StartCoroutine(FireCoroutine(activeWeapon));
//            }
//
//            _input.fire = false;
//            Debug.Log("Fired");
//        }
//         
//         if(_input.ThrowAction.triggered)
//         {
//            _input.Throw = true;
//
//            if (weapons.Count > 0)
//            {
//                Weapon activeWeapon = weapons[_input.selectedWeapon];
//                Throw(activeWeapon);
//                weapons.Remove(activeWeapon);
//                _input.selectedWeapon = Mathf.Clamp(_input.selectedWeapon, 0, weapons.Count - 1);
//            }
//
//            _input.Throw = false;
//         }
//    }
//
//    public void AssignWeapon(Weapon newWeapon)
//    {
//        weapons.Add(newWeapon);
//       
//    }
//
//    private void OnTriggerEnter(Collider other)
//    {
//        if (!weapon && other.gameObject.CompareTag("Weapon"))
//        {
//            weapon = other.gameObject.GetComponent<Weapon>();
//            
//            weapon.rb.isKinematic = true;
//            weapon.gameObject.transform.SetParent(weaponAttachPoint);
//            weapon.gameObject.transform.SetPositionAndRotation(weaponAttachPoint.position, weaponAttachPoint.rotation);
//            Physics.IgnoreCollision(weapon.gameObject.GetComponent<Collider>(), GetComponent<Collider>(), true);
//
//            AssignWeapon(weapon);
//            Debug.Log("weapon picked up");
//        }
//    }
//
//// private void OnCollisionEnter(Collision collision)
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
//
//    public void Throw(Weapon weapon)
//    {
//        if(_input.Throw && weapon)
//        {
//            
//            weaponAttachPoint.DetachChildren();
//            StartCoroutine(EnableCollisions(2));
//
//            weapon.rb.isKinematic = false;
//            weapon.rb.AddForce(weapon.transform.forward * weaponDropForce, ForceMode.Impulse);
//        }
//    }
//    
//    IEnumerator EnableCollisions(float timeToDisable)
//    {
//        yield return new WaitForSeconds(timeToDisable);
//        Physics.IgnoreCollision(weapon.gameObject.GetComponent<Collider>(), GetComponent<Collider>(), true);
//        
//        weapon = null;
//    }
//    private IEnumerator FireCoroutine(Weapon weapon)
//    {
//        isFiring = true;
//        weapon.Fire();
//        yield return new WaitForSeconds(weapon.fireRate);
//        isFiring = false;
//    }
//}
//make a firing range lvl for A2