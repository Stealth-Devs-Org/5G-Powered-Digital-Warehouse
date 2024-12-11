// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using System;

// [Serializable]
// public class AGVMessage
// {
//     public string agv_id;       // AGV ID
//     public int[] location;      // Location [x, y]
//     public List<int[]> segment; // Segment (list of points [[x1, y1], [x2, y2], ...])
//     public int status;          // Status code
//     public string timestamp;    // Timestamp
// }

// public class AGV0Controller : MonoBehaviour
// {
//     WebSocketClient webSocketClient;
//     public string agv1Message;

//     public GameObject agvPrefab;      // Prefab of the AGV
//     private GameObject agvObject;     // Instance of the AGV
//     private Queue<Vector3> locationQueue = new Queue<Vector3>(2); // Stores the last two locations

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
//         if (webSocketClient != null && webSocketClient.agv1Message != null && webSocketClient.newmessageArrvied)
//         {
//             webSocketClient.newmessageArrvied = false;
//             agv1Message = webSocketClient.agv1Message;

//             // Parse the JSON message
//             AGVMessage message = JsonUtility.FromJson<AGVMessage>(agv1Message);

//             Debug.Log($"AGV ID: {message.agv_id}");
//             Debug.Log($"Location: X = {message.location[0]}, Y = {message.location[1]}");
//             Debug.Log($"Status: {message.status}");
//             Debug.Log($"Timestamp: {message.timestamp}");

//             // Convert the location to a Vector3
//             Vector3 newLocation = new Vector3(message.location[0], 0, message.location[1]);

//             // Add the new location to the queue
//             UpdateLocationQueue(newLocation);

//             // Spawn the AGV object if it doesn't exist yet
//             if (agvObject == null)
//             {
//                 agvObject = Instantiate(agvPrefab, newLocation, Quaternion.identity);
//             }
//             else if (locationQueue.Count == 2)
//             {
//                 // Move the AGV between the last two locations
//                 Vector3 previousLocation = locationQueue.ToArray()[0]; // Get the first location in the queue
//                 StartCoroutine(MoveAGV(previousLocation, newLocation));
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

//     // Smoothly moves the AGV between two positions
//     private IEnumerator MoveAGV(Vector3 startPos, Vector3 targetPos)
//     {
//         float speed = 29.0f; // Movement speed
//         float distance = Vector3.Distance(startPos, targetPos);

//         // Smoothly move the AGV from startPos to targetPos
//         while (Vector3.Distance(agvObject.transform.position, targetPos) > 0.01f)
//         {
//             agvObject.transform.position = Vector3.MoveTowards(
//                 agvObject.transform.position,
//                 targetPos,
//                 speed * Time.deltaTime
//             );
//             yield return null;
//         }

//         // Ensure the AGV is at the target position
//         agvObject.transform.position = targetPos;
//     }
// }






// // // AGV cordinate mapped to unity world cordinate


// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using System;

// [Serializable]
// public class AGVMessage
// {
//     public string agv_id;       // AGV ID
//     public int[] location;      // Location [x, y]
//     public List<int[]> segment; // Segment (list of points [[x1, y1], [x2, y2], ...])
//     public int status;          // Status code
//     public string timestamp;    // Timestamp
// }

// public class AGV0Controller : MonoBehaviour
// {
//     WebSocketClient webSocketClient;
//     public string agv1Message;

//     public GameObject agvPrefab;      // Prefab of the AGV
//     private GameObject agvObject;     // Instance of the AGV
//     private Queue<Vector3> locationQueue = new Queue<Vector3>(2); // Stores the last two locations

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
//         if (webSocketClient != null && webSocketClient.agv1Message != null && webSocketClient.newmessageArrvied)
//         {
//             webSocketClient.newmessageArrvied = false;
//             agv1Message = webSocketClient.agv1Message;

//             // Parse the JSON message
//             AGVMessage message = JsonUtility.FromJson<AGVMessage>(agv1Message);

//             Debug.Log($"AGV ID: {message.agv_id}");
//             Debug.Log($"Location: X = {message.location[0]}, Y = {message.location[1]}");
//             Debug.Log($"Status: {message.status}");
//             Debug.Log($"Timestamp: {message.timestamp}");

//             // Apply the reverse equations to get the Unity world coordinates
//             float positionZ = 2 * message.location[0] - 22+2;
//             float positionX = 55 - 2 * message.location[1];
//             Vector3 newLocation = new Vector3(positionX, 0, positionZ);

//             // Add the new location to the queue
//             UpdateLocationQueue(newLocation);

//             // Spawn the AGV object if it doesn't exist yet
//             if (agvObject == null)
//             {
//                 agvObject = Instantiate(agvPrefab, newLocation, Quaternion.identity);
//             }
//             else if (locationQueue.Count == 2)
//             {
//                 // Move the AGV between the last two locations
//                 Vector3 previousLocation = locationQueue.ToArray()[0]; // Get the first location in the queue
//                 StartCoroutine(MoveAGV(previousLocation, newLocation));
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

