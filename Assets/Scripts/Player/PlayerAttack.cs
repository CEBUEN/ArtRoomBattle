using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerShooting : NetworkBehaviour
{
    [Header("References")]
    public GameObject ammoPrefab; // Ensure this is in NetworkManager Prefabs list
    private Camera playerCamera;

    [Header("Shooting Settings")]
    public float shootForce = 20f;
    public float shootOffset = 1.5f;
    public float verticalOffset = 1.0f;

    [Header("Audio")]
    [SerializeField] private AudioClip shootSound;
    private AudioSource audioSource;

    void Start()
    {
        if (!IsOwner)
        {
            enabled = false;
            return;
        }

        // Get local player camera
        playerCamera = GetComponentInChildren<Camera>();
        if (!playerCamera)
        {
            Debug.LogError("❌ No Camera found on local player for shooting!");
            return;
        }

        // Ensure we have an AudioSource
        audioSource = GetComponent<AudioSource>();
        if (!audioSource)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (!IsOwner) return;

        if (Input.GetMouseButtonDown(0)) // Left-click to shoot
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (!playerCamera) return;

        // Play local shot sound
        if (shootSound && audioSource != null)
        {
            audioSource.PlayOneShot(shootSound);
        }

        // Raycast from the camera to determine shot direction
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out hit, 1000f))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100f);
        }

        Vector3 shootDirection = (targetPoint - transform.position).normalized;

        // Calculate bullet spawn position
        Vector3 spawnPos = transform.position + shootDirection * shootOffset + Vector3.up * verticalOffset;

        // Debug.Log($"🔫 Shooting from {spawnPos} towards {targetPoint}");

        // Tell the server to spawn the projectile
        ShootServerRpc(spawnPos, shootDirection);
    }

    [ServerRpc(RequireOwnership = false)] // Allow non-owners to call this
    private void ShootServerRpc(Vector3 spawnPosition, Vector3 direction)
    {
        if (ammoPrefab == null)
        {
            Debug.LogError("❌ Ammo prefab is missing! Make sure it is assigned in NetworkManager.");
            return;
        }

        GameObject ammoInstance = Instantiate(ammoPrefab, spawnPosition, Quaternion.identity);
        NetworkObject netObj = ammoInstance.GetComponent<NetworkObject>();

        if (netObj != null)
        {
            netObj.Spawn(true); // Spawns across the network
        }
        else
        {
            Debug.LogError("❌ Spawned bullet is missing NetworkObject component!");
        }

        // Apply force to bullet
        Rigidbody rb = ammoInstance.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(direction * shootForce, ForceMode.Impulse);
        }
        else
        {
            Debug.LogError("❌ Ammo prefab is missing a Rigidbody component!");
        }
    }
}
