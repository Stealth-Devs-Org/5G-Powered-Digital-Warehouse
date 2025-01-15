// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class SensorControllerTemperature : MonoBehaviour
// {
//     WebSocketClientSensor webSocketClientSensor;

//     // SensorData sensorData;
    
//     public GameObject TempSensorPrefab;     
//     private GameObject SensorObject;     
    
//     private Vector3 targetPosition;    // Target position for the AGV

//     public class SensorMessage
//     {
//         public string sensor_type;      
//         public string sensor_id;       
//         public int partition_id;
//         public string sensor_location; // Location in the format "(x,y)"
//         public double reading;         // Reading
//         public int status;             // Status code
//     }

//     private string sensorMessage;

//     void Start()
//     {
//         webSocketClientSensor = FindObjectOfType<WebSocketClientSensor>();

//         if (webSocketClientSensor == null)
//         {
//             Debug.LogError("WebSocketClient not found in the scene!");
//         }
//     }

//     void Update()
//     {
//         if (webSocketClientSensor != null && webSocketClientSensor.sensorMessage != null && webSocketClientSensor.newmessageArrviedforSensorTemp)
//         {
//             webSocketClientSensor.newmessageArrviedforSensorTemp = false;
//             sensorMessage = webSocketClientSensor.sensorMessage;

//             // Parse the JSON message
//             SensorMessage message = JsonUtility.FromJson<SensorMessage>(sensorMessage);

//             //Debug.Log($"Sensor Location: {message.sensor_location}");

//             // Debug.Log($"Sensor Type: {message.sensor_type}");
//             // Debug.Log($"Sensor ID: {message.sensor_id}");
//             // Debug.Log($"Partition ID: {message.partition_id}");
//             // Debug.Log($"Location Sensor: {message.sensor_location}");

//             // Extract coordinates from sensor_location string
//             Vector2 sensorCoords = ParseCoordinates(message.sensor_location);
//             int positionZ = (int)(2 * sensorCoords.x - 22 + 2);
//             int positionX = (int)(55 - 2 * sensorCoords.y);
//             Vector3 newLocation = new Vector3(positionX, 13, positionZ);

//             //Debug.Log($"New Location: {newLocation}");

//             // Debug.Log($"Reading: {message.reading}");
//             // Debug.Log($"Status: {message.status}");

//             // Add the new location to the queue
            

//             // // Instantiate the sensor object if it doesn't exist with it name on scene



            
//             // if ( SensorObject.name != "TempSensor" + message.sensor_id)
//             // {
//             //     SensorObject = Instantiate(TempSensorPrefab, newLocation, Quaternion.identity);
//             //     SensorObject.name = "TempSensor"+ message.sensor_id;
//             //     targetPosition = newLocation; // Set initial target position
                
//             // }

//             string sensorName = "TempSensor" + message.sensor_id;
//             GameObject existingSensor = GameObject.Find(sensorName);

//             if (existingSensor == null) // If no sensor exists with this name
//             {
//                 // Instantiate a new sensor object
//                 SensorObject = Instantiate(TempSensorPrefab, newLocation, Quaternion.identity);
//                 SensorObject.name = sensorName;
//                 //Asign it JSON data to the sensor object's script under this object

//                 SensorData sensorData = SensorObject.GetComponent<SensorData>();
//                 if (sensorData != null)
//                 {
                    
//                     sensorData.AssignJsonData(sensorMessage);

//                     // sensorData.AssignJsonData(sensorMessage);
//                 }
//                 else
//                 {
//                     Debug.LogError("SensorData script not found in the sensor object!");
//                 }

            

//             }
//             else
//             {
//                 // If the sensor exists, use it
//                 SensorObject = existingSensor;
//                 //Debug.Log($"Found existing sensor: {sensorName}");
//             }

//             // Set or update the target position
//             targetPosition = newLocation;





//         }
//     }

//     // Parses the coordinates from the sensor_location string
//     private Vector2 ParseCoordinates(string coordinates)
//     {
//         // Remove parentheses and split by comma
//         coordinates = coordinates.Trim('(', ')');
//         string[] parts = coordinates.Split(',');

//         // Parse x and y as floats
//         float x = float.Parse(parts[0]);
//         float y = float.Parse(parts[1]);

//         return new Vector2(x, y);
//     }





     




// }




















using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorControllerTemperature : MonoBehaviour
{
    WebSocketClientSensorTemp webSocketClientSensor;

    public GameObject TempSensorPrefab;     
    private GameObject SensorObject;     
    private Vector3 targetPosition;   

    public class SensorMessage
    {
        public string sensor_type;      
        public string sensor_id;       
        public int partition_id;
        public string sensor_location; 
        public double reading;        
        public int status;             
    }

    private string sensorMessage;

    void Start()
    {
        webSocketClientSensor = FindFirstObjectByType<WebSocketClientSensorTemp>();

        if (webSocketClientSensor == null)
        {
            Debug.LogError("WebSocketClient not found in the scene!");
        }
    }

    void Update()
    {
        if (webSocketClientSensor != null && webSocketClientSensor.sensorMessageTemp != null && webSocketClientSensor.newmessageArrviedforSensorTemp)
        {
            
            webSocketClientSensor.newmessageArrviedforSensorTemp = false;
            sensorMessage = webSocketClientSensor.sensorMessageTemp;

            SensorMessage message = JsonUtility.FromJson<SensorMessage>(sensorMessage);

            //Debug.Log($"Sensor Location: {message.status}");

            Vector2 sensorCoords = ParseCoordinates(message.sensor_location);
            int positionZ = (int)(2 * sensorCoords.x - 22 + 2);
            int positionX = (int)(55 - 2 * sensorCoords.y);
            Vector3 newLocation = new Vector3(positionX, 13, positionZ);

            
            string sensorName = "TempSensor" + message.sensor_id;
            GameObject existingSensor = GameObject.Find(sensorName);

            if (existingSensor == null) 
            {
               
                SensorObject = Instantiate(TempSensorPrefab, newLocation, Quaternion.identity);
                SensorObject.name = sensorName;
            }
            else
            {
            
                SensorObject = existingSensor;
                SensorObject.transform.position = newLocation;
            }


            SensorData sensorData = SensorObject.GetComponent<SensorData>();
            if (sensorData != null)
            {
                sensorData.AssignJsonData(sensorMessage); 
            }
            else
            {
                Debug.LogError("SensorData script not found in the sensor object!");
            }

            // Update target position
            targetPosition = newLocation;
        }
    }

    // Parses the coordinates from the sensor_location string
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
}











