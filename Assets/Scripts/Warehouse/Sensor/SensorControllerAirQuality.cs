using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorControllerAirQuality : MonoBehaviour
{
    WebSocketClientSensorAirQ webSocketClientSensor;

    public GameObject AirQualitySensorPrefab;     
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
        webSocketClientSensor = FindFirstObjectByType<WebSocketClientSensorAirQ>();

        if (webSocketClientSensor == null)
        {
            Debug.LogError("WebSocketClient not found in the scene!");
        }
    }

    void Update()
    {
        if (webSocketClientSensor != null && webSocketClientSensor.sensorMessageAirQ != null && webSocketClientSensor.newmessageArrviedforSensorAirQ)
        {
            Debug.Log("Air Quality Sensor Update");
            webSocketClientSensor.newmessageArrviedforSensorAirQ = false;
            sensorMessage = webSocketClientSensor.sensorMessageAirQ;

            SensorMessage message = JsonUtility.FromJson<SensorMessage>(sensorMessage);

            //Debug.Log($"Sensor Location: {message.status}");

            Vector2 sensorCoords = ParseCoordinates(message.sensor_location);
            int positionZ = (int)(2 * sensorCoords.x - 22 + 2);
            int positionX = (int)(55 - 2 * sensorCoords.y);
            Vector3 newLocation = new Vector3(positionX, 13, positionZ);

            
            string sensorName = "AirQualitySensor" + message.sensor_id;
            GameObject existingSensor = GameObject.Find(sensorName);

            if (existingSensor == null) 
            {
               
                SensorObject = Instantiate(AirQualitySensorPrefab, newLocation, Quaternion.identity);
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











