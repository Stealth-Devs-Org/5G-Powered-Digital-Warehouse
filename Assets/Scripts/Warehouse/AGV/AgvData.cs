using UnityEngine;
using System.Collections;


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


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        
    }

 
    public void AssignJsonData(string sensorMessage)
    {
        AgvMessage message = JsonUtility.FromJson<AgvMessage>(sensorMessage);
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
