using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int poolSize = 10;

    private Queue<GameObject> pool = new Queue<GameObject>();

    private void Start()
    {
        // Inicializa el pool de enemigos
        for (int i = 0; i < poolSize; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.SetActive(false);
            pool.Enqueue(enemy);
        }
    }

    public GameObject GetEnemyFromPool()
    {
        if (pool.Count > 0)
        {
            GameObject enemy = pool.Dequeue();
            enemy.SetActive(true);
            return enemy;
        }
        else
        {
            // Opcional: Expande el pool si es necesario
            GameObject enemy = Instantiate(enemyPrefab);
            return enemy;
        }
    }

    public void ReturnEnemyToPool(EnemyController enemy)
    {
        enemy.gameObject.SetActive(false);
        pool.Enqueue(enemy.gameObject);
    }
}
