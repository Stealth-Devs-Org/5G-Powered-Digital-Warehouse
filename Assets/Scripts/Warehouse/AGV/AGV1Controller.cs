
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class AGV1Controller : MonoBehaviour
// {
//     WebSocketClient webSocketClient;

//     public GameObject agvPrefab;      // Prefab of the AGV
//     private GameObject agvObject;     // Instance of the AGV
//     private Queue<Vector3> locationQueue = new Queue<Vector3>(2); // Stores the last two locations
//     private Vector3 targetPosition;    // Target position for the AGV
//     private bool isMoving;             // Indicates if the AGV is currently moving

//     public int currentStatus = 0;

//     private class AGVMessage
//     {
//         public string agv_id;       // AGV ID
//         public int[] location;      // Location [x, y]
//         public List<int[]> segment; // Segment (list of points [[x1, y1], [x2, y2], ...])
//         public int status;          // Status code
//         public string timestamp;    // Timestamp
//     }

//     private string agv1Message;

//     void Start()
//     {
//         webSocketClient = FindObjectOfType<WebSocketClient>();

//         if (webSocketClient == null)
//         {
//             Debug.LogError("WebSocketClient not found in the scene!");
//         }
//     }

//     void Update()
//     {
//         if (webSocketClient != null && webSocketClient.agv1Message != null && webSocketClient.newmessageArrviedAGV1)
//         {
//             webSocketClient.newmessageArrviedAGV1 = false;
//             agv1Message = webSocketClient.agv1Message;

//             // Parse the JSON message
//             AGVMessage message = JsonUtility.FromJson<AGVMessage>(agv1Message);

//             currentStatus = message.status;

//             // Apply the reverse equations to get the Unity world coordinates
//             int positionZ = (int)(2 * message.location[0] - 22 + 2);
//             int positionX = (int)(55 - 2 * message.location[1]);
//             Vector3 newLocation = new Vector3(positionX, 0, positionZ);

//             //Debug.Log($"[DEBUG] New Target Location: {newLocation}");

//             // Add the new location to the queue
//             UpdateLocationQueue(newLocation);

//             // Spawn the AGV object if it doesn't exist yet
//             if (agvObject == null)
//             {
//                 Debug.Log("[DEBUG] Instantiating AGV for the first time.");
//                 agvObject = Instantiate(agvPrefab, newLocation, Quaternion.identity);
//                 agvObject.name = "AGV1";
//                 targetPosition = newLocation; // Set initial target position
//                 isMoving = true; // Start moving
//                 StartCoroutine(MoveAGV(targetPosition));
//             }
//             else if (locationQueue.Count == 2 && !isMoving)
//             {
//                 //Debug.Log("[DEBUG] Updating Target Position.");
//                 targetPosition = newLocation; // Update the target position
//                 StopCoroutine("MoveAGV"); // Stop any ongoing movement
//                 StartCoroutine(MoveAGV(targetPosition)); // Start moving to the new location
//             }
//         }
//     }

//     // Updates the queue with the last two locations
//     private void UpdateLocationQueue(Vector3 newLocation)
//     {
//         if (locationQueue.Count == 2)
//         {
//             locationQueue.Dequeue(); // Remove the oldest location
//         }
//         locationQueue.Enqueue(newLocation); // Add the new location
//     }

//     // Smoothly moves the AGV towards the target position
//     private IEnumerator MoveAGV(Vector3 targetPos)
//     {
//         isMoving = true; // Mark as moving
//         float minSpeed = 1f; // Minimum speed
//         float maxSpeed = 12.0f; // Maximum speed
//         float maxDistance = 15.0f; // Distance at which max speed is reached
//         float stopThreshold = 0.1f; // Threshold for stopping

//         while (Vector3.Distance(agvObject.transform.position, targetPos) > stopThreshold)
//         {
//             // Calculate distance from the current position to the target position
//             float distance = Vector3.Distance(agvObject.transform.position, targetPos);
//             //Debug.Log($"[DEBUG] Distance to Target: {distance}");

//             // Map distance to speed
//             float speed = Mathf.Lerp(minSpeed, maxSpeed, Mathf.Clamp01(distance / maxDistance));
//             //Debug.Log($"[DEBUG] Speed: {speed}");

//             // Determine the next position
//             Vector3 nextPosition = Vector3.MoveTowards(
//                 agvObject.transform.position,
//                 targetPos,
//                 speed * Time.deltaTime
//             );

//             // Calculate the direction to the next position
//             Vector3 direction = nextPosition - agvObject.transform.position;

//             Debug.Log($"[DEBUG] Direction: {direction}");

//             if (direction != Vector3.zero) // Ensure direction is valid
//             {
//                 // Smoothly rotate the AGV to face the moving direction
//                 Quaternion targetRotation = Quaternion.LookRotation(direction);
//                 agvObject.transform.rotation = Quaternion.Slerp(
//                     agvObject.transform.rotation,
//                     targetRotation,
//                     Time.deltaTime * 50.0f // Adjust rotation speed as needed
//                 );

//                 Debug.Log($"[DEBUG] Current Rotation: {agvObject.transform.rotation.eulerAngles}");
//             }

