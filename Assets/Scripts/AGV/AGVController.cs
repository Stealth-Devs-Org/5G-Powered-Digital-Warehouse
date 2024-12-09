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
            
        }
    }
}
