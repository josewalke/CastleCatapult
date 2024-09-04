using UnityEngine;

public class SoldierManager : MonoBehaviour
{
    [SerializeField] private SoldierPool soldierPool;
    [SerializeField] private Transform[] spawnPoints; // Puntos de spawn para los soldados
    [SerializeField] private float spawnInterval = 5f; // Intervalo de tiempo entre los spawns

    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnSoldier();
            timer = 0f;
        }
    }

    private void SpawnSoldier()
    {
        // Obtén un soldado del pool
        GameObject soldierObject = soldierPool.GetSoldierFromPool();
        SoldierController soldier = soldierObject.GetComponent<SoldierController>();

        // Establece la posición y rotación del soldado en un punto de spawn aleatorio
        if (spawnPoints.Length > 0)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            soldierObject.transform.position = spawnPoint.position;
            soldierObject.transform.rotation = spawnPoint.rotation;

            // Establece el soldado como hijo del objeto de calle en el punto de spawn
            soldierObject.transform.SetParent(spawnPoint);
        }
    }
}
