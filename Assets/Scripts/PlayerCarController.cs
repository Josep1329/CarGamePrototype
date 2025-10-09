using UnityEngine;
using Fusion;

public class PlayerCarController : NetworkBehaviour
{
    [Header("Car Settings")]
    public float baseSpeed = 10f;
    public float clutchMultiplier = 2f;
    public float clutchWindow = 0.2f; // Tiempo óptimo para presionar el clutch

    private float currentSpeed;
    private bool clutchPressed;
    private float clutchPressTime;

    private void Start()
    {
        currentSpeed = baseSpeed;
    }

    public override void FixedUpdateNetwork()
    {
        if (Object.HasInputAuthority)
        {
            // Leer input del clutch
            clutchPressed = Input.GetKey(KeyCode.Space);
            if (clutchPressed)
            {
                clutchPressTime += Runner.DeltaTime;
            }
            else
            {
                // Si el clutch se suelta dentro de la ventana óptima, aumenta la velocidad
                if (clutchPressTime > 0 && clutchPressTime <= clutchWindow)
                {
                    currentSpeed = baseSpeed * clutchMultiplier;
                }
                else
                {
                    currentSpeed = baseSpeed;
                }
                clutchPressTime = 0f;
            }
        }

        // Movimiento automático hacia adelante
        transform.Translate(Vector3.forward * currentSpeed * Runner.DeltaTime);
    }
}
