using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankTurret : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform player; // Reference to the player's transform
    public Transform turretHead; // Reference to the turret's head (the part that rotates)
    public Transform gunBarrel;
   
    public float rotationSpeed = 5f; // Speed at which the turret head rotates towards the player
    public float fireRate = 1f; // Time delay between each shot
    public float shotRange = 10f; // Maximum distance the turret can shoot

    public GameObject[] powerUpPrefab;
    public int maxHealth = 30;
    public int currentHealth = 30;
    public Transform powerupSpawnPoint;
    public int damage;
    private bool isDestroyed;
    
    
    public Healthbar healthbar;

    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;
    public float explosionRadius = 5f;
    public int explosionDamage = 10;
    public GameObject explosionEffect;

    public int score = 50;
    public float detectionRange = 60f;
 
    public float maxBarrelElevation = 10f;
    public float firingConeAngle = 5f;
    public Transform firingPoint;
    public float fireCooldown = 1f;

    private float lastFireTime;
    public RandomMovement AImove;

    public ParticleSystem death;
    public ParticleSystem fire;   
    public AudioClip fireSFx;
    public AudioClip deathSFX;

    private void Update()
    {
        if (player == null)
            return;

        // Check if player is within detection range
        if (Vector3.Distance(transform.position, player.position) <= detectionRange)
        {
            // Rotate the turret towards the player
            Quaternion targetRotation = Quaternion.LookRotation(player.position - turretHead.position, Vector3.up);
            turretHead.rotation = Quaternion.RotateTowards(turretHead.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Calculate angle between gun barrel and player
            Vector3 toPlayer = player.position - gunBarrel.position;
            float angle = Vector3.Angle(gunBarrel.forward, toPlayer);

            // Check if the angle is within the firing cone
            if (angle <= firingConeAngle)
            {
                // Elevate the gun barrel
                float elevationAngle = Mathf.Clamp(Vector3.SignedAngle(firingPoint.forward, toPlayer, firingPoint.right), -maxBarrelElevation, maxBarrelElevation);
                firingPoint.localRotation = Quaternion.Euler(elevationAngle, 0f, 0f);

                // Fire at the player
                if (Time.time - lastFireTime >= fireCooldown)
                {
                    ShootProjectile();
                    lastFireTime = Time.time;
                }
            }
        }
    }

    public void ShootProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, gunBarrel.position, gunBarrel.rotation);
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
        projectileRb.velocity = gunBarrel.forward * projectileSpeed;
        fire.Play();

        tankshell tankshell = projectile.GetComponent<tankshell>();
        if (tankshell != null)
        {
            tankshell.SetExplosionCallback(Explode);
        }
    }

   

    private void Explode(Vector3 explosionPosition)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                Playercontroller playerController = collider.GetComponent<Playercontroller>();
                if (playerController != null)
                {
                    playerController.TakeDamage(explosionDamage);
                }
            }
        }
        // Instantiate the explosion effect at the tank's position
        Instantiate(explosionEffect, explosionPosition, Quaternion.identity);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthbar.SetHealth(currentHealth);
        if (currentHealth <= 0)
        {
            isDestroyed = true;
            AImove.agent.speed = 0f;
            StartCoroutine(DeathSequence());
            
        }
    }

    private IEnumerator DeathSequence()
    {
        death.Play();
        yield return new WaitForSeconds(1f);

        Instantiate(powerUpPrefab[Random.Range(0, powerUpPrefab.Length)], powerupSpawnPoint.transform.position, Quaternion.identity);

        yield return new WaitForSeconds(2.0f);

        Destroy(gameObject);
    }
    
}
