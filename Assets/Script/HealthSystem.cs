using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f; // Salud máxima
    public float currentHealth; // Salud actual

    public float CurrentHealth => currentHealth; // Propiedad para acceder a la salud actual

    private void Awake()
    {
        currentHealth = maxHealth; // Inicializa la salud actual con el valor máximo
    }

    // Método para aplicar daño
    public void TakeDamage(float amount)
    {
        currentHealth -= amount; // Reduce la salud actual
        if (currentHealth <= 0)
        {
            Die(); // Llama al método de muerte si la salud es 0 o menos
        }
    }

    // Método llamado cuando la salud llega a 0
    protected virtual void Die()
    {
        // Puedes implementar aquí la lógica de muerte, como devolver al pool
        gameObject.SetActive(false); // Desactiva el objeto (o puedes devolverlo al pool)
    }
}
