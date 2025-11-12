using UnityEngine;

public class EnemyHealth2D : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage!");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} died!");
        //TODO : change the next 2 lines to use a settable local variable.
        CurrencyManagerScript.Instance.Gold += 10;
        CurrencyManagerScript.Instance.Exp += 10;
        Destroy(gameObject);
    }
}