//             // Move the AGV towards the target position
//             agvObject.transform.position = nextPosition;

//             //Debug.Log($"[DEBUG] Current Position: {agvObject.transform.position}");

//             yield return null; // Wait for the next frame
//         }

//         // Ensure the AGV is at the target position
//         agvObject.transform.position = targetPos;
//         //Debug.Log($"[DEBUG] Reached Target Position: {targetPos}");

//         isMoving = false; // Mark as not moving
//     }
// }











































































































using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AGV1Controller : MonoBehaviour
{
    WebSocketClient webSocketClient;
    

    public GameObject agvPrefab;      // Prefab of the AGV
    private GameObject agvObject;     // Instance of the AGV
    private Queue<Vector3> locationQueue = new Queue<Vector3>(2); // Stores the last two locations
    private Vector3 targetPosition;    // Target position for the AGV
    private bool isMoving;             // Indicates if the AGV is currently moving

    public int currentStatus = 0;

    AgvData agvData;

        
    private class AGVMessage
    {
        public string agv_id;       // AGV ID
        public int[] location;      // Location [x, y]
        public List<int[]> segment; // Segment (list of points [[x1, y1], [x2, y2], ...])
        public int status;          // Status code
        public string timestamp;    // Timestamp
    }

    
    private string agv1Message;


    void Start()
    {
        webSocketClient = FindObjectOfType<WebSocketClient>();

        agvData = FindObjectOfType<AgvData>();


        if (webSocketClient == null)
        {
            Debug.LogError("WebSocketClient not found in the scene!");
        }
    }

    void Update()
    {
        if (webSocketClient != null && webSocketClient.agv1Message != null && webSocketClient.newmessageArrviedAGV1)
        {
            webSocketClient.newmessageArrviedAGV1 = false;
            agv1Message = webSocketClient.agv1Message;

            // Parse the JSON message
            AGVMessage message = JsonUtility.FromJson<AGVMessage>(agv1Message);

            //Debug.Log($"AGV1 ID: {message.agv_id}");
            //Debug.Log($"Location AGV1: X = {message.location[0]}, Y = {message.location[1]}");
            //Debug.Log($"Status AGV1: {message.status}");
            currentStatus = message.status;
            //Debug.Log($"Timestamp AGV1: {message.timestamp}");

            // Apply the reverse equations to get the Unity world coordinates
            int positionZ = (int)(2 * message.location[0] - 22 + 2);
            int positionX = (int)(55 - 2 * message.location[1]);
            Vector3 newLocation = new Vector3(positionX, 0, positionZ);

            // Add the new location to the queue
            UpdateLocationQueue(newLocation);

            // Spawn the AGV object if it doesn't exist yet
            if (agvObject == null)
            {
                agvObject = Instantiate(agvPrefab, newLocation, Quaternion.identity);
                agvObject.name = "AGV1";
                targetPosition = newLocation; // Set initial target position
                isMoving = true; // Start moving
                StartCoroutine(MoveAGV(targetPosition));
            }
            else if (locationQueue.Count == 2 && !isMoving)
            {
                targetPosition = newLocation; // Update the target position
                StopCoroutine("MoveAGV"); // Stop any ongoing movement
                StartCoroutine(MoveAGV(targetPosition)); // Start moving to the new location
            }
        }
    }

    // Updates the queue with the last two locations
    private void UpdateLocationQueue(Vector3 newLocation)
    {
        if (locationQueue.Count == 2)
        {
            locationQueue.Dequeue(); // Remove the oldest location
        }
        locationQueue.Enqueue(newLocation); // Add the new location
    }

    // Smoothly moves the AGV towards the target position
    private IEnumerator MoveAGV(Vector3 targetPos)
    {
        isMoving = true; // Mark as moving
        float minSpeed = 1f; // Minimum speed
        float maxSpeed = 12.0f; // Maximum speed
        float maxDistance = 15.0f; // Distance at which max speed is reached
        float stopThreshold = 0.1f; // Threshold for stopping

        while (Vector3.Distance(agvObject.transform.position, targetPos) > stopThreshold)
        {
            // Calculate distance from the current position to the target position
            float distance = Vector3.Distance(agvObject.transform.position, targetPos);

            // Map distance to speed
            float speed = Mathf.Lerp(minSpeed, maxSpeed, Mathf.Clamp01(distance / maxDistance));

            // Determine the next position
            Vector3 nextPosition = Vector3.MoveTowards(
                agvObject.transform.position,
                targetPos,
                speed * Time.deltaTime
            );

            // Calculate the direction to the next position
            Vector3 direction = nextPosition - agvObject.transform.position;

            if (direction != Vector3.zero) // Ensure direction is valid
            {
                // Smoothly rotate the AGV to face the moving direction
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                agvObject.transform.rotation = Quaternion.Slerp(
                    agvObject.transform.rotation,
                    targetRotation,
                    Time.deltaTime * 7.0f // Adjust rotation speed as needed
                );
            }

            // Move the AGV towards the target position
            agvObject.transform.position = nextPosition;

            yield return null; // Wait for the next frame
        }

        // Ensure the AGV is at the target position
        agvObject.transform.position = targetPos;

        isMoving = false; // Mark as not moving
    }

}

