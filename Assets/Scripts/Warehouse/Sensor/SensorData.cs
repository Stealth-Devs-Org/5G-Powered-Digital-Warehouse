using UnityEngine;


public class SensorData : MonoBehaviour
{

    public string sensor_type;
    public string sensor_id;
    public int partition_id;
    public string sensor_location; 
    public double reading;         
    public int status;            


    
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
        sensor_type = message.sensor_type;
        sensor_id = message.sensor_id;
        partition_id = message.partition_id;
        sensor_location = message.sensor_location;
        reading = message.reading;
        status = message.status;
    }

    //return sensor json data as JSON when called with the sensor name assigned
    public string GetSensorData()
    {
        SensorMessage message = new SensorMessage();
        message.sensor_type = sensor_type;
        message.sensor_id = sensor_id;
        message.partition_id = partition_id;
        message.sensor_location = sensor_location;
        message.reading = reading;
        message.status = status;

        return JsonUtility.ToJson(message);
    }

}
