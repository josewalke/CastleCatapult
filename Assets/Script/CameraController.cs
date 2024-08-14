using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // El objeto que la cámara observará
    public float distance = 10.0f; // Distancia desde la cámara al objeto
    public float minDistance = 2.0f; // Distancia mínima permitida
    public float maxDistance = 20.0f; // Distancia máxima permitida
    public float rotationSpeed = 120.0f; // Velocidad de rotación
    public float zoomSpeed = 2.0f; // Velocidad de zoom

    public float yMinLimit = -20f; // Límite mínimo de ángulo vertical
    public float yMaxLimit = 80f; // Límite máximo de ángulo vertical

    private float x = 0.0f; // Ángulo horizontal
    private float y = 0.0f; // Ángulo vertical

    // Use this for initialization
    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
    }

    void Update()
    {
        // Aumenta la velocidad del tiempo con la tecla E
        if (Input.GetKeyDown(KeyCode.M))
        {
            Time.timeScale += 0.5f; // Incrementa la velocidad del tiempo
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            Time.maximumDeltaTime = 0.1f * Time.timeScale;
        }

        // Disminuye la velocidad del tiempo con la tecla Q
        if (Input.GetKeyDown(KeyCode.N))
        {
            Time.timeScale = Mathf.Max(0.1f, Time.timeScale - 0.5f); // Decrementa la velocidad del tiempo sin caer por debajo de 0.1
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            Time.maximumDeltaTime = 0.1f * Time.timeScale;
        }

        // Asegúrate de que la velocidad del tiempo no sea inferior a 0
        Time.timeScale = Mathf.Max(0.0f, Time.timeScale);
    }


    void LateUpdate()
    {
        if (target)
        {
            // Obtener la entrada del teclado para la rotación
            float horizontal = Input.GetAxis("Horizontal"); // Teclas A y D
            float vertical = Input.GetAxis("Vertical"); // Teclas W y S

            x += horizontal * rotationSpeed * Time.deltaTime;
            y -= vertical * rotationSpeed * Time.deltaTime;

            // Restringir los ángulos verticales
            y = ClampAngle(y, yMinLimit, yMaxLimit);

            // Obtener la entrada del teclado para el zoom
            if (Input.GetKey(KeyCode.E))
            {
                distance -= zoomSpeed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.Q))
            {
                distance += zoomSpeed * Time.deltaTime;
            }

            // Restringir la distancia
            distance = Mathf.Clamp(distance, minDistance, maxDistance);

            // Calcular la rotación y la posición de la cámara
            Quaternion rotation = Quaternion.Euler(y, x, 0);
            Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.position;

            // Asignar la rotación y la posición a la cámara
            transform.rotation = rotation;
            transform.position = position;
        }
    }

    // Método para restringir los ángulos
    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}
