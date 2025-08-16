using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Car : MonoBehaviour
{
    // ENCAPSULATION - Private fields with controlled access
    [Header("Car Properties")]
    [SerializeField] private float maxSpeed = 300f;
    [SerializeField] private float acceleration = 20f;
    [SerializeField] private float brakeForce = 150f;
    [SerializeField] protected float turnSpeed = 90f; // Protected so derived classes can access
    [SerializeField] private string carName = "Generic Car";
    
    // Runtime properties that change during gameplay
    private float currentSpeed = 0f;
    private bool engineRunning = false;
    
    // PUBLIC PROPERTIES (Getters/Setters) - Demonstrates Encapsulation
    public float MaxSpeed => maxSpeed;
    public float CurrentSpeed => currentSpeed;
    public float Acceleration => acceleration;
    public float BrakeForce => brakeForce;
    public float TurnSpeed => turnSpeed;
    public string CarName => carName;
    public bool IsEngineRunning => engineRunning;
    
    // Protected setters for derived classes
    protected void SetCurrentSpeed(float speed)
    {
        currentSpeed = Mathf.Clamp(speed, 0f, maxSpeed);
    }
    
    protected void SetEngineState(bool running)
    {
        engineRunning = running;
    }

    // ABSTRACTION - Abstract methods that derived classes must implement
    // Each car type will have different acceleration behavior
    public abstract void Accelerate();
    
    // Each car type will have different braking behavior  
    public abstract void Brake();
    
    // STEERING - Common steering behavior for all cars
    // Uses the specific turnSpeed of each car type
    public virtual void TurnLeft()
    {
        if (currentSpeed > 0.1f) // Only turn when moving
        {
            float turnAmount = turnSpeed * Time.deltaTime;
            transform.Rotate(0, -turnAmount, 0);
        }
    }
    
    public virtual void TurnRight()
    {
        if (currentSpeed > 0.1f) // Only turn when moving
        {
            float turnAmount = turnSpeed * Time.deltaTime;
            transform.Rotate(0, turnAmount, 0);
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
