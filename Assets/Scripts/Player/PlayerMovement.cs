using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : NetworkBehaviour
{
    [Header("Movement Settings")]
    public float speed = 6f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;
    public float mouseSensitivity = 100f;

    [Header("References")]
    public CharacterController controller;
    public Transform playerCamera;

    private Vector3 velocity;
    private bool isGrounded;
    private float xRotation = 0f;

    private bool isCursorLocked = true;

    private void Awake()
    {
        if (controller == null)
        {
            controller = GetComponent<CharacterController>();
        }
    }

    public override void OnNetworkSpawn()
{
    if (GetComponent<NetworkObject>() == null)
    {
        Debug.LogError("❌ NetworkObject is missing on this Player prefab!");
        return;
    }

    if (IsOwner)
    {
        Debug.Log($"✅ Client {NetworkManager.Singleton.LocalClientId} owns this player!");
    }
    else
    {
        Debug.LogWarning($"❌ Client {NetworkManager.Singleton.LocalClientId} does NOT own this player! Requesting ownership...");
        RequestOwnershipServerRpc(NetworkManager.Singleton.LocalClientId);
    }
}


    private void DelayedRequestOwnership()
    {
        if (!IsOwner)
        {
            RequestOwnershipServerRpc(NetworkManager.Singleton.LocalClientId);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void RequestOwnershipServerRpc(ulong clientId)
    {
        NetworkObject netObj = GetComponent<NetworkObject>();

        if (netObj == null)
        {
            Debug.LogError("❌ NetworkObject is missing! Cannot assign ownership.");
            return;
        }

        Debug.Log($"👑 Assigning ownership of this player to Client {clientId}");
        netObj.ChangeOwnership(clientId);
    }

    void Start()
    {
        if (!IsOwner)
        {
            if (playerCamera != null)
            {
                playerCamera.gameObject.SetActive(false);
            }
            enabled = false;
            return;
        }

        if (playerCamera == null)
        {
            Camera camInChildren = GetComponentInChildren<Camera>();
            if (camInChildren != null)
            {
                playerCamera = camInChildren.transform;
            }
        }

        if (playerCamera == null)
        {
            Debug.LogError("No playerCamera assigned or found in children!");
        }

        LockCursor();
    }

    void Update()
    {
        if (!IsOwner) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleCursorLock();
        }

        if (Cursor.lockState == CursorLockMode.None) return;

        HandleMouseLook();
        HandleMovement();
    }

    private void ToggleCursorLock()
    {
        if (isCursorLocked)
        {
            UnlockCursor();
        }
        else
        {
            LockCursor();
        }
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isCursorLocked = true;
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isCursorLocked = false;
    }

    private void HandleMouseLook()
    {
        if (playerCamera == null) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void HandleMovement()
    {
        if (controller == null) return;

        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
