using UnityEngine;
using UnityEngine.UI;

// Interfaz que define las funciones básicas para un sistema de salud
public interface IHealth
{
    // Método para que el objeto tome daño
    void TakeDamage(float damageAmount);

    // Propiedad que devuelve la salud actual
    float CurrentHealth { get; }

    // Propiedad que indica si el objeto está vivo
    bool IsAlive { get; }
}

// Sistema de salud que implementa la interfaz IHealth
public class HealthSystem : MonoBehaviour, IHealth
{
    [SerializeField] private float maxHealth = 100f;  // Salud máxima del objeto
    [SerializeField] private float currentHealth;     // Salud actual del objeto
    [SerializeField] private Image healthBar;         // Barra de salud en la UI

    // Inicializa el sistema de salud cuando el objeto se activa
    private void Start()
    {
        currentHealth = maxHealth;  // La salud comienza en el valor máximo
        UpdateHealthBar();  // Actualiza la barra de salud en la UI para reflejar la salud inicial
    }

    // Método para recibir daño y reducir la salud
    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;  // Reduce la salud por la cantidad de daño recibido
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);  // Asegura que la salud no sea menor a 0 o mayor a la salud máxima

        Debug.Log(gameObject.name + " received damage! Current health: " + currentHealth);  // Muestra un mensaje en la consola

        UpdateHealthBar();  // Actualiza la barra de salud en la UI tras recibir daño

        // Si la salud es igual o menor a 0, el objeto "muere"
        if (currentHealth <= 0f)
        {
            Die();  // Llama al método para manejar la muerte del objeto
        }
    }

    // Propiedad que devuelve la salud actual
    public float CurrentHealth => currentHealth;

    // Propiedad que indica si el objeto sigue vivo
    public bool IsAlive => currentHealth > 0;

    // Método que maneja la "muerte" del objeto
    private void Die()
    {
        Debug.Log(gameObject.name + " has died!");  // Mensaje en la consola indicando que el objeto ha muerto
        gameObject.SetActive(false);  // Desactiva el objeto, simulando su "muerte" en el juego
    }

    // Método para actualizar la barra de salud en la UI
    private void UpdateHealthBar()
    {
        Debug.Log("HOLAAA");  // Mensaje de depuración para verificar si se ejecuta
        if (healthBar != null)  // Verifica si la barra de salud está asignada en la UI
        {
            // Actualiza la cantidad que rellena la barra en función del porcentaje de salud restante
            healthBar.fillAmount = currentHealth / maxHealth;
        }
    }
}
