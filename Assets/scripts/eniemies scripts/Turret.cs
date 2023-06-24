using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform player; // Reference to the player's transform
    public Transform turretHead; // Reference to the turret's head (the part that rotates)
    public Transform gunBarrel1;
    public Transform gunBarrel2;
    public float rotationSpeed = 5f; // Speed at which the turret head rotates towards the player
    public float fireRate = 1f; // Time delay between each shot
    public float shotRange = 10f; // Maximum distance the turret can shoot
    private bool useGunBarrel1 = true;
    private bool canShoot = true; // Flag to control the firing rate
    public GameObject[] powerUpPrefab;
    public int maxHealth = 30;
    public int currentHealth = 30;
    public Transform powerupSpawnPoint;
    public int damage;
    private bool isDestroyed;
    public ParticleSystem death;
    public ParticleSystem lazer;
    public Healthbar healthbar;

    public Transform groundCheck;
    public float groundCheckDistance = 0.2f;
    public LayerMask groundLayer;
    private bool isGrounded;
    private void Update()
    {
        if (!isDestroyed && player != null)
        {
            // Rotate the turret head towards the player
            Vector3 directionToPlayer = player.position - turretHead.position;
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            turretHead.rotation = Quaternion.Slerp(turretHead.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Check if the player is within the shooting range
            if (directionToPlayer.magnitude <= shotRange)
            {
                // Fire a raycast towards the player
                if (canShoot)
                {
                    FireShot();
                    canShoot = false;
                    Invoke("ResetShot", 1f / fireRate);
                }
            }
        }
    }
    private void FireShot()
    {
        // Perform raycast to detect if the shot hits the player
        RaycastHit hit;
        if (Physics.Raycast(turretHead.position, turretHead.forward, out hit, shotRange))
        {
            if (hit.collider.CompareTag("Player"))
            {
                Debug.Log("Shot hit the player!");
                // Apply damage to the player
                Playercontroller playerHealth = hit.collider.GetComponent<Playercontroller>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damage); // Adjust the damage value as needed
                }
            }
        }
        if (lazer != null)
        {
            Transform activeGunBarrel = useGunBarrel1 ? gunBarrel1 : gunBarrel2;
            Instantiate(lazer, activeGunBarrel.position, activeGunBarrel.rotation);
        }
        useGunBarrel1 = !useGunBarrel1;
    }


    private void ResetShot()
    {
        canShoot = true;
    }
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthbar.SetHealth(currentHealth);
        if (currentHealth <= 0)
        {
            isDestroyed = true;
            
            StartCoroutine(DeathSequence());
            enabled = false;
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
    private void OnDrawGizmosSelected()
    {
        // Draw a wire sphere to represent the shooting range in the scene view
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, shotRange);
    }
}
