using UnityEngine;

public class EnemyMoveToPlayer : MonoBehaviour
{
    public float speed = 5f;
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
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    void Update()
    {
        if (player != null)
        {
            Vector2 direction = ((Vector2)player.position - (Vector2)transform.position).normalized;
            transform.position += (Vector3)direction * speed * Time.deltaTime;
        }

        // Off-screen check
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
