using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// INHERITANCE - Lilly inherits from the abstract Car class
public class Lilly : Car
{
    [Header("Lilly Specific Settings")]
    [SerializeField] private float accelerationMultiplier = 1.0f; // Balanced acceleration
    [SerializeField] private float brakeEfficiency = 1.0f; // Balanced braking
    
    void Start()
    {
        // Initialize Lilly with balanced settings
        // Set Lilly's specific turn speed (balanced)
        turnSpeed = 50f; // Moderate turning speed for balanced handling
        StartEngine(); // Auto-start for testing
    }
    
    // POLYMORPHISM - Override abstract methods with Lilly-specific implementation
    public override void Accelerate()
    {
        // Balanced acceleration - smooth and consistent
        if (IsEngineRunning)
        {
            float newSpeed = CurrentSpeed + (Acceleration * accelerationMultiplier * Time.deltaTime);
            SetCurrentSpeed(newSpeed);
            
            // Debug info for testing
            Debug.Log($"{CarName} accelerating to {CurrentSpeed:F1} km/h");
        }
        else
        {
            Debug.Log($"{CarName} engine is off! Start the engine first.");
        }
    }
    
    public override void Brake()
    {
        // Balanced braking - reliable and steady
        float newSpeed = CurrentSpeed - (BrakeForce * brakeEfficiency * Time.deltaTime);
        SetCurrentSpeed(newSpeed);
        
        // Debug info for testing
        Debug.Log($"{CarName} braking to {CurrentSpeed:F1} km/h");
    }
    
    // VIRTUAL METHOD - Can be overridden by derived classes
    public virtual void StartEngine()
    {
        SetEngineState(true);
        Debug.Log($"{CarName} engine started! Ready to drive.");
    }
    
    public virtual void StopEngine()
    {
        SetEngineState(false);
        SetCurrentSpeed(0f);
        Debug.Log($"{CarName} engine stopped.");
    }

    void Update()
    {
        // Test controls (can be removed later when we add proper input)
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            Accelerate();
        }
        
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            Brake();
        }
        
        // STEERING CONTROLS
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            TurnLeft();
        }
        
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            TurnRight();
        }
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (IsEngineRunning)
                StopEngine();
            else
                StartEngine();
        }
        
        // MOVEMENT - Actually move the car based on current speed
        if (CurrentSpeed > 0)
        {
            // Convert speed to Unity units and move forward
            float movementSpeed = CurrentSpeed / 3.6f; // Convert km/h to m/s
            transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
        }
    }
}
