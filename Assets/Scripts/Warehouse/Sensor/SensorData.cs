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
        // Parse the JSON message
        SensorMessage message = JsonUtility.FromJson<SensorMessage>(sensorMessage);

        // // Extract coordinates from sensor_location string
        // Vector2 sensorCoords = ParseCoordinates(message.sensor_location);
        // int positionZ = (int)(2 * sensorCoords.x - 22 + 2);
        // int positionX = (int)(55 - 2 * sensorCoords.y);
        // Vector3 newLocation = new Vector3(positionX, 13, positionZ);


        sensor_type = message.sensor_type;
        sensor_id = message.sensor_id;
        partition_id = message.partition_id;
        sensor_location = message.sensor_location;
        reading = message.reading;
        status = message.status;


    }



    private Vector2 ParseCoordinates(string coordinates)
    {
        // Remove parentheses and split by comma
        coordinates = coordinates.Trim('(', ')');
        string[] parts = coordinates.Split(',');

        // Parse x and y as floats
        float x = float.Parse(parts[0]);
        float y = float.Parse(parts[1]);

        return new Vector2(x, y);
    }


    //return sensor json data when called with the sensor name assigned
    public string GetSensorData(string sensorName)
    {
        return "";
    }
}
