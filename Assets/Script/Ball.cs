using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float lifetime = 5.0f; // Tiempo de vida de la bola antes de ser devuelta al pool
    public string targetTag = "EnemySoldier"; // Tag para identificar el objetivo
    private Action<GameObject> returnToPoolAction; // Acción a ejecutar al devolver la bola al pool
    private Rigidbody rb; // Referencia al componente Rigidbody de la bola

    void Awake()
    {
        // Obtiene la referencia al componente Rigidbody al iniciar el script
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        // Inicia la desactivación de la bola después de 'lifetime' segundos
        // Esto asegura que la bola sea devuelta al pool después del tiempo especificado
        Invoke("ReturnToPool", lifetime);
    }

    void OnDisable()
    {
        // Cancela cualquier invocación pendiente de ReturnToPool cuando la bola se desactive
        // Esto evita que la bola sea devuelta al pool si se desactiva antes de que el tiempo de vida termine
        CancelInvoke();
    }

    public void SetReturnToPoolAction(Action<GameObject> action)
    {
        // Asigna la acción que se debe ejecutar al devolver la bola al pool
        returnToPoolAction = action;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Cuando la bola colisiona con otro objeto
        // Si se desea devolver la bola al pool solo al colisionar con objetos específicos, 
        // se puede descomentar la siguiente línea y ajustar el tag.
        // if (collision.collider.CompareTag(targetTag))
        {
            // Llama al método para devolver la bola al pool
            ReturnToPool();
        }
    }

    void ReturnToPool()
    {
        // Ejecuta la acción de devolver la bola al pool si está asignada
        returnToPoolAction?.Invoke(gameObject);
    }

    public void AddForce(Vector3 force)
    {
        // Aplica una fuerza a la bola usando el modo de impulso
        rb.AddForce(force, ForceMode.Impulse);
    }
}
