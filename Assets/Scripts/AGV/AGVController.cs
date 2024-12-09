using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class AGVMessage
{
    public string agv_id;       // AGV ID
    public int[] location;      // Location [x, y]
    public List<int[]> segment; // Segment (list of points [[x1, y1], [x2, y2], ...])
    public int status;          // Status code
    public string timestamp;    // Timestamp
}

public class AGVController : MonoBehaviour
{
    WebSocketClient webSocketClient;
    public string agv1Message;

    public GameObject agvPrefab;      // Prefab of the AGV
    private GameObject agvObject;     // Instance of the AGV
    private Queue<Vector3> locationQueue = new Queue<Vector3>(2); // Stores the last two locations

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
        if (webSocketClient != null && webSocketClient.agv1Message != null && webSocketClient.newmessageArrvied)
        {
            webSocketClient.newmessageArrvied = false;
            agv1Message = webSocketClient.agv1Message;

            // Parse the JSON message
            AGVMessage message = JsonUtility.FromJson<AGVMessage>(agv1Message);

            Debug.Log($"AGV ID: {message.agv_id}");
            Debug.Log($"Location: X = {message.location[0]}, Y = {message.location[1]}");
            Debug.Log($"Status: {message.status}");
            Debug.Log($"Timestamp: {message.timestamp}");

            // Convert the location to a Vector3
            Vector3 newLocation = new Vector3(message.location[0], 0, message.location[1]);

            // Add the new location to the queue
            UpdateLocationQueue(newLocation);

            // Spawn the AGV object if it doesn't exist yet
            if (agvObject == null)
            {
                agvObject = Instantiate(agvPrefab, newLocation, Quaternion.identity);
            }
            else if (locationQueue.Count == 2)
            {
                // Move the AGV between the last two locations
                Vector3 previousLocation = locationQueue.ToArray()[0]; // Get the first location in the queue
                StartCoroutine(MoveAGV(previousLocation, newLocation));
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

    // Smoothly moves the AGV between two positions
    private IEnumerator MoveAGV(Vector3 startPos, Vector3 targetPos)
    {
        float speed = 29.0f; // Movement speed
        float distance = Vector3.Distance(startPos, targetPos);

        // Smoothly move the AGV from startPos to targetPos
        while (Vector3.Distance(agvObject.transform.position, targetPos) > 0.01f)
        {
            agvObject.transform.position = Vector3.MoveTowards(
                agvObject.transform.position,
                targetPos,
                speed * Time.deltaTime
            );
            yield return null;
        }

        // Ensure the AGV is at the target position
        agvObject.transform.position = targetPos;
    }
}
