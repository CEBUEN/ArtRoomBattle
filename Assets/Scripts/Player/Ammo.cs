using UnityEngine;

public class Ammo : MonoBehaviour
{
    public float lifespan = 5f; // Time before the ammo is destroyed
    public int damage = 10;     // Damage dealt by the ammo

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("No Rigidbody found on ammo prefab!");
            Destroy(gameObject);
            return;
        }

        // Make the ammo extremely light so it imparts virtually no force
        rb.mass = 0.001f;
    }

    void Update()
    {
        // Debug check: Log velocity to confirm ammo is moving
        // (commented out for cleanliness)
        // if (rb != null && rb.velocity.magnitude > 0.1f)
        // {
        //     Debug.Log($"Ammo is flying! Velocity: {rb.velocity}");
        // }

        // Disable gravity so it won't drop and lose momentum
        rb.useGravity = false;

        // Set collision detection to continuous to prevent passing through objects
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        // Destroy the ammo after its lifespan
        Destroy(gameObject, lifespan);
    }

    void OnCollisionEnter(Collision collision)
    {
        // 1) If we hit an Enemy, deal damage to the Enemy
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyAI enemyAI = collision.gameObject.GetComponent<EnemyAI>();
            if (enemyAI != null)
            {
                enemyAI.TakeDamage(damage);
            }
        }
        // 2) If we hit a Player, deal 20 damage to the Player
        else if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(20);
            }
        }

        // Destroy the ammo upon collision
        Destroy(gameObject);
    }
}
