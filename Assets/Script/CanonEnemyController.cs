using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonEnemyController : MonoBehaviour
{
    // Variables públicas configurables desde el editor de Unity
    public GameObject target;                // Objetivo actual del cañón
    public PoolingManager ballPool;          // Referencia al gestor de pooling para las balas
    public CastleController castleController; // Referencia al controlador del castillo
    public Transform SpawnBall;              // Punto de spawn de las balas
    public float rotationSpeed = 1.0f;       // Velocidad de rotación del cañón hacia el objetivo
    public float rayLength = 10.0f;          // Longitud del raycast para detectar objetivos
    public string targetTag = "EnemySoldier"; // Tag para identificar los objetivos
    public float cooldownTime = 1.0f;        // Tiempo de enfriamiento entre disparos
    private float cooldownTimer = 0f;        // Temporizador para el enfriamiento
    public float baseShootForce = 10f;       // Fuerza base del disparo
    public float forceMultiplier = 1.0f;     // Multiplicador para ajustar la fuerza en función de la distancia

    private int currentStreetIndex = 0;      // Índice de la calle actual, usado para control (aun no implementado)

    void Start()
    {
    }

    // Método Update se llama una vez por frame
    void Update()
    {
        // Decrementa el temporizador de enfriamiento cada frame
        cooldownTimer -= Time.deltaTime;

        // Encuentra el objetivo más cercano basado en su tag
        FindClosestTarget();

        // Si hay un objetivo seleccionado
        if (target != null)
        {
            // Calcula la distancia al objetivo
            float distance = Vector3.Distance(transform.position, target.transform.position);
            rayLength = distance;

            // Realiza un raycast hacia adelante desde la posición actual
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, rayLength))
            {
                // Verifica si el raycast golpeó un objeto con el tag correcto
                if (hit.collider.CompareTag(targetTag))
                {
                    // Dispara si el tiempo de enfriamiento ha pasado
                    if (cooldownTimer <= 0f)
                    {
                        ShootBall(distance); // Dispara una bala hacia el objetivo
                        cooldownTimer = cooldownTime; // Reinicia el temporizador de enfriamiento
                    }
                }
            }

            // Calcula la dirección hacia el objetivo
            Vector3 direction = target.transform.position - transform.position;

            // Calcula la rotación necesaria para apuntar al objetivo
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Interpola la rotación actual hacia la rotación objetivo
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            // Si no hay objetivo, resetea la longitud del raycast
            rayLength = 0;

            // Define la rotación deseada (reseteada a una posición inicial)
            Quaternion resetRotation = Quaternion.Euler(0, 90, 0);

            // Interpola la rotación actual hacia la rotación de reset
            transform.rotation = Quaternion.Slerp(transform.rotation, resetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    // Método OnDrawGizmos para visualizar el raycast en la escena de Unity
    void OnDrawGizmos()
    {
        // Dibuja una línea roja que representa el raycast
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * rayLength);
    }

    // Método para disparar una bala
    void ShootBall(float distance)
    {
        // Obtiene una bala del pool y la posiciona en el punto de spawn
        GameObject ball = ballPool.GetObject();
        ball.transform.position = SpawnBall.position;
        ball.transform.rotation = Quaternion.identity;

        // Calcula la dirección de disparo hacia el objetivo
        Vector3 shootDirection = (target.transform.position - SpawnBall.position).normalized;

        // Calcula la fuerza de disparo basada en la distancia al objetivo
        float shootForce = baseShootForce + distance * forceMultiplier;

        // Aplica la fuerza de disparo a la bala
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero; // Resetea la velocidad antes de aplicar la nueva fuerza
            rb.AddForce(shootDirection * shootForce, ForceMode.Impulse);
        }
    }

    // Método para encontrar el objetivo más cercano basado en el tag
    void FindClosestTarget()
    {
        // Encuentra todos los objetos con el tag del objetivo
        GameObject[] soldiers = GameObject.FindGameObjectsWithTag(targetTag);
        float closestDistance = Mathf.Infinity;
        GameObject closestSoldier = null;

        // Itera sobre todos los soldados y encuentra el más cercano
        foreach (GameObject soldier in soldiers)
        {
            float distance = Vector3.Distance(transform.position, soldier.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestSoldier = soldier;
            }
        }

        // Asigna el soldado más cercano como el objetivo actual
        target = closestSoldier;
    }
}
