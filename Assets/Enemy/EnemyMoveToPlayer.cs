using UnityEngine;

public class EnemyMoveToPlayer : MonoBehaviour
{
    public float speed = 5f;
    private float speedMultiplier = 1f;
    private float slowTimer;

    private Camera mainCamera;
    private EnemyPool pool;
    private Transform player;

    void Awake()
    {
        mainCamera = Camera.main;
        pool = FindObjectOfType<EnemyPool>();
    }

    void OnEnable()
    {
        speedMultiplier = 1f;
        slowTimer = 0f;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    void Update()
    {
        if (slowTimer > 0f)
        {
            slowTimer -= Time.deltaTime;
            if (slowTimer <= 0f)
            {
                speedMultiplier = 1f;
                Debug.Log($"[SLOW END] {name} back to normal speed");
            }
        }

        if (player != null)
        {
            Vector2 direction = ((Vector2)player.position - (Vector2)transform.position).normalized;
            float moveSpeed = speed * speedMultiplier;
            transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
        }

        // Off-screen check
        Vector3 screenPoint = mainCamera.WorldToViewportPoint(transform.position);
        if (screenPoint.x < -0.1f || screenPoint.x > 1.1f || screenPoint.y < -0.1f || screenPoint.y > 1.1f)
        {
            pool.ReturnEnemyToPool(gameObject);
        }
    }

    public void ApplySlow(float multiplier, float duration)
    {
        multiplier = Mathf.Clamp01(multiplier);

        speedMultiplier = multiplier;
        slowTimer = duration;

        Debug.Log($"[SLOW APPLY] {name} slowed to {speedMultiplier} for {slowTimer} seconds");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            pool.ReturnEnemyToPool(gameObject);
        }
    }
}
