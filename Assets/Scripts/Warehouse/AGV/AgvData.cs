using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgvData : MonoBehaviour
{
    public string object_agv_id;
    public int[] object_agv_location;
    public string object_agv_segment;
    public double object_agv_status;
    public int object_segment_timestamp;



    WebSocketClient webSocketClient;
    public class AgvMessage
    {
        public string agv_id;       // AGV ID
        public int[] location;      // Location [x, y]
        public int status;          // Status code

    }

    void Start()
    {
        webSocketClient = FindAnyObjectByType<WebSocketClient>();
        if (webSocketClient == null)
        {
            Debug.LogError("WebSocketClient not found in the scene!");
        }
    }

    void Update()
    {
        if (webSocketClient != null)
        {



            if (gameObject.name == "AGV1")
            {
                AgvMessage message = JsonUtility.FromJson<AgvMessage>(webSocketClient.agv1Message);
                object_agv_id = message.agv_id;
                object_agv_location = message.location;
                // object_agv_segment = message.segment.ToString();
                object_agv_status = message.status;
                // object_segment_timestamp = int.Parse(message.timestamp);
            }
            else if (gameObject.name == "AGV2")
            {
                AgvMessage message = JsonUtility.FromJson<AgvMessage>(webSocketClient.agv2Message);
                object_agv_id = message.agv_id;
                object_agv_location = message.location;
                // object_agv_segment = message.segment.ToString();
                object_agv_status = message.status;
                // object_segment_timestamp = int.Parse(message.timestamp);
            }
            else if (gameObject.name == "AGV3")
            {
                AgvMessage message = JsonUtility.FromJson<AgvMessage>(webSocketClient.agv3Message);
                object_agv_id = message.agv_id;
                object_agv_location = message.location;
                // object_agv_segment = message.segment.ToString();
                object_agv_status = message.status;
                // object_segment_timestamp = int.Parse(message.timestamp);
            }
            else if (gameObject.name == "AGV4")
            {
                AgvMessage message = JsonUtility.FromJson<AgvMessage>(webSocketClient.agv4Message);
                object_agv_id = message.agv_id;
                object_agv_location = message.location;
                // object_agv_segment = message.segment.ToString();
                object_agv_status = message.status;
                // object_segment_timestamp = int.Parse(message.timestamp);
            }
 
            
        }

    }





//     public void AssignJsonData(string agvMessage)
//     {
//         AGVMessage message = JsonUtility.FromJson<AGVMessage>(agvMessage);      
//         object_agv_id = message.agv_id;
//         object_agv_location = message.agv_location;
//         object_agv_segment = message.agv_segment;
//         object_agv_status = message.agv_status;
//         object_segment_timestamp = message.segment_timestamp;
//     }

    public string GetAgvData()
    {
        AgvMessage message = new AgvMessage
        {
            agv_id = object_agv_id,
            location = object_agv_location,
    
            status = (int)object_agv_status,
            // timestamp = object_segment_timestamp.ToString()
        };


        return JsonUtility.ToJson(message);
    }
}
