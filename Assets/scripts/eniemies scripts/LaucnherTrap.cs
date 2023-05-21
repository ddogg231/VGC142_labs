using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaucnherTrap : MonoBehaviour
{
    public float launchForce = 10f;
    public float launchDuration = 0.5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterController characterController = other.GetComponent<CharacterController>();
            if (characterController != null)
            {
                // Disable player control during the launch
                Playercontroller playerController = other.GetComponent<Playercontroller>();
                if (playerController != null)
                {
                    playerController.enabled = false;
                }

                // Launch the player
                Vector3 launchDirection = transform.up.normalized;
                StartCoroutine(LaunchPlayer(characterController, launchDirection));
            }
        }
    }

    private IEnumerator LaunchPlayer(CharacterController characterController, Vector3 launchDirection)
    {
        float elapsedTime = 0f;
        Vector3 initialPosition = characterController.transform.position;
        float launchHeight = launchDirection.y * launchForce;

        while (elapsedTime < launchDuration)
        {
            float normalizedTime = elapsedTime / launchDuration;
            float verticalOffset = Mathf.Sin(normalizedTime * Mathf.PI) * launchHeight;

            characterController.Move(launchDirection * launchForce * Time.deltaTime);
            characterController.Move(Vector3.up * verticalOffset * Time.deltaTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Re-enable player control after the launch
        Playercontroller playerController = characterController.GetComponent<Playercontroller>();
        if (playerController != null)
        {
            playerController.enabled = true;
        }
    }

}
