using UnityEngine;

public class SlowProjectile : MonoBehaviour
{
    public float speed = 6f;
    public float slowMultiplier = 0.1f;
    public float slowDuration = 5f;
    public float lifeTime = 3f;

    Vector2 direction;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"[PROJECTILE HIT] Hit {other.name} with tag {other.tag}");

        if (other.CompareTag("Enemy"))
        {
            var move = other.GetComponentInParent<EnemyMoveToPlayer>();
            if (move != null)
            {
                Debug.Log($"[PROJECTILE] Applying slow to {move.name}");
                move.ApplySlow(slowMultiplier, slowDuration);
            }
            else
            {
                Debug.Log("[PROJECTILE] No EnemyMoveToPlayer found on hit object or parents");
            }

            Destroy(gameObject);
        }
    }

}

