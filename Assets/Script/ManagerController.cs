using UnityEngine;

// Controlador de la cámara que rota, ajusta la posición y la distancia alrededor de un objetivo.
public class ManagerController : MonoBehaviour
{
    [SerializeField] private Transform target; // El objeto alrededor del cual rota la cámara
    [SerializeField] private float rotationSpeed = 50f; // Velocidad de rotación alrededor del objetivo
    [SerializeField] private float movementSpeed = 5f; // Velocidad de movimiento de la cámara arriba/abajo
    [SerializeField] private float zoomSpeed = 10f; // Velocidad de zoom (acercar/alejar la cámara)
    [SerializeField] private float minDistance = 5f; // Distancia mínima que la cámara puede estar del objetivo
    [SerializeField] private float maxDistance = 20f; // Distancia máxima que la cámara puede estar del objetivo

    private float currentDistance; // Distancia actual entre la cámara y el objetivo

    // Método que se llama cuando el script se inicializa
    private void Awake()
    {
        // Calcula la distancia inicial entre la cámara y el objetivo
        currentDistance = Vector3.Distance(transform.position, target.position);
    }

    // Método que se llama en cada frame
    private void Update()
    {
        HandleCameraMovement(); // Controla el movimiento y la rotación de la cámara
        HandleTimeControl();     // Controla la velocidad del tiempo en el juego
    }

    // Controla el movimiento, la rotación y el zoom de la cámara
    private void HandleCameraMovement()
    {
        // Si hay un objetivo válido
        if (target != null)
        {
            // Rotación de la cámara alrededor del objetivo en base al input horizontal (A/D o flechas izquierda/derecha)
            float horizontalInput = GetHorizontalInput();
            float rotationAmount = horizontalInput * rotationSpeed * Time.deltaTime;
            transform.RotateAround(target.position, Vector3.up, -rotationAmount); // Rota la cámara alrededor del eje Y

            // Movimiento vertical de la cámara (arriba/abajo) en base al input vertical (W/S o flechas arriba/abajo)
            float upDownInput = GetUpDownInput();
            float movementAmount = upDownInput * movementSpeed * Time.deltaTime;
            transform.position += transform.up * movementAmount; // Mueve la cámara arriba o abajo

            // Zoom de la cámara en base al input (Q para alejar, E para acercar)
            float zoomInput = GetZoomInput();
            float zoomAmount = zoomInput * zoomSpeed * Time.deltaTime;
            // Ajusta la distancia actual con el zoom, asegurando que esté dentro de los límites establecidos
            currentDistance = Mathf.Clamp(currentDistance - zoomAmount, minDistance, maxDistance);

            // Calcula la dirección hacia el objetivo y ajusta la posición de la cámara en función de la nueva distancia
            Vector3 direction = (transform.position - target.position).normalized;
            transform.position = target.position + direction * currentDistance;

            // Asegura que la cámara siempre esté mirando hacia el objetivo
            transform.LookAt(target);
        }
    }

    // Controla la velocidad del tiempo del juego (aumenta/disminuye con las teclas M y N)
    private void HandleTimeControl()
    {
        // Aumenta la velocidad del juego al presionar la tecla M
        if (Input.GetKey(KeyCode.M))
        {
            Time.timeScale += 0.1f; // Incrementa el tiempo de juego
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0.1f, 10f); // Asegura que el valor esté dentro del rango permitido
        }
        // Disminuye la velocidad del juego al presionar la tecla N
        if (Input.GetKey(KeyCode.N))
        {
            Time.timeScale -= 0.1f; // Decrementa el tiempo de juego
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0.1f, 10f); // Asegura que el valor esté dentro del rango permitido
        }
    }

    // Obtiene la entrada horizontal (A/D o flechas izquierda/derecha)
    private float GetHorizontalInput()
    {
        return Input.GetAxis("Horizontal");
    }

    // Obtiene la entrada vertical (W/S o flechas arriba/abajo) para mover la cámara arriba o abajo
    private float GetUpDownInput()
    {
        return Input.GetAxis("Vertical");
    }

    // Obtiene la entrada de zoom (Q para alejar y E para acercar)
    private float GetZoomInput()
    {
        if (Input.GetKey(KeyCode.Q)) return -1f; // Aleja la cámara
        if (Input.GetKey(KeyCode.E)) return 1f;  // Acerca la cámara
        return 0f;  // Si no se presiona ninguna tecla de zoom, no hay cambio
    }
}
