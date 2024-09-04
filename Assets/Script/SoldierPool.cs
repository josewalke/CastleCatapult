using System.Collections.Generic;
using UnityEngine;

public class SoldierPool : MonoBehaviour
{
    [SerializeField] private GameObject soldierPrefab;
    [SerializeField] private int poolSize = 10;

    private Queue<GameObject> pool = new Queue<GameObject>();

    private void Start()
    {
        // Inicializa el pool de soldados
        for (int i = 0; i < poolSize; i++)
        {
            GameObject soldier = Instantiate(soldierPrefab);
            soldier.SetActive(false);
            pool.Enqueue(soldier);
        }
    }

    public GameObject GetSoldierFromPool()
    {
        if (pool.Count > 0)
        {
            GameObject soldier = pool.Dequeue();
            soldier.SetActive(true);
            return soldier;
        }
        else
        {
            // Opcional: Expande el pool si es necesario
            GameObject soldier = Instantiate(soldierPrefab);
            return soldier;
        }
    }

    public void ReturnSoldierToPool(SoldierController soldier)
    {
        soldier.gameObject.SetActive(false);
        pool.Enqueue(soldier.gameObject);
    }
}
