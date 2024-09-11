using UnityEngine;

public class SoldierManager : MonoBehaviour
{
    [SerializeField] private SoldierPool soldierPool; // Referencia al pool de soldados
    [SerializeField] private Transform[] spawnPoints; // Array de puntos de spawn donde se generarán los soldados
    [SerializeField] private float spawnInterval = 5f; // Intervalo de tiempo entre las apariciones de soldados

    private float timer; // Temporizador que controlará cuándo generar el siguiente soldado

    // Método que se llama en cada frame
    private void Update()
    {
        // Incrementa el temporizador con el tiempo transcurrido entre frames
        timer += Time.deltaTime;

        // Si el tiempo acumulado es mayor o igual al intervalo de spawn, genera un nuevo soldado
        if (timer >= spawnInterval)
        {
            SpawnSoldier(); // Llama al método para generar un soldado
            timer = 0f;     // Reinicia el temporizador para esperar el siguiente spawn
        }
    }

    // Método que se encarga de generar soldados
    private void SpawnSoldier()
    {
        // Obtiene un soldado del pool usando el método GetSoldier
        SoldierController soldier = soldierPool.GetSoldier();

        // Si hay puntos de spawn definidos
        if (spawnPoints.Length > 0)
        {
            // Selecciona un punto de spawn aleatorio del array de spawnPoints
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            // Posiciona el soldado en el punto de spawn aleatorio y ajusta su rotación
            soldier.transform.position = spawnPoint.position;
            soldier.transform.rotation = spawnPoint.rotation;

            // Inicializa el soldado con una referencia al pool, para que pueda volver al pool después de morir
            soldier.Initialize(soldierPool);

            // Opcional: Establece el soldado como hijo del objeto de spawn para mantener la jerarquía organizada
            soldier.transform.SetParent(spawnPoint);
        }
    }
}
