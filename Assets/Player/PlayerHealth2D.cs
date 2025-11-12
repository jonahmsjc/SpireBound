using UnityEngine;

public class PlayerHealth2D : MonoBehaviour
{
    public int maxHealth = 5;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    // Call this function whenever the player gets hit
    public void TakeDamage(int damageAmount = 1)
    {
        currentHealth -= damageAmount;
        Debug.Log("Player took damage! Current Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player Died!");
        // Add respawn / game over logic here
        // Destroy(gameObject);
    }

    // Detect trigger collisions in 2D (ex: enemy hitbox or projectile)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            TakeDamage();
        }
    }
}
