using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SensorControllerTemperature : MonoBehaviour
{
    WebSocketClientSensor webSocketClientSensor;
    

    public GameObject TempSensorPrefab;     
    private GameObject agvObject;     
    private Queue<Vector3> locationQueue = new Queue<Vector3>(2); // Stores the last two locations
    private Vector3 targetPosition;    // Target position for the AGV
    // private bool isMoving;             // Indicates if the AGV is currently moving

        
    public class SensorMessage
    {
        public string sensor_type;      
        public string sensor_id;       
        public int partition_id;
        public int[] sensor_location;      // Location [x, y]
        public double reading;      // Reading
        public int status;          // Status code
 
    }

    private string sensorMessage;


    void Start()
    {
        webSocketClientSensor = FindObjectOfType<WebSocketClientSensor>();


        if (webSocketClientSensor == null)
        {
            Debug.LogError("WebSocketClient not found in the scene!");
        }
    }

    void Update()
    {
        if (webSocketClientSensor != null && webSocketClientSensor.sensorMessage != null && webSocketClientSensor.newmessageArrviedforSensorTemp)
        {
            webSocketClientSensor.newmessageArrviedforSensorTemp = false;
            sensorMessage = webSocketClientSensor.sensorMessage;

            // Parse the JSON message
            SensorMessage message = JsonUtility.FromJson<SensorMessage>(sensorMessage);

            Debug.Log($"Sensor Type: {message.sensor_type}");
            Debug.Log($"Sensor ID: {message.sensor_id}");
            Debug.Log($"Partition ID: {message.partition_id}");
            Debug.Log($"Location Sensor: X = {message.sensor_location[0]}, Y = {message.sensor_location[1]}");
            
            // Apply the reverse equations to get the Unity world coordinates
            int positionZ = (int)(2 * message.sensor_location[0] - 22 + 2);
            int positionX = (int)(55 - 2 * message.sensor_location[1]);
            Vector3 newLocation = new Vector3(positionX, 0, positionZ);

            Debug.Log($"Reading: {message.reading}");
            Debug.Log($"Status: {message.status}");


            // Add the new location to the queue
            UpdateLocationQueue(newLocation);

            // Spawn the AGV object if it doesn't exist yet
            if (agvObject == null)
            {
                agvObject = Instantiate(TempSensorPrefab, newLocation, Quaternion.identity);
                agvObject.name = "AGV1";
                targetPosition = newLocation; // Set initial target position
                // isMoving = true; // Start moving
                StartCoroutine(MoveAGV(targetPosition));
            }
            else if (locationQueue.Count == 2)
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
        // isMoving = true; // Mark as moving
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
        // isMoving = false; // Mark as not moving
    }
}
