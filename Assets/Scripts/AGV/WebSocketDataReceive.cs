using WebSocketSharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebSocketDataReceive : MonoBehaviour
{
    public string url;
    WebSocket ws;
    string DataReceived;

    // Define classes to map the JSON structure
    [System.Serializable]
    public class DataObject
    {
        public int agv_id;
        public float[] location; // Location as an array of floats
        public int segment;
        public int status; // Status of AGV
        public string timestamp;
    }

    private void Start()
    {
        ws = new WebSocket(url);
        ws.Connect();
        ws.OnMessage += (sender, e) =>
        {
            DataReceived = e.Data;
            //Debug.Log("Raw Data Received: " + DataReceived);

            // Parse the JSON data
            DataObject dataObject = JsonUtility.FromJson<DataObject>(DataReceived);
            
            if (dataObject != null && dataObject.location != null)
            {
                //Debug.Log($"AGV ID: {dataObject.agv_id}");
                Debug.Log($"Location: x={dataObject.location[0]}, y={dataObject.location[1]}");
                //Debug.Log($"Segment: {dataObject.segment}, Status: {dataObject.status}, Timestamp: {dataObject.timestamp}");
            }
            else
            {
                Debug.LogWarning("Failed to parse AGV data.");
            }
        };
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Debug.Log("Space key was pressed.");
            ws.Send("Hello");
        }
    }
}
