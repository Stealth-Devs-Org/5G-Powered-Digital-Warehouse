using System;
using UnityEngine;
using MikeSchweitzer.WebSocket;
using System.IO;
using System.Collections.Generic;


public class WebSocketClientSensorHumidity : MonoBehaviour
{
    // Reference to the WebSocketConnection component
    public WebSocketConnection _connection;

    public bool newmessageArrviedforSensorHumidity = false;
    

    private string fileName = "IP_Addressdata.txt";

    public string WebSocketExtenstion;
    string filePath;
    string dataRead;


    public class SensorMessage
    {
        public string sensor_type;      
        public string sensor_id;       
        public int partition_id;
        public string sensor_location;      // Location [x, y]
        public double reading;      // Reading
        public int current_status;          // Status code
 
    }

    private SensorMessage MSGTest;
    public string sensorMessageTemp;
    public string sensorMessageAirQ;
    public string sensorMessageHumidity;


    // URL of the WebSocket server
    public string url_ ;  // "ws://localhost:8765/sensor"; // Replace with your WebSocket server URL


    private void Awake()
    {
        filePath = Path.Combine(Application.dataPath, "../" + fileName);

        if (File.Exists(filePath))
        {
            // Read the string from the file
            dataRead = File.ReadAllText(filePath);
            //Debug.Log("Data read from file: " + dataRead);
            url_= "ws://"+dataRead+":8765/"+WebSocketExtenstion;
            //Debug.Log("URL is set to: " + url_);
        }
        else
        {
            Debug.LogWarning("File not found: " + filePath);
        }




        if (_connection == null)
        {
            // Add WebSocketConnection component dynamically if not already added
            _connection = gameObject.AddComponent<WebSocketConnection>();
        }

        // Set up event listeners
        _connection.MessageReceived += OnMessageReceived;
        _connection.ErrorMessageReceived += OnErrorMessageReceived;
        _connection.StateChanged += OnStateChanged;

        // Configure and connect
        ConnectToServer();
    }

    private void OnDestroy()
    {
        // Clean up event listeners to avoid memory leaks
        _connection.MessageReceived -= OnMessageReceived;
        _connection.ErrorMessageReceived -= OnErrorMessageReceived;
        _connection.StateChanged -= OnStateChanged;

        DisconnectFromServer();
    }

    private void ConnectToServer()
    {
        // Configure WebSocket
        _connection.DesiredConfig = new WebSocketConfig
        {
            Url = url_,
            PingInterval = TimeSpan.FromSeconds(3), // Optional: Ping every 3 seconds
            PingMessage = new WebSocketMessage("ping") // Optional: Custom ping message
        };

        // Connect to the WebSocket server
        _connection.Connect();
        Debug.Log("Connecting to WebSocket server...");
    }

    private void DisconnectFromServer()
    {
        // Disconnect from the WebSocket server
        _connection.Disconnect();
        Debug.Log("Disconnected from WebSocket server.");
    }

    // Handle incoming messages
    private void OnMessageReceived(WebSocketConnection connection, WebSocketMessage message)
    {
        //Debug.Log($"Message received: {message.String}");
        //sensorMessage = message.String;
        MSGTest = JsonUtility.FromJson<SensorMessage>(message.String);

        //Debug.Log($"Sensor Type: {MSGTest.current_status}");




  
        
        if (MSGTest.sensor_type == "Humidity")
        {
            sensorMessageHumidity = message.String;
            newmessageArrviedforSensorHumidity = true;
        }

 
    
       
    }


    // Handle errors
    private void OnErrorMessageReceived(WebSocketConnection connection, string errorMessage)
    {
        Debug.LogError($"WebSocket error: {errorMessage}");
    }

    // Handle state changes (e.g., connected, disconnected)
    private void OnStateChanged(WebSocketConnection connection, WebSocketState oldState, WebSocketState newState)
    {
        //Debug.Log($"WebSocket state changed from {oldState} to {newState}");
    }

    // // Example: Sending a message to the WebSocket server                  // //SAIRISAN EDITED ----------------
    // public void SendMessageToServer(string message)
    // {
    //     if (_connection.State == WebSocketState.Connected)
    //     {
    //         _connection.AddOutgoingMessage(message);
    //         Debug.Log($"Message sent: {message}");
    //     }
    //     else
    //     {
    //         Debug.LogWarning("Cannot send message. WebSocket is not connected.");
    //     }
    // }
}


































