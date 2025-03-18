using TMPro;
using UnityEngine;

public class DeathDisplay : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI deathText;

    private void Awake()
    {
        // Check if we've assigned the reference
        if (deathText == null)
        {
            Debug.LogError("Death Text UI element is not assigned!");
        }
        else
        {
            // Initialize text to "Deaths: 0" on Awake
            deathText.text = "Deaths: 0";
        }
    }

    public void UpdateDeathText(int currentDeaths)
    {
        // If the reference is valid, update the text
        if (deathText != null)
        {
            // Use simple concatenation or string interpolation:
            deathText.text = "Deaths: " + currentDeaths;
            // or
            // deathText.text = $"Deaths: {currentDeaths}";
        }
    }
}
