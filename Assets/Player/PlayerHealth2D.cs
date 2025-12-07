using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth2D : MonoBehaviour
{
    public int maxHealth = 5;
    private int currentHealth;
    private PlayerMove playerMove;

    void Start()
    {
        float def = CurrencyManagerScript.Instance != null ? CurrencyManagerScript.Instance.Def : 0f;
        int bonusHealth = Mathf.FloorToInt(def / 2f);
        maxHealth += bonusHealth;

        currentHealth = maxHealth;
        playerMove = GetComponent<PlayerMove>();
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
        SceneManager.LoadScene("Death Scene"); 

    }

    // Detect trigger collisions in 2D (ex: enemy hitbox or projectile)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            bool dashAttackUnlocked = CurrencyManagerScript.Instance != null && CurrencyManagerScript.Instance.DashAttackUnlocked;

            if (dashAttackUnlocked && playerMove != null && playerMove.isDashing)
            {
                EnemyHealth2D enemyHealth = collision.GetComponent<EnemyHealth2D>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(1);
                }
                return;
            }

            TakeDamage();
        }
    }
}
