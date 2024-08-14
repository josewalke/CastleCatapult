using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    public GameObject ball; // Prefab del objeto que se va a usar en el pool
    public int poolSize = 10; // Tamaño inicial del pool

    public Queue<GameObject> pool = new Queue<GameObject>(); // Cola para almacenar objetos inactivos

    void Start()
    {
        // Inicializa el pool con objetos desactivados
        for (int i = 0; i < poolSize; i++)
        {
            // Instancia un nuevo objeto del prefab 'ball'
            GameObject obj = Instantiate(ball);
            // Desactiva el objeto para que no esté visible ni interactúe hasta que sea reutilizado
            obj.SetActive(false);
            // Añade el objeto al pool
            pool.Enqueue(obj);
        }
    }

    public GameObject GetObject()
    {
        GameObject obj;
        if (pool.Count > 0)
        {
            // Si hay objetos en el pool, obtén el siguiente objeto disponible
            obj = pool.Dequeue();
        }
        else
        {
            // Si el pool está vacío, instancia un nuevo objeto
            obj = Instantiate(ball);
        }

        // Activa el objeto para que pueda ser utilizado
        obj.SetActive(true);
        // Asigna la acción de devolución al pool al componente Ball del objeto
        obj.GetComponent<Ball>().SetReturnToPoolAction(ReturnObject);
        return obj;
    }

    public void ReturnObject(GameObject obj)
    {
        // Desactiva el objeto antes de devolverlo al pool
        obj.SetActive(false);
        // Añade el objeto de vuelta al pool
        pool.Enqueue(obj);
    }
}