//     // Smoothly moves the AGV between two positions
//     private IEnumerator MoveAGV(Vector3 startPos, Vector3 targetPos)
//     {
//         float speed = 2.0f; // Movement speed

//         // Smoothly move the AGV from startPos to targetPos
//         while (Vector3.Distance(agvObject.transform.position, targetPos) > 0.01f)
//         {
//             agvObject.transform.position = Vector3.MoveTowards(
//                 agvObject.transform.position,
//                 targetPos,
//                 speed * Time.deltaTime
//             );
//             yield return null;
//         }

//         // Ensure the AGV is at the target position
//         agvObject.transform.position = targetPos;
//     }
// }









// // // AGV dynamic distance speed movment


// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using System;

// [Serializable]
// public class AGVMessage
// {
//     public string agv_id;       // AGV ID
//     public int[] location;      // Location [x, y]
//     public List<int[]> segment; // Segment (list of points [[x1, y1], [x2, y2], ...])
//     public int status;          // Status code
//     public string timestamp;    // Timestamp
// }

// public class AGV0Controller : MonoBehaviour
// {
//     WebSocketClient webSocketClient;
//     public string agv1Message;

//     public GameObject agvPrefab;      // Prefab of the AGV
//     private GameObject agvObject;     // Instance of the AGV
//     private Queue<Vector3> locationQueue = new Queue<Vector3>(2); // Stores the last two locations

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
//         if (webSocketClient != null && webSocketClient.agv1Message != null && webSocketClient.newmessageArrvied)
//         {
//             webSocketClient.newmessageArrvied = false;
//             agv1Message = webSocketClient.agv1Message;

//             // Parse the JSON message
//             AGVMessage message = JsonUtility.FromJson<AGVMessage>(agv1Message);

//             Debug.Log($"AGV ID: {message.agv_id}");
//             Debug.Log($"Location: X = {message.location[0]}, Y = {message.location[1]}");
//             Debug.Log($"Status: {message.status}");
//             Debug.Log($"Timestamp: {message.timestamp}");

//             // Apply the reverse equations to get the Unity world coordinates
//             float positionZ = 2 * message.location[0] - 22+2;
//             float positionX = 55 - 2 * message.location[1];
//             Vector3 newLocation = new Vector3(positionX, 0, positionZ);

//             // Add the new location to the queue
//             UpdateLocationQueue(newLocation);

//             // Spawn the AGV object if it doesn't exist yet
//             if (agvObject == null)
//             {
//                 agvObject = Instantiate(agvPrefab, newLocation, Quaternion.identity);
//             }
//             else if (locationQueue.Count == 2)
//             {
//                 // Move the AGV between the last two locations
//                 Vector3 previousLocation = locationQueue.ToArray()[0]; // Get the first location in the queue
//                 StartCoroutine(MoveAGV(previousLocation, newLocation));
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

//     // Smoothly moves the AGV between two positions
//     private IEnumerator MoveAGV(Vector3 startPos, Vector3 targetPos)
//     {
//         // Constants for speed range
//         float minSpeed = 1.0f; // Minimum speed
//         float maxSpeed = 15.0f; // Maximum speed
//         float maxDistance = 6.0f; // Distance at which max speed is reached

//         // Calculate distance
//         float distance = Vector3.Distance(startPos, targetPos);

//         // Map distance to speed
//         float speed = Mathf.Lerp(minSpeed, maxSpeed, Mathf.Clamp01(distance / maxDistance));

//         // Smoothly move the AGV from startPos to targetPos
//         while (Vector3.Distance(agvObject.transform.position, targetPos) > 0.01f)
//         {
//             agvObject.transform.position = Vector3.MoveTowards(
//                 agvObject.transform.position,
//                 targetPos,
//                 speed * Time.deltaTime
//             );
//             yield return null;
//         }

//         // Ensure the AGV is at the target position
//         agvObject.transform.position = targetPos;
//     }


// }









// // // AGV dynamic distance speed movment


// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using System;

// [Serializable]
// public class AGVMessage
// {
//     public string agv_id;       // AGV ID
//     public int[] location;      // Location [x, y]
//     public List<int[]> segment; // Segment (list of points [[x1, y1], [x2, y2], ...])
//     public int status;          // Status code
//     public string timestamp;    // Timestamp
// }

// public class AGV0Controller : MonoBehaviour
// {
//     WebSocketClient webSocketClient;
//     public string agv1Message;

