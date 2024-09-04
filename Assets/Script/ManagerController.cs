using UnityEngine;

// Controlador de la cámara que rota, ajusta la posición y distancia alrededor de un objetivo
public class ManagerController : MonoBehaviour
{
    [SerializeField] private Transform target; // El objeto alrededor del cual rota la cámara
    [SerializeField] private float rotationSpeed = 50f; // Velocidad de rotación
    [SerializeField] private float movementSpeed = 5f; // Velocidad de movimiento arriba/abajo
    [SerializeField] private float zoomSpeed = 10f; // Velocidad de zoom
    [SerializeField] private float minDistance = 5f; // Distancia mínima al objetivo
    [SerializeField] private float maxDistance = 20f; // Distancia máxima al objetivo

    private float currentDistance; // Distancia actual de la cámara al objetivo

    private void Awake()
    {
        currentDistance = Vector3.Distance(transform.position, target.position);
    }

    private void Update()
    {
        HandleCameraMovement();
        HandleTimeControl();
    }

    private void HandleCameraMovement()
    {
        if (target != null)
        {
            // Obtén el input horizontal para rotación
            float horizontalInput = GetHorizontalInput();
            // Calcula la rotación en el eje Y
            float rotationAmount = horizontalInput * rotationSpeed * Time.deltaTime;
            // Rota la cámara alrededor del objetivo; "D" rotará a la derecha y "A" a la izquierda
            transform.RotateAround(target.position, Vector3.up, -rotationAmount);

            // Obtén el input vertical para movimiento arriba/abajo
            float upDownInput = GetUpDownInput();
            // Calcula el movimiento vertical
            float movementAmount = upDownInput * movementSpeed * Time.deltaTime;
            // Mueve la cámara arriba o abajo
            transform.position += transform.up * movementAmount;

            // Obtén el input de zoom (acercar/alejar)
            float zoomInput = GetZoomInput();
            // Calcula la cantidad de zoom
            float zoomAmount = zoomInput * zoomSpeed * Time.deltaTime;
            // Ajusta la distancia actual de la cámara
            currentDistance = Mathf.Clamp(currentDistance - zoomAmount, minDistance, maxDistance);
            // Ajusta la posición de la cámara
            Vector3 direction = (transform.position - target.position).normalized;
            transform.position = target.position + direction * currentDistance;
            // Asegúrate de que la cámara mire hacia el objetivo
            transform.LookAt(target);
        }
    }

    private void HandleTimeControl()
    {
        if (Input.GetKey(KeyCode.M))
        {
            Time.timeScale += 0.1f; // Aumentar la velocidad del juego
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0.1f, 10f); // Limitar el valor para evitar valores extremos
        }
        if (Input.GetKey(KeyCode.N))
        {
            Time.timeScale -= 0.1f; // Disminuir la velocidad del juego
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0.1f, 10f); // Limitar el valor para evitar valores negativos o cero
        }
    }

    // Métodos para obtener entradas de movimiento
    private float GetHorizontalInput()
    {
        return Input.GetAxis("Horizontal"); // Entrada horizontal (A/D o flechas)
    }

    private float GetUpDownInput()
    {
        return Input.GetAxis("Vertical"); // Entrada para mover la cámara hacia arriba o abajo usando W/S o flechas
    }

    private float GetZoomInput()
    {
        if (Input.GetKey(KeyCode.Q)) return -1f;
        if (Input.GetKey(KeyCode.E)) return 1f;
        return 0f;
    }
}
