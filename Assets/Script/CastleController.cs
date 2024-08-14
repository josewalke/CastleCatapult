using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleController : MonoBehaviour
{
    public GameObject street1; // Referencia al objeto de la primera calle
    public GameObject street2; // Referencia al objeto de la segunda calle
    public GameObject street3; // Referencia al objeto de la tercera calle
    private GameObject currentStreet; // Calle actual donde se desplegarán los soldados
    public GameObject soldier; // Prefab del soldado que se va a usar
    public int poolSize = 10; // Tamaño inicial del pool de soldados
    public float speedInvoke; // Tiempo entre despliegues de soldados
    private Queue<GameObject> soldierPool; // Cola que actúa como pool de soldados
    public List<GameObject> soldiersStreet1 = new List<GameObject>(); // Lista de soldados en la primera calle
    public List<GameObject> soldiersStreet2 = new List<GameObject>(); // Lista de soldados en la segunda calle
    public List<GameObject> soldiersStreet3 = new List<GameObject>(); // Lista de soldados en la tercera calle
    public Transform collection; // Transform para el punto de colección de soldados
    public bool castleEnemy; // Booleano para determinar si el castillo es enemigo
    private int num; // Contador para numerar a los soldados

    void Start()
    {
        // Inicializa el pool de soldados y configura la invocación periódica de soldados
        InitializePool();
        InvokeRepeating("InvokeSoldier", 2, speedInvoke);
    }

    void InitializePool()
    {
        // Crea la cola del pool y la llena con soldados inactivos
        soldierPool = new Queue<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            // Instancia un nuevo soldado y lo desactiva
            GameObject newSoldier = Instantiate(soldier, collection);
            newSoldier.SetActive(false);
            // Añade el soldado al pool
            soldierPool.Enqueue(newSoldier);
        }
    }

    void InvokeSoldier()
    {
        // Limpia las listas de soldados inactivos
        CleanEmptySoldiers();

        // Elige aleatoriamente en qué calle desplegar el soldado
        int randomNumber = Random.Range(0, 3);
        switch (randomNumber)
        {
            case 0:
                currentStreet = street1;
                break;
            case 1:
                currentStreet = street2;
                break;
            case 2:
                currentStreet = street3;
                break;
        }

        // Verifica que la calle seleccionada no sea null
        if (currentStreet == null)
        {
            Debug.LogError("La calle seleccionada es null. Asegúrate de que las calles estén correctamente asignadas.");
            return;
        }

        // Obtiene un soldado del pool si hay disponibles, o crea uno nuevo si no
        GameObject pooledSoldier;
        if (soldierPool.Count > 0)
        {
            pooledSoldier = soldierPool.Dequeue(); // Obtiene un soldado del pool
        }
        else
        {
            Debug.LogWarning("No hay soldados disponibles en el pool. Instanciando uno nuevo.");
            pooledSoldier = Instantiate(soldier, collection); // Instancia un nuevo soldado
        }

        // Verifica que el soldado no sea null y resetea su estado
        if (pooledSoldier != null)
        {
            ResetSoldierState(pooledSoldier, randomNumber);
            pooledSoldier.SetActive(true); // Activa el soldado
        }
        else
        {
            Debug.LogError("El soldado instanciado o recuperado del pool es null.");
        }

        // Añade el soldado a la lista correspondiente según la calle
        switch (randomNumber)
        {
            case 0:
                soldiersStreet1.Add(pooledSoldier);
                break;
            case 1:
                soldiersStreet2.Add(pooledSoldier);
                break;
            case 2:
                soldiersStreet3.Add(pooledSoldier);
                break;
        }
    }

    void ResetSoldierState(GameObject soldier, int streetIndex)
    {
        // Verifica si el castillo es enemigo y asigna el controlador adecuado al soldado
        if (castleEnemy)
        {
            // Si es un enemigo, busca el componente EnemySoldierController
            EnemySoldierController enemyController = soldier.GetComponent<EnemySoldierController>();
            if (enemyController == null)
            {
                Debug.LogError("El soldado no tiene un componente EnemySoldierController asignado.");
                return;
            }

            // Resetea la salud del soldado enemigo
            enemyController.currentHealth = enemyController.maxHealth;
            enemyController.SetCastleController(this);

            // Aquí puedes añadir cualquier lógica específica para los enemigos
        }
        else
        {
            // Si es un aliado, busca el componente SoldierController
            SoldierController soldierController = soldier.GetComponent<SoldierController>();
            if (soldierController == null)
            {
                Debug.LogError("El soldado no tiene un componente SoldierController asignado.");
                return;
            }

            // Resetea la salud del soldado aliado
            soldierController.currentHealth = soldierController.maxHealth;
            soldierController.SetCastleController(this);
        }

        // Resetea la posición y rotación del soldado
        soldier.transform.position = currentStreet.transform.position;
        soldier.transform.rotation = castleEnemy ? Quaternion.Euler(0, 90, 0) : Quaternion.Euler(0, -90, 0);

        // Asigna un nombre al soldado basado en si es enemigo o aliado
        soldier.name = (castleEnemy ? "Enemy" : "Soldier") + " numero " + num;
        num++;
    }

    public void CleanEmptySoldiers()
    {
        // Limpia las listas de soldados inactivos
        soldiersStreet1.RemoveAll(soldier => soldier == null);
        soldiersStreet2.RemoveAll(soldier => soldier == null);
        soldiersStreet3.RemoveAll(soldier => soldier == null);

        // Recolecta soldados inactivos y los devuelve al pool
        RecollectSoldiers(soldiersStreet1);
        RecollectSoldiers(soldiersStreet2);
        RecollectSoldiers(soldiersStreet3);
    }

    private void RecollectSoldiers(List<GameObject> soldiersList)
    {
        // Recorre la lista de soldados y devuelve al pool los que están inactivos
        for (int i = soldiersList.Count - 1; i >= 0; i--)
        {
            if (!soldiersList[i].activeInHierarchy)
            {
                soldiersList[i].SetActive(false);
                soldierPool.Enqueue(soldiersList[i]);
                soldiersList.RemoveAt(i);
            }
        }
    }

    // Método para devolver un soldado al pool
    public void ReturnSoldierToPool(GameObject soldier)
    {
        soldier.SetActive(false); // Desactiva el soldado
        soldierPool.Enqueue(soldier); // Lo añade al pool para su reutilización

        // Remueve el soldado de las listas de calles
        soldiersStreet1.Remove(soldier);
        soldiersStreet2.Remove(soldier);
        soldiersStreet3.Remove(soldier);
    }
}
