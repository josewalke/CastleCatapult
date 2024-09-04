using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private string soldierTag = "Soldier"; // Tag del soldado
    [SerializeField] private string playerCastleTag = "PlayerCastle"; // Tag del castillo del jugador
    [SerializeField] private float damageAmount = 10f; // Cantidad de daño al colisionar con un soldado

    private bool isMoving = true; // Indica si el enemigo está en movimiento

    private void Update()
    {
        if (isMoving)
        {
            // Movimiento del enemigo hacia adelante
            transform.Translate(Vector3.forward * Time.deltaTime * 5f); // Ajusta la velocidad según sea necesario
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(soldierTag))
        {
            Debug.Log("Enemy collided with a Soldier!");
            isMoving = false;

            // Aplica daño al soldado
            HealthSystem soldierHealth = other.GetComponent<HealthSystem>();
            if (soldierHealth != null)
            {
                soldierHealth.TakeDamage(damageAmount);
            }
        }
        else if (other.CompareTag(playerCastleTag))
        {
            Debug.Log("Enemy reached the PlayerCastle!");
            ReturnToPool();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(soldierTag))
        {
            Debug.Log("Enemy is no longer colliding with a Soldier!");
            isMoving = true;
        }
    }

    private void ReturnToPool()
    {
        isMoving = false;
        gameObject.SetActive(false);
    }
}
