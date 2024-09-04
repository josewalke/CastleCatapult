using UnityEngine;

public class SoldierController : MonoBehaviour
{
    [SerializeField] private string enemyTag = "Enemy"; // Tag del enemigo
    [SerializeField] private string enemyCastleTag = "EnemyCastle"; // Tag del castillo enemigo
    [SerializeField] private float damageAmount = 10f; // Cantidad de daño al colisionar con un enemigo

    private bool isMoving = true; // Indica si el soldado está en movimiento

    private void Update()
    {
        if (isMoving)
        {
            // Movimiento del soldado hacia adelante
            transform.Translate(Vector3.forward * Time.deltaTime * 5f); // Ajusta la velocidad según sea necesario
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(enemyTag))
        {
            Debug.Log("Soldier collided with an Enemy!");
            isMoving = false;

            // Aplica daño al enemigo
            HealthSystem enemyHealth = other.GetComponent<HealthSystem>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damageAmount);
            }
        }
        else if (other.CompareTag(enemyCastleTag))
        {
            Debug.Log("Soldier reached the EnemyCastle!");
            ReturnToPool();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(enemyTag))
        {
            Debug.Log("Soldier is no longer colliding with an Enemy!");
            isMoving = true;
        }
    }

    private void ReturnToPool()
    {
        isMoving = false;
        gameObject.SetActive(false);
    }
}
