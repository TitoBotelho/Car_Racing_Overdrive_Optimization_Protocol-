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
    [SerializeField] protected float turnSpeed = 90f; // Max yaw degrees per second at full steer
    [SerializeField] private string carName = "Generic Car";

    [Header("Steering Input Settings")] // New simplified steering smoothing
    [Tooltip("How fast steering builds toward full lock (units per second). Higher = quicker response.")]
    [SerializeField] private float steeringAcceleration = 4f;
    [Tooltip("How fast steering returns to center when input released (units per second). Higher = snaps back faster.")]
    [SerializeField] private float steeringDeceleration = 6f;
    [Tooltip("Multiplier applied to current steering when player instantly reverses direction (to avoid harsh snap). 0.3 = drop to 30% then build.")]
    [Range(0f, 1f)] [SerializeField] private float directionChangePenalty = 0.3f;

    [Header("Speed Influence")] // Keeps previous speed-based reduction
    [Tooltip("Curve: X = normalized speed (0..1), Y = steering intensity multiplier.")]
    [SerializeField] private AnimationCurve speedSteerCurve = new AnimationCurve(
        new Keyframe(0f, 1f),
        new Keyframe(0.3f, 0.9f),
        new Keyframe(0.6f, 0.7f),
        new Keyframe(1f, 0.4f)
    );

    // Runtime properties that change during gameplay
    private float currentSpeed = 0f;
    private bool engineRunning = false;

    // Steering state (new approach)
    private float steeringInput = 0f;      // Smoothed steering value (-1..1)
    private float desiredSteerRaw = 0f;    // Raw target for this frame from TurnLeft/TurnRight

    // PUBLIC PROPERTIES
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

    // ABSTRACT BEHAVIOR
    public abstract void Accelerate();
    public abstract void Brake();

    // INPUT REGISTRATION (called by derived classes each frame a key is held)
    public virtual void TurnLeft()
    {
        if (currentSpeed > 0.1f)
            desiredSteerRaw = -1f; // Will be processed in LateUpdate
    }

    public virtual void TurnRight()
    {
        if (currentSpeed > 0.1f)
            desiredSteerRaw = 1f; // Will be processed in LateUpdate
    }

    // PROCESS STEERING (runs after derived Update so raw inputs for the frame are known)
    void LateUpdate()
    {
        // Determine target (-1, 0, 1)
        float target = desiredSteerRaw; // if no key pressed remains 0

        // If reversing direction instantly, apply penalty to soften snap
        if (target != 0f && Mathf.Sign(target) != Mathf.Sign(steeringInput) && Mathf.Abs(steeringInput) > 0.001f)
        {
            steeringInput *= directionChangePenalty; // drop magnitude
        }

        // Choose rate (accel vs decel)
        float rate = (target != 0f) ? steeringAcceleration : steeringDeceleration;
        steeringInput = Mathf.MoveTowards(steeringInput, target, rate * Time.deltaTime);

        // Apply rotation if moving and we have any steering
        if (Mathf.Abs(steeringInput) > 0.001f && currentSpeed > 0.1f)
        {
            float speedFactor = speedSteerCurve.Evaluate(currentSpeed / maxSpeed);
            float yaw = steeringInput * turnSpeed * speedFactor * Time.deltaTime;
            transform.Rotate(0f, yaw, 0f);
        }

        // Reset raw input for next frame (must be set again by TurnLeft/TurnRight)
        desiredSteerRaw = 0f;
    }
}
