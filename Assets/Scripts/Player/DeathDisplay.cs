using TMPro;
using UnityEngine;

public class DeathDisplay : MonoBehaviour
{
    [Header("References")]
    public PlayerHealth playerHealth;
    public TextMeshProUGUI deathText;

    private void Start()
    {
        // Auto-find the PlayerHealth if not assigned
        if (playerHealth == null)
        {
            playerHealth = FindFirstObjectByType<PlayerHealth>();
            Debug.Log($"PlayerHealth found: {playerHealth != null}");
        }

        if (deathText == null)
        {
            Debug.LogError("Death Text UI element is not assigned!");
        }
        else
        {
            Debug.Log("Death Text UI element is assigned.");
        }

        // Initialize the UI with the current death count
        RefreshDeath();
    }

    public void RefreshDeath()
    {
        // Update the death text UI
        if (playerHealth != null && deathText != null)
        {
            deathText.text = $"Deaths: {playerHealth.GetDeathCount()}";
            Debug.Log($"Deaths updated to: {playerHealth.GetDeathCount()}");
        }
        else
        {
            Debug.LogError("PlayerHealth or DeathText is missing!");
        }
    }
}