using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{

    public Transform weaponAttachPoint;

    private Weapon weapon;

    public float weaponDropForce;


    // Start is called before the first frame update
    void Start()
    {
        weapon = null;

        if (!weaponAttachPoint)
            weaponAttachPoint = GameObject.FindGameObjectWithTag("AttachPoint").transform;

        if (weaponDropForce <= 0)
            weaponDropForce = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void fire()
    {

    }
}
