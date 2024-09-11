using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Tags para identificar otros objetos con los que el enemigo puede interactuar
    [SerializeField] private string enemyTag = "Enemy";             // Tag para detectar otros enemigos
    [SerializeField] private string soldierTag = "Soldier";         // Tag del soldado enemigo
    [SerializeField] private string enemyCastleTag = "EnemyCastle"; // Tag del castillo enemigo

    // Parámetros de combate y movimiento
    [SerializeField] private float damageAmount = 10f;              // Cantidad de daño que el enemigo inflige al soldado
    [SerializeField] private float speed = 5f;                      // Velocidad de movimiento del enemigo
    [SerializeField] private float attackCooldown = 1f;             // Tiempo de espera entre ataques consecutivos

    // Variables de estado
    private bool isMoving = true;                                   // Indica si el enemigo está en movimiento
    private bool isInCombat = false;                                // Indica si el enemigo está en combate
    private float lastAttackTime = 0f;                              // Registra el momento del último ataque

    // Referencias a otros componentes
    private HealthSystem currentTarget;                             // Referencia al soldado que está siendo atacado
    private EnemyPool enemyPool;                                    // Referencia al pool del que proviene el enemigo

    // Inicializa la referencia al pool
    public void Initialize(EnemyPool pool)
    {
        enemyPool = pool; // Asocia el enemigo con su pool para poder devolverlo cuando sea necesario
    }

    private void Update()
    {
        // Movimiento hacia adelante si el enemigo no está en combate
        if (isMoving)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed); // Movimiento continuo hacia adelante
        }

        // Si el enemigo está en combate y tiene un objetivo válido, ataca repetidamente
        if (isInCombat && currentTarget != null && Time.time >= lastAttackTime + attackCooldown)
        {
            Attack(currentTarget); // Ejecuta el ataque
            lastAttackTime = Time.time; // Actualiza el tiempo del último ataque
        }

        // Si el objetivo (soldado) ha sido derrotado, reanuda el movimiento
        if (isInCombat && currentTarget != null && !currentTarget.IsAlive)
        {
            ResumeMovement(); // Reanuda el movimiento si el soldado muere
            isInCombat = false; // Sale del modo de combate
            currentTarget = null; // Elimina la referencia al objetivo
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Si colisiona con otro enemigo, se detiene
        if (other.CompareTag(enemyTag))
        {
            Debug.Log("Enemy collided with another Enemy! Stopping.");
            StopMovement(); // Detiene el movimiento al chocar con otro enemigo
        }
        // Si colisiona con un soldado, inicia el combate
        else if (other.CompareTag(soldierTag))
        {
            Debug.Log("Enemy collided with a Soldier!");
            StopMovement(); // Detiene el movimiento al entrar en combate
            isInCombat = true; // Indica que el enemigo está en combate
            currentTarget = other.GetComponent<HealthSystem>(); // Guarda la referencia del soldado para atacarlo
        }
        // Si llega al castillo enemigo, vuelve al pool
        else if (other.CompareTag(enemyCastleTag))
        {
            Debug.Log("Enemy reached the EnemyCastle!");
            ReturnToPool(); // El enemigo vuelve al pool cuando llega al castillo enemigo
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Si deja de colisionar con un soldado, reanuda el movimiento
        if (other.CompareTag(soldierTag))
        {
            Debug.Log("Enemy is no longer colliding with a Soldier! Resuming movement.");
            ResumeMovement(); // Reanuda el movimiento al dejar de colisionar con el soldado
            currentTarget = null; // Borra la referencia al objetivo
            isInCombat = false; // Sale del modo de combate
        }
    }

    // Método para atacar al soldado
    private void Attack(HealthSystem target)
    {
        if (target != null && target.IsAlive)
        {
            Debug.Log("Enemy is attacking the soldier!");
            target.TakeDamage(damageAmount); // Aplica daño al soldado
        }
    }

    // Método para detener el movimiento
    private void StopMovement()
    {
        isMoving = false; // Cambia el estado de movimiento a falso
    }

    // Método para reanudar el movimiento
    private void ResumeMovement()
    {
        isMoving = true; // Cambia el estado de movimiento a verdadero
    }

    // Método para volver al pool
    private void ReturnToPool()
    {
        StopMovement(); // Detiene el movimiento antes de regresar al pool
        enemyPool.ReturnToPool(this); // Devuelve el enemigo al pool de donde fue tomado
    }
}