//     public GameObject agvPrefab;      // Prefab of the AGV
//     private GameObject agvObject;     // Instance of the AGV
//     private Queue<Vector3> locationQueue = new Queue<Vector3>(2); // Stores the last two locations

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
//         if (webSocketClient != null && webSocketClient.agv1Message != null && webSocketClient.newmessageArrvied)
//         {
//             webSocketClient.newmessageArrvied = false;
//             agv1Message = webSocketClient.agv1Message;

//             // Parse the JSON message
//             AGVMessage message = JsonUtility.FromJson<AGVMessage>(agv1Message);

//             Debug.Log($"AGV ID: {message.agv_id}");
//             Debug.Log($"Location: X = {message.location[0]}, Y = {message.location[1]}");
//             Debug.Log($"Status: {message.status}");
//             Debug.Log($"Timestamp: {message.timestamp}");

//             // Apply the reverse equations to get the Unity world coordinates
//             int positionZ = (int)(2 * message.location[0] - 22 + 2);
//             int positionX = (int)(55 - 2 * message.location[1]);
//             Vector3 newLocation = new Vector3(positionX, 0, positionZ);

//             // Add the new location to the queue
//             UpdateLocationQueue(newLocation);

//             // Spawn the AGV object if it doesn't exist yet
//             if (agvObject == null)
//             {
//                 agvObject = Instantiate(agvPrefab, newLocation, Quaternion.identity);
//             }
//             else if (locationQueue.Count == 2)
//             {
//                 Vector3 previousLocation = locationQueue.ToArray()[0]; // Get the first location in the queue
//                 StopCoroutine("MoveAGV"); // Stop any ongoing movement
//                 StartCoroutine(MoveAGV(newLocation)); // Start moving to the new location
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
//         float minSpeed = 0.5f; // Minimum speed
//         float maxSpeed = 20.0f; // Maximum speed
//         float maxDistance = 10.0f; // Distance at which max speed is reached

//         while (true) // Continuous loop
//         {
//             // Calculate distance from the current position to the target position
//             float distance = Vector3.Distance(agvObject.transform.position, targetPos);

//             // Map distance to speed
//             float speed = Mathf.Lerp(minSpeed, maxSpeed, Mathf.Clamp01(distance / maxDistance));

//             // Move the AGV towards the target position
//             agvObject.transform.position = Vector3.MoveTowards(
//                 agvObject.transform.position,
//                 targetPos,
//                 speed * Time.deltaTime
//             );

//             // Check if the AGV is close enough to the target position to consider it reached
//             if (distance <= 0.01f)
//             {
//                 // Break the loop if the target is reached
//                 break;
//             }

//             yield return null; // Wait for the next frame
//         }

//         // Ensure the AGV is at the target position
//         agvObject.transform.position = targetPos;
//     }
// }










// // AGV dynamic distance speed movment Interpolation      dont use this code new mewssage arived issue

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AGV0Controller : MonoBehaviour
{
    WebSocketClient webSocketClient;
    

    public GameObject agvPrefab;      // Prefab of the AGV
    private GameObject agvObject;     // Instance of the AGV
    private Queue<Vector3> locationQueue = new Queue<Vector3>(2); // Stores the last two locations
    private Vector3 targetPosition;    // Target position for the AGV
    private bool isMoving;             // Indicates if the AGV is currently moving

        
    private class AGVMessage
    {
        public string agv_id;       // AGV ID
        public int[] location;      // Location [x, y]
        public List<int[]> segment; // Segment (list of points [[x1, y1], [x2, y2], ...])
        public int status;          // Status code
        public string timestamp;    // Timestamp
    }

    private string agv0Message;


    void Start()
    {
        webSocketClient = FindObjectOfType<WebSocketClient>();


        if (webSocketClient == null)
        {
            Debug.LogError("WebSocketClient not found in the scene!");
        }
    }

    void Update()
    {
        if (webSocketClient != null && webSocketClient.agv0Message != null && webSocketClient.newmessageArrviedAGV1)  // dont use this code
        {
            webSocketClient.newmessageArrvied = false;
            agv0Message = webSocketClient.agv0Message;

            // Parse the JSON message
            AGVMessage message = JsonUtility.FromJson<AGVMessage>(agv0Message);

            Debug.Log($"AGV0 ID: {message.agv_id}");
            Debug.Log($"Location AGV0: X = {message.location[0]}, Y = {message.location[1]}");
            //Debug.Log($"Status AGV0: {message.status}");
            //Debug.Log($"Timestamp AGV0: {message.timestamp}");

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
                agvObject.name = "AGV0";
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

            // Move the AGV towards the target position using interpolation for smoother movement
            agvObject.transform.position = Vector3.MoveTowards(
                agvObject.transform.position,
                targetPos,
                speed * Time.deltaTime
            );

            yield return null; // Wait for the next frame
        }

        // Ensure the AGV is at the target position
        agvObject.transform.position = targetPos;
        isMoving = false; // Mark as not moving
    }
}
