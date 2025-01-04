using UnityEngine;


public class SensorData : MonoBehaviour
{

    public string object_sensor_type;
    public string object_sensor_id;
    public int object_partition_id;
    public string object_sensor_location;
    public double object_reading;
    public int object_status;

        


    
    public class SensorMessage
    {
        public string sensor_type;      
        public string sensor_id;       
        public int partition_id;
        public string sensor_location; // Location in the format "(x,y)"
        public double reading;         // Reading
        public int status;             // Status code
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // assign json data to the SensorMessage object
    public void AssignJsonData(string sensorMessage)
    {
        SensorMessage message = JsonUtility.FromJson<SensorMessage>(sensorMessage);
        object_sensor_type = message.sensor_type;
        object_sensor_id = message.sensor_id;
        object_partition_id = message.partition_id;
        object_sensor_location = message.sensor_location;
        object_reading = message.reading;
        object_status = message.status;

    }

    //return sensor json data as JSON when called with the sensor name assigned
    public string GetSensorData()
    {
        SensorMessage message = new SensorMessage();
        message.sensor_type = object_sensor_type;
        message.sensor_id = object_sensor_id;
        message.partition_id = object_partition_id;
        message.sensor_location = object_sensor_location;
        message.reading = object_reading;
        message.status = object_status;
        

       

        return JsonUtility.ToJson(message);
    }

}
