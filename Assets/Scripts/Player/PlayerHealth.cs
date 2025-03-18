using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Respawn Settings")]
    public RespawnSystem respawnSystem;

    private void Start()
    {
        currentHealth = maxHealth;

        // If not assigned, try to auto-find the RespawnSystem
        if (respawnSystem == null)
        {
            respawnSystem = FindFirstObjectByType<RespawnSystem>();
        }

        if (respawnSystem == null)
        {
            Debug.LogError("No RespawnSystem found! Assign one in the Inspector.");
        }
    }

    private void Update()
    {
        // Press Y to kill the player instantly, for testing
        if (Input.GetKeyDown(KeyCode.Y))
        {
            currentHealth = 0;
            Die();
        }
    }

    public void TakeDamage(int damage)
    {
        if (damage <= 0) return;

        currentHealth -= damage;
        Debug.Log($"Player took {damage} damage. Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player died!");

        // Use the RespawnSystem to handle death logic (increment death count, respawn, etc.)
        if (respawnSystem != null)
        {
            respawnSystem.HandlePlayerDeath(gameObject);
        }
        else
        {
            Debug.LogError("RespawnSystem is missing! Player cannot respawn.");
        }
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        Debug.Log("Player health reset.");
    }
}
