using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    // Referencia al pool de enemigos que administra la reutilización de objetos
    [SerializeField] private EnemyPool enemyPool;

    // Arreglo de puntos de spawn, donde los enemigos serán instanciados
    [SerializeField] private Transform[] spawnPoints;

    // Intervalo de tiempo entre spawns de enemigos
    [SerializeField] private float spawnInterval = 5f;

    // Temporizador para contar el tiempo entre spawns
    private float timer;

    private void Update()
    {
        // Incrementa el temporizador por cada cuadro, usando el tiempo transcurrido
        timer += Time.deltaTime;

        // Si el temporizador excede el intervalo de spawn
        if (timer >= spawnInterval)
        {
            SpawnEnemy();  // Genera un enemigo
            timer = 0f;    // Reinicia el temporizador
        }
    }

    // Método que maneja el spawn de enemigos
    private void SpawnEnemy()
    {
        // Obtén un enemigo del pool; si no hay enemigos disponibles, se creará uno nuevo
        EnemyController enemy = enemyPool.GetEnemy();

        // Si hay puntos de spawn definidos
        if (spawnPoints.Length > 0)
        {
            // Selecciona un punto de spawn aleatorio
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            // Posiciona y rota al enemigo en el punto de spawn
            enemy.transform.position = spawnPoint.position;
            enemy.transform.rotation = spawnPoint.rotation;

            // Inicializa el enemigo con la referencia del pool
            enemy.Initialize(enemyPool);

            // Asocia el enemigo como hijo del objeto en el punto de spawn para mantener jerarquía organizada
            enemy.transform.SetParent(spawnPoint);
        }
    }
}
