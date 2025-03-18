using UnityEngine;
using Unity.Netcode;

public class PersistentNetworkManager : MonoBehaviour
{
    private void Awake()
    {
        // Check if another NetworkManager already exists
        if (Object.FindObjectsByType<NetworkManager>(FindObjectsSortMode.None).Length > 1)
        {
            Debug.LogWarning("⚠️ Another NetworkManager already exists! Destroying this one.");
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
}
