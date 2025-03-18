using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class NetworkUI : MonoBehaviour
{
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;
    [SerializeField] private Camera mainMenuCamera; // Assign this in the Inspector

    private void Awake()
    {
        if (hostButton == null || clientButton == null)
        {
            Debug.LogError("❌ UI Buttons are not assigned in the Inspector!");
            return;
        }

        hostButton.onClick.AddListener(() =>
        {
            if (!NetworkManager.Singleton.IsHost && !NetworkManager.Singleton.IsClient)
            {
                NetworkManager.Singleton.StartHost();
                DisableMainMenuCamera();
                UpdateButtonInteractivity();
            }
        });

        clientButton.onClick.AddListener(() =>
        {
            if (!NetworkManager.Singleton.IsClient)
            {
                NetworkManager.Singleton.StartClient();
                DisableMainMenuCamera();
                UpdateButtonInteractivity();
            }
        });

        UpdateButtonInteractivity();
    }

    private void Update()
    {
        UpdateButtonInteractivity();
    }

    private void UpdateButtonInteractivity()
    {
        if (NetworkManager.Singleton == null)
        {
            Debug.LogError("❌ NetworkManager is missing! Ensure it's in the scene.");
            return;
        }

        hostButton.interactable = !NetworkManager.Singleton.IsHost;
        clientButton.interactable = !NetworkManager.Singleton.IsClient;
    }

    private void DisableMainMenuCamera()
    {
        if (mainMenuCamera != null)
        {
            mainMenuCamera.gameObject.SetActive(false);
        }
    }
}
