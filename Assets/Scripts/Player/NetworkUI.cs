using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class NetworkUI : MonoBehaviour
{
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;

    private void Awake()
    {
        hostButton.onClick.AddListener(() =>
        {
            if (!NetworkManager.Singleton.IsHost && !NetworkManager.Singleton.IsClient)
            {
                NetworkManager.Singleton.StartHost();
                UpdateButtonInteractivity();
            }
        });

        clientButton.onClick.AddListener(() =>
        {
            if (!NetworkManager.Singleton.IsClient)
            {
                NetworkManager.Singleton.StartClient();
                UpdateButtonInteractivity();
            }
        });

        // Ensure buttons are correctly updated when the scene loads
        UpdateButtonInteractivity();
    }

    private void UpdateButtonInteractivity()
    {
        // Ensure the buttons remain interactable even after the host starts
        hostButton.interactable = !NetworkManager.Singleton.IsHost;
        clientButton.interactable = !NetworkManager.Singleton.IsClient;
    }

    private void Update()
    {
        // Continuously check network state and update buttons dynamically
        UpdateButtonInteractivity();
    }
}
