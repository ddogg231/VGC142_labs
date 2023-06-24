using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public Transform weaponAttachPoint;
    public float weaponDropForce;
    void Start()
    {
        if (!weaponAttachPoint)
            weaponAttachPoint = GameObject.FindGameObjectWithTag("AttachPoint").transform;

        if (weaponDropForce <= 0)
            weaponDropForce = 10f;
    }
}
