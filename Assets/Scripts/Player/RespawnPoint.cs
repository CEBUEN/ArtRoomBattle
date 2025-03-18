using UnityEngine;

public class RespawnSystem : MonoBehaviour
{
    public Transform respawnPoint; 

    // Keep track of total deaths here
    private int totalDeaths = 0;

    public void HandlePlayerDeath(GameObject player)
    {
        totalDeaths++;
        Debug.Log($"Total deaths so far: {totalDeaths}");

        // Update death UI
        DeathDisplay deathDisplay = FindFirstObjectByType<DeathDisplay>();
        if (deathDisplay != null)
        {
            deathDisplay.UpdateDeathText(totalDeaths);
        }

        // Then respawn
        RespawnPlayer(player);
    }

    public void RespawnPlayer(GameObject player)
    {
        if (respawnPoint != null && player != null)
        {
            // Temporarily disable player
            player.SetActive(false);

            // Move them to respawn position
            player.transform.position = respawnPoint.position;
            player.transform.rotation = respawnPoint.rotation;

            // Reset any Rigidbody velocities
            Rigidbody rb = player.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            // Re-enable the player
            player.SetActive(true);

            // Reset health (only if player has PlayerHealth)
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.ResetHealth();
            }
        }
        else
        {
            Debug.LogError("Respawn point or player is missing!");
        }
    }
}
