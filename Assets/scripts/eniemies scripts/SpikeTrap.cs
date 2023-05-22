using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    public int damageAmount = 10;
    public float cooldownDuration = 5f;

    private bool isArmed = true;
    private float cooldownTimer = 0f;

    private void OnTriggerEnter(Collider other)
    {
        if (!isArmed || !IsValidTarget(other.gameObject))
            return;

       
        ApplyDamage(other.gameObject);

       
        isArmed = false;

        
        cooldownTimer = cooldownDuration;
    }

    private void Update()
    {
        if (!isArmed)
        {
            
            cooldownTimer -= Time.deltaTime;

            if (cooldownTimer <= 0f)
            {
                
                isArmed = true;
            }
        }
    }

    private bool IsValidTarget(GameObject target)
    {
        
        return target.CompareTag("Player")|| CompareTag("Enemy");
    }

    private void ApplyDamage(GameObject target)
    {
        
        Playercontroller health = target.GetComponent<Playercontroller>();
        EnemyAi healthComponent = target.GetComponent<EnemyAi>();
        if (healthComponent != null)
        {
            healthComponent.TakeDamage(damageAmount);
        }
        if (health != null)
        {
            health.TakeDamage(damageAmount);
        }
    }
}
