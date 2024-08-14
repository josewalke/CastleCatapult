using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonController : MonoBehaviour
{
    public GameObject target; // El objetivo actual al que el cañón está apuntando
    public PoolingManager ballPool; // Referencia al PoolingManager para obtener bolas del pool
    public CastleController castleController; // Referencia al CastleController para acceder a los soldados
    public Transform SpawnBall; // Punto desde el cual se dispararán las bolas
    public float rotationSpeed = 1.0f; // Velocidad de rotación del cañón
    public float rayLength = 10.0f; // Longitud del raycast para detectar objetivos
    public string targetTag = "EnemySoldier"; // Tag que identifica a los objetivos válidos
    public float cooldownTime = 1.0f; // Tiempo de enfriamiento entre disparos
    private float cooldownTimer = 0f; // Temporizador para el tiempo de enfriamiento
    public float baseShootForce = 10f; // Fuerza base del disparo
    public float forceMultiplier = 1.0f; // Multiplicador de fuerza basado en la distancia

    private int currentStreetIndex = 0; // Índice de la calle actual para cambiar de objetivo

    // Update se llama una vez por frame
    void Update()
    {
        // Cambia de calle cuando se presiona la tecla C
        if (Input.GetKeyDown(KeyCode.C))
        {
            ChangeTargetStreet();
        }

        // Actualiza el temporizador de enfriamiento
        cooldownTimer -= Time.deltaTime;

        if (target != null)
        {
            // Calcula la distancia entre el cañón y el objetivo
            float distance = Vector3.Distance(transform.position, target.transform.position);
            rayLength = distance; // Ajusta la longitud del raycast según la distancia

            // Crea un raycast desde la posición del cañón hacia adelante
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, rayLength))
            {
                // Verifica si el raycast golpea un objeto con el tag correcto
                if (hit.collider.CompareTag(targetTag))
                {
                    if (cooldownTimer <= 0f)
                    {
                        // Dispara una bola si el temporizador de enfriamiento ha expirado
                        ShootBall(distance);
                        // Reinicia el temporizador de enfriamiento
                        cooldownTimer = cooldownTime;
                    }
                }
            }

            // Calcula la dirección hacia el objetivo
            Vector3 direction = target.transform.position - transform.position;

            // Calcula la rotación deseada para apuntar al objetivo
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Interpola suavemente la rotación actual hacia la rotación deseada
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            // Cambia de calle si no hay objetivo
            ChangeTargetStreet();
        }
    }

    void OnDrawGizmos()
    {
        // Dibuja una línea verde en la escena para visualizar el raycast
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * rayLength);
    }

    void ShootBall(float distance)
    {
        // Obtiene una bola del pool y configura su posición y rotación
        GameObject ball = ballPool.GetObject();
        ball.transform.position = SpawnBall.position;
        ball.transform.rotation = Quaternion.identity;

        // Calcula la dirección del disparo hacia el objetivo
        Vector3 shootDirection = (target.transform.position - SpawnBall.position).normalized;

        // Calcula la fuerza del disparo basada en la distancia
        float shootForce = baseShootForce + distance * forceMultiplier;

        // Añade un impulso a la bola
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero; // Reinicia la velocidad antes de aplicar la nueva fuerza
            rb.AddForce(shootDirection * shootForce, ForceMode.Impulse);
        }
    }

    void ChangeTargetStreet()
    {
        // Incrementa el índice de la calle y envuelve entre 0 y 2
        currentStreetIndex = (currentStreetIndex + 1) % 3;
        castleController.CleanEmptySoldiers(); // Limpia los soldados inactivos

        // Cambia el objetivo basado en el índice de la calle
        switch (currentStreetIndex)
        {
            case 0:
                if (castleController.soldiersStreet1.Count > 0 && castleController.soldiersStreet1[0] != null)
                {
                    target = castleController.soldiersStreet1[0];
                }
                else
                {
                    FindEnemyStreet();
                }
                break;
            case 1:
                if (castleController.soldiersStreet2.Count > 0 && castleController.soldiersStreet2[0] != null)
                {
                    target = castleController.soldiersStreet2[0];
                }
                else
                {
                    FindEnemyStreet();
                }
                break;
            case 2:
                if (castleController.soldiersStreet3.Count > 0 && castleController.soldiersStreet3[0] != null)
                {
                    target = castleController.soldiersStreet3[0];
                }
                else
                {
                    FindEnemyStreet();
                }
                break;
        }
    }

    void FindEnemyStreet()
    {
        // Limpia los soldados inactivos
        castleController.CleanEmptySoldiers();

        // Intenta encontrar un objetivo en las tres calles
        if (castleController.soldiersStreet1.Count > 0 && castleController.soldiersStreet1[0] != null)
        {
            target = castleController.soldiersStreet1[0];
            return;
        }
        if (castleController.soldiersStreet2.Count > 0 && castleController.soldiersStreet2[0] != null)
        {
            target = castleController.soldiersStreet2[0];
            return;
        }
        if (castleController.soldiersStreet3.Count > 0 && castleController.soldiersStreet3[0] != null)
        {
            target = castleController.soldiersStreet3[0];
            return;
        }

        // Si no hay soldados en ninguna calle, detiene el raycast
        rayLength = 0;

        // Calcula la rotación de reseteo para el cañón
        Quaternion resetRotation = Quaternion.Euler(0, -90, 0);

        // Interpola la rotación actual hacia la rotación de reseteo
        transform.rotation = Quaternion.Slerp(transform.rotation, resetRotation, rotationSpeed * Time.deltaTime);
    }
}
