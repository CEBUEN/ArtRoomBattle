using Unity.Netcode;
using UnityEngine;

public class PlayerShooting : NetworkBehaviour
{
    public GameObject ammoPrefab; // Assign in Inspector
    public float shootForce = 20f;
    public float shootOffset = 1.5f; // Distance in front of the player
    public float verticalOffset = 1.0f; // Height offset for bullet spawning
    private Camera playerCamera; // This will be assigned dynamically

    [SerializeField] private AudioClip shootSound; // Assign in Inspector
    private AudioSource audioSource;

    void Start()
    {
        // Disable script for remote players
        if (!IsOwner)
        {
            enabled = false;
            return;
        }

        // Assign the correct camera for the local player
        playerCamera = GetComponentInChildren<Camera>(); // Finds the first camera in the player's children
        if (playerCamera == null)
        {
            Debug.LogError("Player camera not found!");
        }

        // Ensure an AudioSource is attached
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (!IsOwner) return; // Only allow local player to shoot

        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (playerCamera == null) return; // Prevent shooting if camera is missing

        // Play shooting sound
        if (shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }

        // Raycast from the player's camera to determine shooting direction
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100f); // Default to a distant point
        }

        // Calculate shooting direction
        Vector3 shootDirection = (targetPoint - transform.position).normalized;

        // Calculate ammo spawn position
        Vector3 shootPosition = transform.position + shootDirection * shootOffset + new Vector3(0, verticalOffset, 0);

        // Spawn the ammo with a server command
        ShootServerRpc(shootPosition, shootDirection);
    }

    [ServerRpc]
    void ShootServerRpc(Vector3 spawnPosition, Vector3 direction)
    {
        GameObject ammoInstance = Instantiate(ammoPrefab, spawnPosition, Quaternion.identity);
        ammoInstance.GetComponent<NetworkObject>().Spawn(); // Network spawn

        Rigidbody rb = ammoInstance.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(direction * shootForce, ForceMode.Impulse);
        }
    }
}
