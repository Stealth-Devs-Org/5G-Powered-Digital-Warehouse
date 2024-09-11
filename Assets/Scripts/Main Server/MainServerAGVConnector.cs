// using WebSocketSharp;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class WebSocketWorker : MonoBehaviour
// {
//     public string url;
//     WebSocket ws;
//     string DataReceived;

//     private void Start()
//     {
//         ws = new WebSocket(url);
//         ws.Connect();
//         ws.OnMessage += (sender, e) =>
//         {
//             DataReceived = e.Data;
//             Debug.Log(DataReceived);
//         };
//     }

//     private void Update()
//     {
//         if (Input.GetKey(KeyCode.Space))
//         {
//             ws.Send("Hello");

//         }
//     }
// }