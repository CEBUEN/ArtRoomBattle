using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100; 
    private int currentHealth;
    private bool isInvincible = false; 

    [Header("Respawn Settings")]
    public RespawnSystem respawnSystem; 

    // We now track death count instead of grade
    private int deathCount = 0;

    private void Start()
    {
        currentHealth = maxHealth;

        if (respawnSystem == null)
        {
            respawnSystem = FindFirstObjectByType<RespawnSystem>();
        }

        if (respawnSystem == null)
        {
            Debug.LogError("No RespawnSystem found! Assign one in the Inspector.");
        }
        
        Debug.Log("PlayerHealth initialized.");
    }

    private void Update()
    {
        // Press "Y" to cause instant death, for testing
        if (Input.GetKeyDown(KeyCode.Y))
        {
            TakeDamage(currentHealth);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible)
        {
            Debug.Log("Player is invincible! Damage ignored.");
            return;
        }

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

        // Increase the death count
        deathCount++;

        // Update the DeathDisplay (if it exists)
        DeathDisplay deathDisplay = FindFirstObjectByType<DeathDisplay>();
        if (deathDisplay != null)
        {
            deathDisplay.RefreshDeath();
        }

        // Respawn and reset health
        if (respawnSystem != null)
        {
            respawnSystem.RespawnPlayer(gameObject);
            ResetHealth();
        }
        else
        {
            Debug.LogError("RespawnSystem is missing! Player cannot respawn.");
        }
    }

    private void ResetHealth()
    {
        currentHealth = maxHealth;
        Debug.Log("Player health reset.");
    }

    public void Heal(int amount)
    {
        if (amount > 0)
        {
            currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
            Debug.Log($"Player healed by {amount}. Current health: {currentHealth}");
        }
    }

    // Simple invincibility
    public void StartInvincibility(float duration)
    {
        if (isInvincible) return;
        StartCoroutine(InvincibilityCoroutine(duration));
    }

    private System.Collections.IEnumerator InvincibilityCoroutine(float duration)
    {
        isInvincible = true;
        Debug.Log("Player is now invincible!");
        yield return new WaitForSeconds(duration);
        isInvincible = false;
        Debug.Log("Player is no longer invincible!");
    }

    public int GetDeathCount()
    {
        return deathCount;
    }
}
