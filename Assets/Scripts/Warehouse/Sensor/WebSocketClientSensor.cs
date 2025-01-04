using System;
using UnityEngine;
using MikeSchweitzer.WebSocket;
using System.Collections.Generic;


public class WebSocketClientSensor : MonoBehaviour
{
    // Reference to the WebSocketConnection component
    public WebSocketConnection _connection;

    public bool newmessageArrviedforSensorTemp = false;
    public bool newmessageArrviedforSensorAirQ = false;
    public bool newmessageArrviedforSensorHumidity = false;
    public bool newmessageArrviedforSensorSmoke = false;


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
    public string sensorMessage;


    // URL of the WebSocket server
    public string _url = "ws://localhost:8765/sensor"; // Replace with your WebSocket server URL


    private void Awake()
    {
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
            Url = _url,
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

        Debug.Log($"Sensor Type: {MSGTest.current_status}");




        if (MSGTest.sensor_type == "Temperature")
        {
            sensorMessage = message.String;
            newmessageArrviedforSensorTemp = true;
            //Debug.Log($"Message received: {message.String}");
            
            
        }
        else if (MSGTest.sensor_type == "AirQuality")
        {
            sensorMessage = message.String;
            newmessageArrviedforSensorAirQ = true;
            
        }
        else if (MSGTest.sensor_type == "Humidity")
        {
            sensorMessage = message.String;
            newmessageArrviedforSensorHumidity = true;
        }
        else if (MSGTest.sensor_type == "Smoke")
        {
            sensorMessage = message.String;
            newmessageArrviedforSensorSmoke = true;
            
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


































