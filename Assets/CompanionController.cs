using UnityEngine;

public class CompanionController : MonoBehaviour
{
    public Transform player;
    public GameObject slowProjectilePrefab;
    public float orbitRadius = 0.7f;
    public float orbitSpeed = 90f;
    public float fireInterval = 1.5f;
    public float targetRange = 6f;

    float angle;
    float fireTimer;

    void Update()
    {
        if (player == null) return;

        angle += orbitSpeed * Mathf.Deg2Rad * Time.deltaTime;
        Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * orbitRadius;
        transform.position = player.position + offset;

        fireTimer -= Time.deltaTime;
        if (fireTimer <= 0f)
        {
            TryFireAtEnemy();
            fireTimer = fireInterval;
        }
    }

    void TryFireAtEnemy()
    {
        if (slowProjectilePrefab == null) return;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0) return;

        GameObject closest = null;
        float bestDist = Mathf.Infinity;

        foreach (var e in enemies)
        {
            float d = Vector2.Distance(transform.position, e.transform.position);
            if (d < bestDist && d <= targetRange)
            {
                bestDist = d;
                closest = e;
            }
        }

        if (closest == null) return;

        Vector2 dir = (closest.transform.position - transform.position).normalized;
        GameObject proj = Instantiate(slowProjectilePrefab, transform.position, Quaternion.identity);
        var sp = proj.GetComponent<SlowProjectile>();
        if (sp != null)
            sp.SetDirection(dir);
    }
}
