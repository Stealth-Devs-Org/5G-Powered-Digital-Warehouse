using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Vector3 targetPosition; 
    public float moveSpeed = 2f;     
    
    void Update()
    {
        if (targetPosition != null)
        {
            
            transform.position = Vector3.MoveTowards(
                transform.position,      
                targetPosition,
                moveSpeed * Time.deltaTime 
            );
        }

        
    }
}
