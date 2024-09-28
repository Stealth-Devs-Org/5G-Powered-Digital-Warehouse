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
    public class Location
    {
        public float x;
        public float y;
        public float z;
    }

    [System.Serializable]
    public class DataObject
    {
        public Location location;
        public string otherField; // In case you have other fields you need
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
                Debug.Log(dataObject.location);
            }
            else
            {
                Debug.LogWarning("Failed to parse location data.");
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
