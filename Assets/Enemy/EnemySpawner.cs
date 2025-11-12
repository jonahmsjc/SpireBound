using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float spawnInterval = 1.5f;
    private EnemyPool pool;

    void Start()
    {
        pool = FindObjectOfType<EnemyPool>();
        InvokeRepeating(nameof(SpawnEnemy), spawnInterval, spawnInterval);
    }

    void SpawnEnemy()
    {

        Camera cam = Camera.main;
        Vector2 spawnPos = GetRandomScreenEdgePos(cam);
        pool.GetEnemyFromPool(spawnPos);
    }

    Vector2 GetRandomScreenEdgePos(Camera cam)
    {
        Vector2 screenPos = Vector2.zero;

        int side = Random.Range(0, 4); // 0=top,1=bottom,2=left,3=right

        switch (side)
        {
            case 0: screenPos = new Vector2(Random.value, 1); break;
            case 1: screenPos = new Vector2(Random.value, 0); break;
            case 2: screenPos = new Vector2(0, Random.value); break;
            case 3: screenPos = new Vector2(1, Random.value); break;
        }

        return cam.ViewportToWorldPoint(screenPos);
    }
}
