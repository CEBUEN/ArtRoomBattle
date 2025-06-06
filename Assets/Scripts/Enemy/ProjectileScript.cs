using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private float lifeTime = 5f; // Time before the projectile is destroyed
    [SerializeField] private float initialSpeed = 10f; // Initial speed of the projectile
    [SerializeField] private float slowDownRate = 0f; // Rate at which the projectile slows down per second
    [SerializeField] private int damage = 10; // Damage the projectile deals to the player

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Set Rigidbody settings for fast-moving objects
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            rb.interpolation = RigidbodyInterpolation.Interpolate;

            // Set the initial velocity of the projectile
            rb.linearVelocity = transform.forward * initialSpeed;
        }

        // Schedule destruction after the lifetime expires
        Destroy(gameObject, lifeTime);
    }

    private void FixedUpdate()
    {
        if (rb != null && slowDownRate > 0f)
        {
            // Reduce the projectile's velocity over time
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, Vector3.zero, slowDownRate * Time.fixedDeltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the projectile hit the player
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                // Apply damage to the player
                playerHealth.TakeDamage(damage);
                Debug.Log($"Projectile hit {collision.gameObject.name}, dealing {damage} damage.");
            }
        }

        // Destroy the projectile on collision
        Destroy(gameObject);
    }
}
