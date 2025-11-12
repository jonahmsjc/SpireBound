using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    [SerializeField] private int damage = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyHealth2D>()?.TakeDamage(damage);
        }

        // Option 2 (alternative): detect any object that has EnemyHealth2D (no tag needed)
        // var enemy = other.GetComponent<EnemyHealth2D>();
        // if (enemy != null)
        // {
        //     enemy.TakeDamage(damage);
        // }
    }
}
