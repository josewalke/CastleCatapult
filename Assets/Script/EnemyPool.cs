using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    // Prefab del enemigo que será instanciado cuando el pool esté vacío
    [SerializeField] private EnemyController enemyPrefab;

    // Cola para almacenar los enemigos inactivos que pueden ser reutilizados
    private Queue<EnemyController> enemyPool = new Queue<EnemyController>();

    // Contador para asignar nombres únicos a los enemigos
    private int num;

    // Método para obtener un enemigo del pool
    public EnemyController GetEnemy()
    {
        // Si hay enemigos disponibles en el pool, los reutiliza
        if (enemyPool.Count > 0)
        {
            // Saca el enemigo de la cola (desactivado) y lo reactiva
            EnemyController enemy = enemyPool.Dequeue();
            enemy.gameObject.SetActive(true);  // Reactiva el enemigo
            return enemy;  // Devuelve el enemigo reutilizado
        }
        else
        {
            // Si no hay enemigos en el pool, crea uno nuevo
            num++;  // Incrementa el contador para asignar un nombre único
            EnemyController newEnemy = Instantiate(enemyPrefab);  // Crea una nueva instancia del prefab
            newEnemy.Initialize(this);  // Inicializa el enemigo con referencia a este pool
            newEnemy.name = "Enemy " + num;  // Asigna un nombre único al enemigo
            return newEnemy;  // Devuelve el nuevo enemigo creado
        }
    }

    // Método para devolver un enemigo al pool
    public void ReturnToPool(EnemyController enemy)
    {
        // Desactiva el enemigo y lo devuelve al pool
        enemy.gameObject.SetActive(false);  // Desactiva el objeto del enemigo para evitar que siga en la escena
        enemyPool.Enqueue(enemy);           // Añade el enemigo a la cola del pool para su reutilización futura
    }
}
