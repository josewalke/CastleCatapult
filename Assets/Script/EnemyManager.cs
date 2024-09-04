using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private EnemyPool enemyPool;
    [SerializeField] private Transform[] spawnPoints; // Puntos de spawn para los enemigos
    [SerializeField] private float spawnInterval = 5f; // Intervalo de tiempo entre los spawns

    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    private void SpawnEnemy()
    {
        // Obtén un enemigo del pool
        GameObject enemyObject = enemyPool.GetEnemyFromPool();
        EnemyController enemy = enemyObject.GetComponent<EnemyController>();

        // Establece la posición y rotación del enemigo en un punto de spawn aleatorio
        if (spawnPoints.Length > 0)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            enemyObject.transform.position = spawnPoint.position;
            enemyObject.transform.rotation = spawnPoint.rotation;

            // Establece el enemigo como hijo del objeto de calle en el punto de spawn
            enemyObject.transform.SetParent(spawnPoint);
        }
    }
}
