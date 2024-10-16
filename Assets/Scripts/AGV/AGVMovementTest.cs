using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;
using WebSocketSharp;

public class AGVMovementTest : MonoBehaviour
{
    
    GetLocationOfSphere getLocationOfSphere;
    public float speedofAGV = 1.5f;
    public float rotationSpeed = 150f;
    //WebSocketDataReceive webSocketDataReceive;
    
    public int ID = 100;  // not assigned yet...

    private bool isrenamed = false;
    private Animator anim;

    public string url;
    WebSocket ws;
    string DataReceived;
    private Vector2Int receivedCordinate;
    private Vector2Int AGV1Cordinate;
    public int AGV1Status;


    private int positionX;
    private int positionZ;

    // for turning
    private float currentRotationAngle = 0f; // Tracks the current rotation angle
    private bool isTurningLeft = false;      // Keeps track of whether AGV is turning left
    private bool isTurningRight = false;     // Keeps track of whether AGV is turning right
    private const float targetRotationAngle = 90f;  // The angle we want to rotate by

    public class DataObject   //own JSON
    {
        //public int[] agvIDs;
        public float[] location;
        public int segment;
        public int status;
        public string timestamp;
    }

    




    // Start is called before the first frame update
    void Start()
    {
        getLocationOfSphere = FindObjectOfType<GetLocationOfSphere>();

        anim = GetComponent<Animator>();



        ws = new WebSocket(url);
        ws.Connect();
        ws.OnMessage += (sender, e) =>
        {   
            DataReceived = e.Data;
            DataObject dataObject = JsonUtility.FromJson<DataObject>(DataReceived);
            //Debug.Log(DataReceived);
            receivedCordinate = new Vector2Int((int)dataObject.location[0], (int)dataObject.location[1]);
            
            positionZ = (2 * receivedCordinate.y) - 22;
            positionX = 49 - (2 * receivedCordinate.x - 6);
            AGV1Cordinate = new Vector2Int((int)dataObject.location[0], (int)dataObject.location[1]);
            // AGV1Cordinate = getLocationOfSphere.ReturnLocationCordinate(receivedCordinate);
            // AGV1Cordinate = new Vector2Int (positionX,positionZ);
            // Debug.Log(AGV1Cordinate);
            AGV1Status = dataObject.status;
            Debug.Log(AGV1Cordinate);
        };


        //spawn agv in the location
        
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


            

            //0: idle, 1: moving forward, 2: Loading, 3: Unloading, 4: Charging, 5:Turning Right, 6:Turning Left, 7:Turning Back, 8:Turning Completed,


        if (AGV1Status == 0) 
            {
                anim.SetBool("idle", true);
            }
        else if (AGV1Status == 1) // AGV moving forward
            {
                anim.SetBool("moving", true);
                MoveAGVForwardBackward(AGV1Cordinate);
            }

        // else if (AGV1Status == 2) 
        //     {
                
        //         anim.SetBool("Load&Unload1st", true);
                
        //     }
            // else if (AGVController.agvStatus[ID-1] == 3) 
            // {


                
            // }
            // else if (AGVController.agvStatus[ID-1] == 4) // Charging
            // {
            //     // Handle charging case
            // }



        // else if (AGV1Status == 5) // Turning Right
        //     {
        //         TurnAGVRight();
        //         anim.SetBool("turning", true);
                
        //     }
        // else if (AGV1Status == 6) // Turning Left
        //     {
        //         TurnAGVLeft();
        //         anim.SetBool("turning", true);
                
        //     }
        // else if (AGV1Status == 7) // Turning Back
        //     {
        //         TurnAGVRight();
        //         anim.SetBool("turning", true);

        //     }


        else
            {

                
            }
        

    }




    // public void MoveAGVForwardBackward(Vector2Int targetPosition)
    // {

        
    //     // if (angle > 1.0f)
    //     // {
    //     //     Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction, rotationSpeed * Time.deltaTime, 0.0f);
    //     //     transform.rotation = Quaternion.LookRotation(newDirection);
    //     // }
    //     // else
    //     // {
    //     //     transform.position = Vector3.MoveTowards(transform.position, new Vector3 (targetPosition.x,0,targetPosition.y), speedofAGV * Time.deltaTime);
    //     // }
    // }


    public void MoveAGVForwardBackward(Vector2Int targetPosition)
    {
        Vector3 currentPosition = new Vector3(transform.position.x, transform.position.y,transform.position.z); // Get current position of the AGV
        Vector3 targetPos = new Vector3(targetPosition.x,transform.position.y, targetPosition.y); // Convert the target position to Vector2

        // Move the AGV towards the target position
        transform.position = Vector3.MoveTowards(currentPosition, targetPos, Time.deltaTime * speedofAGV);
    }



// public void TurnAGVLeft()
// {
//     if (!isTurningLeft)
//     {
//         currentRotationAngle = 0f; // Reset angle tracking when starting to turn
//         isTurningLeft = true; // Mark as turning
//     }

//     if (currentRotationAngle < targetRotationAngle)
//     {
//         float step = rotationSpeed * Time.deltaTime;
//         float angle = Mathf.Min(step, targetRotationAngle - currentRotationAngle);  // Ensure we don't overshoot 90 degrees
        
//         transform.Rotate(Vector3.up, -angle);  // Rotate to the left (negative angle)

//         currentRotationAngle += angle;  // Accumulate rotated angle
//     }
//     else
//     {
//         isTurningLeft = false; // Turn completed
//     }
// }

// public void TurnAGVRight()
// {
//     if (!isTurningRight)
//     {
//         currentRotationAngle = 0f; // Reset angle tracking when starting to turn
//         isTurningRight = true; // Mark as turning
//     }

//     if (currentRotationAngle < targetRotationAngle)
//     {
//         float step = rotationSpeed * Time.deltaTime;
//         float angle = Mathf.Min(step, targetRotationAngle - currentRotationAngle);  // Ensure we don't overshoot 90 degrees
        
//         transform.Rotate(Vector3.up, angle);  // Rotate to the right (positive angle)

//         currentRotationAngle += angle;  // Accumulate rotated angle
//     }
//     else
//     {
//         isTurningRight = false; // Turn completed
//     }
// }





    public void LoadingUnloading(Vector3 targetPosition)
    {




    }



    // void SpawnAGVInLocation(Vector2Int location)
    // {
    //     // Convert Vector2Int to Vector3 for 3D space here y is Z actually all y are 0
    //     Vector3 spawnPosition = new Vector3(location.x, 0, location.y);
    //     // Spawn the AGV at the given location with no rotation (Quaternion.identity)
    //     Instantiate(gameobject, spawnPosition, Quaternion.identity);
    // }



}


