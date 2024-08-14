using UnityEngine;
using UnityEngine.UI;

public class SoldierController : MonoBehaviour
{
    public float speed = 5f;                // Velocidad del soldado
    public string enemyTag = "Enemy";       // Tag para detectar enemigos
    public string allyTag = "Ally";         // Tag para detectar aliados
    public string buildingTag = "Building"; // Tag para detectar edificios

    public int maxHealth = 100;             // Salud máxima del soldado
    public Image healthBar;                 // Imagen de la barra de salud en la UI
    public int attackDamage = 20;           // Daño que inflige el soldado al atacar
    public float attackCooldown = 1.5f;     // Tiempo de enfriamiento entre ataques
    private float lastAttackTime;           // Tiempo en el que se realizó el último ataque

    public int currentHealth;               // Salud actual del soldado
    private bool isStopped = false;         // Estado que indica si el soldado está detenido
    private Collider currentTarget;         // Referencia al objetivo actual (enemigo)

    private CastleController castleController; // Referencia al controlador del castillo

    void Start()
    {
        currentHealth = maxHealth; // Inicializa la salud actual del soldado al valor máximo
    }

    void Update()
    {
        if (!isStopped)
        {
            MoveSoldier(); // Mueve al soldado solo si no está detenido
        }
        else if (currentTarget != null && Time.time >= lastAttackTime + attackCooldown)
        {
            Attack(); // Realiza el ataque si el tiempo de enfriamiento ha pasado
        }
    }

    // Método para asignar la referencia del CastleController
    public void SetCastleController(CastleController castle)
    {
        castleController = castle; // Asigna el controlador del castillo
    }

    void MoveSoldier()
    {
        // Mueve al soldado hacia adelante a la velocidad especificada
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void Attack()
    {
        if (currentTarget != null)
        {
            // Obtiene el componente EnemySoldierController del objetivo actual
            EnemySoldierController enemy = currentTarget.GetComponent<EnemySoldierController>();
            if (enemy != null)
            {
                // Inflica daño al enemigo
                enemy.TakeDamage(attackDamage);
                lastAttackTime = Time.time; // Actualiza el tiempo del último ataque

                // Verifica si el enemigo ha muerto
                if (enemy.currentHealth <= 0)
                {
                    currentTarget = null; // Elimina la referencia al enemigo
                    ResumeMovement(); // Reanuda el movimiento del soldado
                }
            }
        }
    }

    public void TakeDamage(int damage)
    {
        // Reduce la salud del soldado en función del daño recibido
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0; // Asegura que la salud no sea menor que 0
            Die(); // Llama al método para manejar la muerte del soldado
        }
        Debug.Log("Soldier received damage. Current health: " + currentHealth);
        UpdateHealthBar(); // Actualiza la barra de salud en la UI
    }

    void Die()
    {
        Debug.Log("Soldier has died.");
        ReturnToPool(); // Llama a un método para gestionar el retorno al pool o la destrucción del objeto
    }

    private void OnEnable()
    {
        ResetSoldier(); // Resetea el estado del soldado cuando se habilita
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Verifica si el soldado colisiona con un objeto que tenga el tag "Ball"
        if (collision.collider.CompareTag("Ball"))
        {
            TakeDamage(30); 
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Verifica el tag del objeto que entra en el trigger
        if (other.CompareTag(enemyTag))
        {
            Debug.Log("Detected enemy: " + other.gameObject.name);
            currentTarget = other; // Guarda la referencia al enemigo
            StopMovement(); // Detiene el movimiento al detectar un enemigo
        }
        else if (other.CompareTag(allyTag))
        {
            Debug.Log("Detected ally: " + other.gameObject.name);
            StopMovement(); // Detiene el movimiento si un aliado está delante
        }
        else if (other.CompareTag(buildingTag))
        {
            Debug.Log("Detected building: " + other.gameObject.name);
            ReturnToPool(); // Retorna al pool si se detecta un edificio
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Verifica el tag del objeto que sale del trigger
        if (other.CompareTag(enemyTag))
        {
            Debug.Log("Lost sight of enemy: " + other.gameObject.name);
            if (currentTarget == other)
            {
                currentTarget = null; // Elimina la referencia al enemigo
                ResumeMovement(); // Reanuda el movimiento si el enemigo salió del área
            }
        }
        else if (other.CompareTag(allyTag))
        {
            Debug.Log("Lost sight of ally: " + other.gameObject.name);
            if (currentTarget == null) // Solo reanuda el movimiento si no hay otro objetivo
            {
                ResumeMovement();
            }
        }
        else if (other.CompareTag(buildingTag))
        {
            Debug.Log("Lost sight of building: " + other.gameObject.name);
            // Aquí podrías manejar acciones adicionales si es necesario
        }
    }

    void StopMovement()
    {
        // Cambia el estado para detener el movimiento del soldado
        isStopped = true;
    }

    void ResumeMovement()
    {
        // Cambia el estado para reanudar el movimiento del soldado
        isStopped = false;
    }

    void ReturnToPool()
    {
        // Verifica si el CastleController está asignado y lo retorna al pool
        if (castleController != null)
        {
            castleController.ReturnSoldierToPool(gameObject); // Devuelve el soldado al pool
        }
        else
        {
            gameObject.SetActive(false); // Si no hay CastleController, solo desactiva el objeto
        }
    }

    void UpdateHealthBar()
    {
        // Actualiza la barra de salud en la UI
        healthBar.fillAmount = (float)currentHealth / maxHealth;
    }

    public void ResetSoldier()
    {
        // Restablece la salud del soldado a su valor máximo
        currentHealth = maxHealth;
        // Actualiza la barra de salud
        UpdateHealthBar();
        // Reactiva el movimiento del soldado
        ResumeMovement();
    }
}
