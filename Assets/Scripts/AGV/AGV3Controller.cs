using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AGV3Controller : MonoBehaviour
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

    private string agv3Message;


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
        if (webSocketClient != null && webSocketClient.agv3Message != null && webSocketClient.newmessageArrviedAGV3)
        {
            webSocketClient.newmessageArrviedAGV3 = false;
            agv3Message = webSocketClient.agv3Message;

            // Parse the JSON message
            AGVMessage message = JsonUtility.FromJson<AGVMessage>(agv3Message);

            Debug.Log($"AGV3 ID: {message.agv_id}");
            Debug.Log($"Location AGV3: X = {message.location[0]}, Y = {message.location[1]}");
            //Debug.Log($"Status AGV3: {message.status}");
            //Debug.Log($"Timestamp AGV3: {message.timestamp}");

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
                agvObject.name = "AGV3";
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
