using UnityEngine;

public class FreeCameraMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float movementSpeed = 10f;       
    public float lookSpeed = 2f;            
    public float scrollSpeedMultiplier = 20f; 
    public float minSpeed = 1f;           
    public float maxSpeed = 50f;           

    private float yaw = 0f;    
    private float pitch = 0f; 

    void Start()
    {
        
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        yaw += mouseX * lookSpeed;
        pitch -= mouseY * lookSpeed;
        pitch = Mathf.Clamp(pitch, -90f, 90f); 

        transform.eulerAngles = new Vector3(pitch, yaw, 0f);

     
        Vector3 moveDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) moveDirection += transform.forward;
        if (Input.GetKey(KeyCode.S)) moveDirection -= transform.forward;
        if (Input.GetKey(KeyCode.A)) moveDirection -= transform.right;
        if (Input.GetKey(KeyCode.D)) moveDirection += transform.right;

        moveDirection.Normalize();

   
        transform.position += moveDirection * movementSpeed * Time.deltaTime;

       
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f) 
        {
            movementSpeed += scroll * scrollSpeedMultiplier;
            movementSpeed = Mathf.Clamp(movementSpeed, minSpeed, maxSpeed); 
        }

        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
