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

    Animator anim; // Reference to the animator component

    private Quaternion targetRotation; // The final rotation after turning

    void Start()
    {
        // Get the animator component
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        TriggerAnimation();
        // Increment the timer
        timer += Time.deltaTime;
        

        // After timertotal seconds, allow the AGV to move
        if (!readyToMove && timer >= timertotal)
        {
            readyToMove = true;
        }

        // Movement logic when readyToMove is true
        if (readyToMove)
        {
            if (!reachedTarget)
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
            else if (isRotating)
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
            else
            {
                // Move forward in the new direction after rotation
                transform.position += transform.forward * moveSpeed * Time.deltaTime;
            }
        }
    }

    // Function to trigger the animation
    public void TriggerAnimation()
    {
        anim.SetBool("moving", true);
        anim.SetBool("idle", false);
        anim.SetBool("loading", false);
    }
}
