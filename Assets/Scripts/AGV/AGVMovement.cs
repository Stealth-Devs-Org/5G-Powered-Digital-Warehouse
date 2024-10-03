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
    public float rotationSpeed = 0.5f;
    //WebSocketDataReceive webSocketDataReceive;
    
    public int ID = 100;  // not assigned yet...

    private bool isrenamed = false;

    

    




    // Start is called before the first frame update
    void Start()
    {
        AGVController = FindObjectOfType<AGVController>();
        //webSocketDataReceive = FindObjectOfType<WebSocketDataReceive>();
        Debug.Log("AGV ID: " + AGVController.agvID[0]);
        ID = AGVController.agvID[0];

        //gameObject.name = "AGV" + ID.ToString();
        StartCoroutine(AGVCoroutine());
    }

    // Update is called once per frame
    void Update()
    {   
        // if (isrenamed==false && ID<100)
        //     {
        //     gameObject.name = "AGV" + ID.ToString();
        //     isrenamed = true;
        //     }


        // //Debug.Log("AGV ID: " + ID);

        // if (ID<100) 
        // {
            

        //     //0: idle, 1: moving forward, 2: Loading, 3: Unloading, 4: Charging, 5:Turning Right, 6:Turning Left, 7:Turning Back, 8:Turning Completed,

        // if (AGVController.AGVPresent[ID-1] == true)
        // {
        //     if (AGVController.agvStatus[ID-1] == 0) 
        //     {
               
        //     }
        //     else if (AGVController.agvStatus[ID-1] == 1) // AGV moving forward
        //     {
        //         MoveAGVForwardBackward(AGVController.agvLocations[ID-1]);
        //     }
        //     else if (AGVController.agvStatus[ID-1] == 2) 
        //     {
                
        //     }
        //     else if (AGVController.agvStatus[ID-1] == 3) 
        //     {
                
        //     }
        //     else if (AGVController.agvStatus[ID-1] == 4) // Charging
        //     {
        //         // Handle charging case
        //     }
        //     else if (AGVController.agvStatus[ID-1] == 5) // Turning Right
        //     {
        //         TurnAGVRight();
                
        //     }
        //     else if (AGVController.agvStatus[ID-1] == 6) // Turning Left
        //     {
        //         TurnAGVLeft();
                
        //     }
        //     else if (AGVController.agvStatus[ID-1] == 7) // Turning Back
        //     {
        //         TurnAGVRight();
        //     }
        //     else if (AGVController.agvStatus[ID-1] == 8) // Turning Completed
        //     {
               
        //     }
        //     else if (AGVController.agvStatus[ID-1] == 9) // Any other stop condition
        //     {
        
        //     }
        //     else
        //     {
                
        //     }
        // }


  
        // }

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
        
        Vector3 targetDirection = Quaternion.Euler(0, -rotationSpeed * Time.deltaTime, 0) * transform.forward;

        
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, rotationSpeed * Time.deltaTime, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }

    public void TurnAGVRight()
    {
        
        Vector3 targetDirection = Quaternion.Euler(0, rotationSpeed * Time.deltaTime, 0) * transform.forward;

       
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, rotationSpeed * Time.deltaTime, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }





        public void LoadingUnloading(Vector3 targetPosition)
    {




    }




    private IEnumerator AGVCoroutine()
    {
        yield return new WaitForSeconds(1);
        
        if (isrenamed == false && ID < 100)
        {
            gameObject.name = "AGV" + ID.ToString();
            isrenamed = true;
        }

        // Debug.Log("AGV ID: " + ID);

        while (ID < 100)
        {
            // Check if the AGV is present
            if (AGVController.AGVPresent[ID - 1] == true)
            {
                // Handle the AGV status
                switch (AGVController.agvStatus[ID - 1])
                {
                    case 0:
                        // Idle state
                        break;
                    case 1: // AGV moving forward
                        MoveAGVForwardBackward(AGVController.agvLocations[ID - 1]);
                        break;
                    case 2:
                        // Loading state
                        break;
                    case 3:
                        // Unloading state
                        break;
                    case 4: // Charging
                        // Handle charging case
                        break;
                    case 5: // Turning Right
                        TurnAGVRight();
                        break;
                    case 6: // Turning Left
                        TurnAGVLeft();
                        break;
                    case 7: // Turning Back
                        TurnAGVLeft(); // Assuming you have a method for turning back
                        break;
                    case 8: // Turning Completed
                        // Handle completed turning
                        break;
                    case 9: // Any other stop condition
                        // Handle stop condition
                        break;
                    default:
                        // Handle unexpected status
                        break;
                }
            }

            // Wait for the next frame before checking again
            yield return null;
        }
    }



}


