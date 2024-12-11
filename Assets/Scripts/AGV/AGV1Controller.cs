using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AGV1Controller : MonoBehaviour
{
    WebSocketClient webSocketClient;
    public string agv1Message;
    public GameObject agvPrefab;      
    private GameObject agvObject;     
    private Queue<Vector3> locationQueue = new Queue<Vector3>(2); 
    private Vector3 targetPosition;    
    private bool isMoving;             
  
    private class AGVMessage
    {
        public string agv_id;       
        public int[] location;      
        public List<int[]> segment; // Segment (list of points [[x1, y1], [x2, y2], ...])
        public int status;          
        public string timestamp;    
    }

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

            AGVMessage message = JsonUtility.FromJson<AGVMessage>(agv1Message);

            Debug.Log($"AGV1 ID: {message.agv_id}");
            Debug.Log($"Location AGV1: X = {message.location[0]}, Y = {message.location[1]}");
            Debug.Log($"Status AGV1: {message.status}");
            Debug.Log($"Timestamp AGV1: {message.timestamp}");

            int positionZ = (int)(2 * message.location[0] - 22 + 2);
            int positionX = (int)(55 - 2 * message.location[1]);
            Vector3 newLocation = new Vector3(positionX, 0, positionZ);

            UpdateLocationQueue(newLocation);

            if (agvObject == null)
            {
                agvObject = Instantiate(agvPrefab, newLocation, Quaternion.identity);
                agvObject.name = "AGV1";
                targetPosition = newLocation;
                isMoving = true; 
                StartCoroutine(MoveAGV(targetPosition));
            }
            else if (locationQueue.Count == 2 && !isMoving)
            {
                targetPosition = newLocation;
                StopCoroutine("MoveAGV"); 
                StartCoroutine(MoveAGV(targetPosition)); 
            }
        }
    }

    private void UpdateLocationQueue(Vector3 newLocation)
    {
        if (locationQueue.Count == 2)
        {
            locationQueue.Dequeue();
        }
        locationQueue.Enqueue(newLocation); 
    }

    private IEnumerator MoveAGV(Vector3 targetPos)
    {
        isMoving = true;
        float minSpeed = 1f; 
        float maxSpeed = 12.0f; 
        float maxDistance = 15.0f; 
        float stopThreshold = 0.1f; 

        while (Vector3.Distance(agvObject.transform.position, targetPos) > stopThreshold)
        {
            float distance = Vector3.Distance(agvObject.transform.position, targetPos);
            float speed = Mathf.Lerp(minSpeed, maxSpeed, Mathf.Clamp01(distance / maxDistance));

            agvObject.transform.position = Vector3.MoveTowards(
                agvObject.transform.position,
                targetPos,
                speed * Time.deltaTime
            );

            yield return null;
        }
        agvObject.transform.position = targetPos;
        isMoving = false; 
    }
}
