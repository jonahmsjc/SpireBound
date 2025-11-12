using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth2D : MonoBehaviour
{
    public static bool playerIsDead = false;   

    public int maxHealth = 5;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        playerIsDead = false;                  // reset when scene reloads
    }

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
        playerIsDead = true;                   // <-- SET DEAD FLAG
        SceneManager.LoadScene("Death Scene");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            TakeDamage();
        }
    }
}
