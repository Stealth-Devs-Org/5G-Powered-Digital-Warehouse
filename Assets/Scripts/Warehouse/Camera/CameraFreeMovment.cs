using UnityEngine;

public class FreeCameraMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float movementSpeed = 10f;        // Base movement speed
    public float lookSpeed = 2f;             // Mouse look sensitivity
    public float scrollSpeedMultiplier = 2f; // Speed change multiplier for scroll wheel
    public float minSpeed = 1f;              // Minimum movement speed
    public float maxSpeed = 50f;             // Maximum movement speed

    [Header("Fixed Position Settings")]
    public Vector3 fixedPosition = new Vector3(0, 5, 0); // Fixed camera position
    public Vector3 fixedRotation = new Vector3(0, 0, 0); // Fixed camera rotation (Euler angles)

    private float yaw = 0f;    // Rotation around the y-axis
    private float pitch = 0f;  // Rotation around the x-axis
    private bool isFixed = false; // Flag to toggle fixed position

    void Start()
    {
        // Lock the cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Toggle between free movement and fixed position when Space is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isFixed = !isFixed;

            if (isFixed)
            {
                // Switch to fixed position
                transform.position = fixedPosition;
                transform.eulerAngles = fixedRotation;
            }
        }

        if (!isFixed)
        {
            HandleFreeMovement();
        }

        // Unlock cursor on Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void HandleFreeMovement()
    {
        // --- Camera Rotation ---
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        yaw += mouseX * lookSpeed;
        pitch -= mouseY * lookSpeed;
        pitch = Mathf.Clamp(pitch, -90f, 90f); // Prevent flipping

        transform.eulerAngles = new Vector3(pitch, yaw, 0f);

        // --- Camera Movement ---
        Vector3 moveDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) moveDirection += transform.forward;
        if (Input.GetKey(KeyCode.S)) moveDirection -= transform.forward;
        if (Input.GetKey(KeyCode.A)) moveDirection -= transform.right;
        if (Input.GetKey(KeyCode.D)) moveDirection += transform.right;

        // Normalize the movement direction
        moveDirection.Normalize();

        // Apply movement
        transform.position += moveDirection * movementSpeed * Time.deltaTime;

        // --- Adjust Speed with Scroll Wheel ---
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f) // Ensure there's meaningful scroll input
        {
            movementSpeed += scroll * scrollSpeedMultiplier;
            movementSpeed = Mathf.Clamp(movementSpeed, minSpeed, maxSpeed); // Keep speed in bounds
        }
    }
}
