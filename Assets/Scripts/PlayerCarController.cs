using UnityEngine;
using Fusion;

public class PlayerCarController : NetworkBehaviour
{
    [Header("Car Settings")]
    public float baseSpeed = 10f;
    public float clutchMultiplier = 2f;
    public float clutchWindow = 0.2f; // Tiempo óptimo para presionar el clutch
    public float boostDuration = 1.0f; // duración del boost en segundos

    // Networked speed so other peers see authoritative value
    [Networked]
    public float CurrentSpeed { get; set; }

    [Networked]
    public float BoostTimeRemaining { get; set; }
    private bool clutchPressed;
    private float clutchPressTime;

    public override void Spawned()
    {
        // Initialize networked values when spawned
        CurrentSpeed = baseSpeed;
        BoostTimeRemaining = 0f;
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
                // Si el clutch se suelta dentro de la ventana óptima, otorgar un boost temporal
                if (clutchPressTime > 0 && clutchPressTime <= clutchWindow)
                {
                    BoostTimeRemaining = boostDuration;
                }
                // reset press timer
                clutchPressTime = 0f;
            }

            // Update speed based on boost time
            if (BoostTimeRemaining > 0f)
            {
                CurrentSpeed = baseSpeed * clutchMultiplier;
                BoostTimeRemaining -= Runner.DeltaTime;
                if (BoostTimeRemaining < 0f) BoostTimeRemaining = 0f;
            }
            else
            {
                CurrentSpeed = baseSpeed;
            }

            // Movimiento automático hacia adelante (solo por la autoridad de input)
            transform.Translate(Vector3.forward * CurrentSpeed * Runner.DeltaTime);
        }
    }
}
