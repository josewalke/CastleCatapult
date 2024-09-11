using System.Collections.Generic;
using UnityEngine;

public class SoldierPool : MonoBehaviour
{
    // Prefab del soldado que se instanciará si el pool está vacío
    [SerializeField] private SoldierController soldierPrefab;

    // Cola que almacena los soldados disponibles en el pool
    private Queue<SoldierController> soldierPool = new Queue<SoldierController>();

    // Contador para numerar los soldados instanciados
    private int num;

    // Método para obtener un soldado del pool
    public SoldierController GetSoldier()
    {
        // Si el pool tiene soldados disponibles, los sacamos de la cola
        if (soldierPool.Count > 0)
        {
            SoldierController soldier = soldierPool.Dequeue();  // Obtiene un soldado de la cola
            soldier.gameObject.SetActive(true);                // Activa el objeto del soldado
            return soldier;                                    // Devuelve el soldado listo para usarse
        }
        else
        {
            // Si el pool está vacío, instanciamos un nuevo soldado
            num++;
            SoldierController newSoldier = Instantiate(soldierPrefab);  // Instancia un nuevo soldado a partir del prefab
            newSoldier.Initialize(this);                                // Asocia el pool al nuevo soldado
            newSoldier.name = "Soldier " + num;                         // Asigna un nombre único al soldado
            return newSoldier;                                          // Devuelve el nuevo soldado instanciado
        }
    }

    // Método para devolver un soldado al pool
    public void ReturnToPool(SoldierController soldier)
    {
        soldier.gameObject.SetActive(false);  // Desactiva el soldado (ya no está en uso)
        soldierPool.Enqueue(soldier);         // Añade el soldado a la cola del pool, para reutilizarlo en el futuro
    }
}
