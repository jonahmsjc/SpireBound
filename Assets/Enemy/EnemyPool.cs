using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int poolSize = 20;

    private Queue<GameObject> pool = new Queue<GameObject>();

    void Start()
    {
        // Pre-instantiate pool objects
        for (int i = 0; i < poolSize; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.SetActive(false);
            pool.Enqueue(enemy);
        }
    }

    public GameObject GetEnemyFromPool(Vector2 spawnPosition)
    {
        GameObject enemy;

    if (pool.Count > 0)
        enemy = pool.Dequeue();
    else
        enemy = Instantiate(enemyPrefab);

    // Set position BEFORE enabling the object
    enemy.transform.position = spawnPosition;

    //  Enable the object AFTER position is set
    enemy.SetActive(true);

    return enemy;
    }

    public void ReturnEnemyToPool(GameObject enemy)
    {
        enemy.SetActive(false);
        pool.Enqueue(enemy);
    }
}