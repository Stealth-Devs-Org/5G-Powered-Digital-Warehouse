using UnityEngine;

public class AGVAnimate : MonoBehaviour
{
    public Vector3 targetPosition;    // Target position for initial movement
    public float moveSpeed = 2f;      // Movement speed
    private bool reachedTarget = false; // Flag to check if target is reached
    private bool readyToMove = false; // Flag to start movement after timer
    private bool isRotating = false;  // Flag to indicate the rotation is ongoing
    private float timer = 0f;         // Timer to track elapsed time
    public float timertotal = 10f;    // Total time before movement starts
    public float rotationSpeed = 5f;  // Rotation speed (degrees per second)

    private Animator anim;            // Reference to the Animator component
    private Quaternion targetRotation; // The final rotation after turning

    void Start()
    {
        // Get the Animator component
        anim = GetComponent<Animator>();

        // Safety check for Animator
        if (anim == null)
        {
            Debug.LogWarning("Animator not found on the GameObject. Please attach an Animator component.");
        }
    }

    void Update()
    {
        // Increment the timer
        timer += Time.deltaTime;

        // Allow the AGV to move after timer reaches timertotal
        if (!readyToMove && timer >= timertotal)
        {
            readyToMove = true;
        }

        // Movement and rotation logic when ready
        if (readyToMove)
        {
            if (!reachedTarget)
            {
                MoveToTarget();
            }
            else if (isRotating)
            {
                RotateToTarget();
            }
            else
            {
                MoveForwardAfterRotation();
            }
        }

        // Update animations based on the current state
        if (anim != null)
        {
            UpdateAnimation();
        }
    }

    // Move the AGV to the target position
    void MoveToTarget()
    {
        // Move towards the target position
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );

        // Check if the object has reached the target position
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            reachedTarget = true;
            isRotating = true;

            // Set the target rotation to turn right 90 degrees
            targetRotation = Quaternion.Euler(0, transform.eulerAngles.y + 90, 0);
        }
    }

    // Rotate the AGV to the target rotation
    void RotateToTarget()
    {
        // Gradually rotate to the target rotation
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );

        // Check if rotation is complete
        if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
        {
            isRotating = false; // Stop rotating
        }
    }

    // Move forward in the new direction after rotation
    void MoveForwardAfterRotation()
    {
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }

    // Update the animation states dynamically
    void UpdateAnimation()
    {
        if (!readyToMove) // Idle state before movement starts
        {
            anim.SetBool("moving", false);
            anim.SetBool("idle", true);
            anim.SetBool("loading", false);
        }
        else if (readyToMove && !reachedTarget) // Moving to target
        {
            anim.SetBool("moving", true);
            anim.SetBool("idle", false);
            anim.SetBool("loading", false);
        }
        else if (reachedTarget && isRotating) // Loading/Rotating state
        {
            anim.SetBool("loading", true);
            anim.SetBool("moving", false);
            anim.SetBool("idle", false);
        }
        else // After rotation, moving forward
        {
            anim.SetBool("moving", true);
            anim.SetBool("idle", false);
            anim.SetBool("loading", false);
        }
    }
}
