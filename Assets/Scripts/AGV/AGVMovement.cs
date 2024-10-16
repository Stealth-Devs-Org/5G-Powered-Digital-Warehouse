using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;

public class AGVMovement : MonoBehaviour
{
    AGVController AGVController;
    
    public float speedofAGV = 1.5f;
    public float rotationSpeed = 150f;
    //WebSocketDataReceive webSocketDataReceive;
    
    public int ID = 100;  // not assigned yet...

    private bool isrenamed = false;

    private Animator anim;




    // for turning
    private float currentRotationAngle = 0f; // Tracks the current rotation angle
    private bool isTurningLeft = false;      // Keeps track of whether AGV is turning left
    private bool isTurningRight = false;     // Keeps track of whether AGV is turning right
    private const float targetRotationAngle = 90f;  // The angle we want to rotate by

    

    




    // Start is called before the first frame update
    void Start()
    {
        AGVController = FindObjectOfType<AGVController>();
        anim = GetComponent<Animator>();
        //webSocketDataReceive = FindObjectOfType<WebSocketDataReceive>();
        Debug.Log("AGV ID: " + AGVController.agvID[0]);
        ID = AGVController.agvID[0];

        //gameObject.name = "AGV" + ID.ToString();


        
    
    }

    // Update is called once per frame
    void Update()
    {  
        //Debug.Log(ID);
        if (isrenamed==false && ID<100)
            {
            gameObject.name = "AGV" + ID.ToString();
            isrenamed = true;
            }


        //Debug.Log("AGV ID: " + ID);

        if (ID<100 && ID>0) 
        {
            

            //0: idle, 1: moving forward, 2: Loading, 3: Unloading, 4: Charging, 5:Turning Right, 6:Turning Left, 7:Turning Back, 8:Turning Completed,

        if (AGVController.AGVPresent[ID-1] == true)
        {
            if (AGVController.agvStatus[ID-1] == 0) 
            {
                anim.SetBool("idle", true);
            }
            else if (AGVController.agvStatus[ID-1] == 1) // AGV moving forward
            {
                anim.SetBool("moving", true);
                MoveAGVForwardBackward(AGVController.agvLocations[ID-1]);
            }
            else if (AGVController.agvStatus[ID-1] == 2) 
            {
                
                anim.SetBool("Load&Unload1st", true);
                
            }
            // else if (AGVController.agvStatus[ID-1] == 3) 
            // {


                
            // }
            // else if (AGVController.agvStatus[ID-1] == 4) // Charging
            // {
            //     // Handle charging case
            // }
            else if (AGVController.agvStatus[ID-1] == 5) // Turning Right
            {
                TurnAGVRight();
                anim.SetBool("turning", true);
                
            }
            else if (AGVController.agvStatus[ID-1] == 6) // Turning Left
            {
                TurnAGVLeft();
                anim.SetBool("turning", true);
                
            }
            else if (AGVController.agvStatus[ID-1] == 7) // Turning Back
            {
                TurnAGVRight();
                anim.SetBool("turning", true);

                

            }
            // else if (AGVController.agvStatus[ID-1] == 8) // Turning Completed
            // {
               
            // }

            else
            {
                //anim.SetBool("idle", true);
                
            }
        }


  
        }


    }




    public void MoveAGVForwardBackward(Vector2Int targetPosition)
    {

        Vector3 direction = new Vector3 (targetPosition.x,0,targetPosition.y) - transform.position;
        float angle = Vector3.Angle(direction, transform.forward);
        float distance = direction.magnitude;

        if (angle > 1.0f)
        {
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction, rotationSpeed * Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3 (targetPosition.x,0,targetPosition.y), speedofAGV * Time.deltaTime);
        }
    }



public void TurnAGVLeft()
{
    if (!isTurningLeft)
    {
        currentRotationAngle = 0f; // Reset angle tracking when starting to turn
        isTurningLeft = true; // Mark as turning
    }

    if (currentRotationAngle < targetRotationAngle)
    {
        float step = rotationSpeed * Time.deltaTime;
        float angle = Mathf.Min(step, targetRotationAngle - currentRotationAngle);  // Ensure we don't overshoot 90 degrees
        
        transform.Rotate(Vector3.up, -angle);  // Rotate to the left (negative angle)

        currentRotationAngle += angle;  // Accumulate rotated angle
    }
    else
    {
        isTurningLeft = false; // Turn completed
    }
}

public void TurnAGVRight()
{
    if (!isTurningRight)
    {
        currentRotationAngle = 0f; // Reset angle tracking when starting to turn
        isTurningRight = true; // Mark as turning
    }

    if (currentRotationAngle < targetRotationAngle)
    {
        float step = rotationSpeed * Time.deltaTime;
        float angle = Mathf.Min(step, targetRotationAngle - currentRotationAngle);  // Ensure we don't overshoot 90 degrees
        
        transform.Rotate(Vector3.up, angle);  // Rotate to the right (positive angle)

        currentRotationAngle += angle;  // Accumulate rotated angle
    }
    else
    {
        isTurningRight = false; // Turn completed
    }
}





        public void LoadingUnloading(Vector3 targetPosition)
    {




    }



}


