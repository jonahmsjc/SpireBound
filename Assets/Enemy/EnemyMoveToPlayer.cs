using UnityEngine;

public class EnemyMoveToPlayer : MonoBehaviour
{
    public float speed = 5f;
    private Vector2 targetDirection;
    private Camera mainCamera;
    private EnemyPool pool;

    void Awake()
    {
        mainCamera = Camera.main;
        pool = FindObjectOfType<EnemyPool>();  // Cache pool reference
    }

    void OnEnable()   // Called EVERY time the enemy is reused
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            targetDirection = ((Vector2)player.transform.position - (Vector2)transform.position).normalized;
        }
        else
        {
            targetDirection = Vector2.zero;
        }
    }

    void Update()
    {
        transform.position += (Vector3)targetDirection * speed * Time.deltaTime;

        // Destroy when off screen
        Vector3 screenPoint = mainCamera.WorldToViewportPoint(transform.position);
        if (screenPoint.x < -0.1f || screenPoint.x > 1.1f || screenPoint.y < -0.1f || screenPoint.y > 1.1f)
        {
            pool.ReturnEnemyToPool(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            pool.ReturnEnemyToPool(gameObject);
        }
    }
}
