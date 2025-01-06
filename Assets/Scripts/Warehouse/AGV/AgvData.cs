using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class AgvData : MonoBehaviour
{

    public string object_agv_id;
    public string object_agv_location;
    public string object_agv_segment;
    public double object_agv_status;
    public int object_segment_timestamp;

        
    string agvMessage;

    

    public class AgvMessage
    {
        public string agv_id;      
        public string agv_location;       
        public string agv_segment;
        public double agv_status;      
        public int segment_timestamp;         
 
    }

    AgvData agvData;
    WebSocketClient webSocketClient;
    void Start()
    {
        
        webSocketClient = FindObjectOfType<WebSocketClient>();
        
    }

    // Update is called once per frame
    void Update()
    {

        
        string message1 = webSocketClient.agv0Message;
        string message2 = webSocketClient.agv1Message;
        string message3 = webSocketClient.agv2Message;
        string message4 = webSocketClient.agv3Message;

        //conversto message 1 to json
        AssignJsonData(message1);

        // if name of the opbject is agv1 then assign the message to the object
        if (gameObject.name == "AGV1")
        {
            AssignJsonData(message1);
        }
        // if name of the opbject is agv2 then assign the message to the object
        else if (gameObject.name == "AGV2")
        {
            AssignJsonData(message2);
        }   
        // if name of the opbject is agv3 then assign the message to the object
        else if (gameObject.name == "AGV3")
        {
            AssignJsonData(message3);
        }
        // if name of the opbject is agv4 then assign the message to the object
        else if (gameObject.name == "AGV4")
        {
            AssignJsonData(message4);
        }






        



        
        
    }

 
    public void AssignJsonData(string agvMessage)
    {
        AgvMessage message = JsonUtility.FromJson<AgvMessage>(agvMessage);
        object_agv_id = message.agv_id;
        object_agv_location = message.agv_location;
        object_agv_segment = message.agv_segment;
        object_agv_status = message.agv_status;
        object_segment_timestamp = message.segment_timestamp;

    }

   
    public string GetAgvData()
    {
        AgvMessage message = new AgvMessage();
        message.agv_id = object_agv_id;
        message.agv_location = object_agv_location;
        message.agv_segment = object_agv_segment;
        message.agv_status = object_agv_status;
        message.segment_timestamp = object_segment_timestamp;
        
        

       

        return JsonUtility.ToJson(message);
    }





}
