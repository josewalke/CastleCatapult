using UnityEngine;

public class SoldierController : MonoBehaviour
{
    [SerializeField] private string soldierTag = "Soldier";         // Tag para detectar otros soldados
    [SerializeField] private string enemyTag = "Enemy";             // Tag del enemigo
    [SerializeField] private string enemyCastleTag = "EnemyCastle"; // Tag del castillo enemigo
    [SerializeField] private float damageAmount = 10f;              // Cantidad de daño que el soldado hace al colisionar con un enemigo
    [SerializeField] private float speed = 5f;                      // Velocidad de movimiento del soldado
    [SerializeField] private float attackCooldown = 1f;             // Tiempo de espera entre ataques sucesivos

    private bool isMoving = true;                                   // Indica si el soldado está actualmente en movimiento
    private bool isInCombat = false;                                // Indica si el soldado está en combate
    private float lastAttackTime = 0f;                              // Almacena el tiempo en que se realizó el último ataque

    private HealthSystem currentTarget;                             // Referencia al sistema de salud del enemigo actual
    private SoldierPool soldierPool;                                // Referencia al SoldierPool para devolver el soldado al pool

    // Inicializa la referencia al pool del que este soldado forma parte
    public void Initialize(SoldierPool pool)
    {
        soldierPool = pool;
    }

    private void Update()
    {
        // Si el soldado está en movimiento, lo desplazamos hacia adelante
        if (isMoving)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }

        // Si está en combate y tiene un objetivo válido, verifica si es tiempo de atacar de nuevo
        if (isInCombat && currentTarget != null && Time.time >= lastAttackTime + attackCooldown)
        {
            Attack(currentTarget);  // Ataca al enemigo
            lastAttackTime = Time.time;  // Actualiza el tiempo del último ataque
        }

        // Si el enemigo ha muerto, el soldado reanuda el movimiento
        if (isInCombat && currentTarget != null && !currentTarget.IsAlive)
        {
            ResumeMovement();  // Reanuda el movimiento
            isInCombat = false;  // Sale del estado de combate
            currentTarget = null;  // Resetea el objetivo actual
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Si el soldado colisiona con otro soldado, se detiene
        if (other.CompareTag(soldierTag))
        {
            Debug.Log("Soldier collided with another soldier! Stopping.");
            StopMovement();  // Detiene el movimiento
        }
        // Si colisiona con un enemigo, inicia el combate y se detiene
        else if (other.CompareTag(enemyTag))
        {
            Debug.Log("Soldier collided with an Enemy!");
            StopMovement();  // Detiene el movimiento
            isInCombat = true;  // Entra en combate

            // Guarda la referencia al enemigo para atacarlo continuamente
            currentTarget = other.GetComponent<HealthSystem>();
        }
        // Si llega al castillo enemigo, vuelve al pool
        else if (other.CompareTag(enemyCastleTag))
        {
            Debug.Log("Soldier reached the EnemyCastle!");
            ReturnToPool();  // Devuelve el soldado al pool
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Si el soldado deja de colisionar con un enemigo, reanuda el movimiento
        if (other.CompareTag(enemyTag))
        {
            Debug.Log("Soldier is no longer colliding with an Enemy! Resuming movement.");
            ResumeMovement();  // Reanuda el movimiento
            currentTarget = null;  // Limpia la referencia al enemigo
            isInCombat = false;  // Sale del combate
        }
    }

    // Método para realizar el ataque al enemigo
    private void Attack(HealthSystem target)
    {
        // Verifica que el objetivo es válido y está vivo antes de atacarlo
        if (target != null && target.IsAlive)
        {
            Debug.Log("Soldier is attacking the enemy!");
            target.TakeDamage(damageAmount);  // Aplica el daño al enemigo
        }
    }

    // Método para detener el movimiento del soldado
    private void StopMovement()
    {
        isMoving = false;
    }

    // Método para reanudar el movimiento del soldado
    private void ResumeMovement()
    {
        isMoving = true;
    }

    // Método para devolver el soldado al pool
    private void ReturnToPool()
    {
        StopMovement();  // Detiene el movimiento
        soldierPool.ReturnToPool(this);  // Devuelve este soldado al pool correspondiente
    }
}
