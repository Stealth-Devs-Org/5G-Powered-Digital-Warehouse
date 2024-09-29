using WebSocketSharp;
using System.Collections;
using UnityEngine;
using System.Numerics;

public class WebSocketDataReceive : MonoBehaviour
{
    public string url;
    private Vector2Int AGV1Cordinate;
    private Vector2Int AGV2Cordinate;
    private Vector2Int AGV3Cordinate;
    private Vector2Int AGV4Cordinate;
    WebSocket ws;
    string DataReceived;
    public int numberOfAGV = 4;
    public bool[] isDataReceivedforAGVs;
    [System.Serializable]
    public class DataObject   //own JSON
    {
        public int agv_id;
        public float[] location;
        public int segment;
        public int status;
        public string timestamp;
    }

    private void Start()
    {
        isDataReceivedforAGVs = new bool[numberOfAGV];
        for (int i = 0; i < numberOfAGV; i++)
        {
            isDataReceivedforAGVs[i] = false;  //initially no AGV present...
        }


        ws = new WebSocket(url);
        ws.Connect();
        ws.OnMessage += (sender, e) =>
        {
            
            DataReceived = e.Data;
            DataObject dataObject = JsonUtility.FromJson<DataObject>(DataReceived);
            
            if (dataObject != null && dataObject.location != null)
            {
                isDataReceivedforAGVs[0] = true; //for now only 1 web server is connected
                Debug.Log($"Location: x={dataObject.location[0]}, y={dataObject.location[1]}");
                AGV1Cordinate = new Vector2Int((int)dataObject.location[0], (int)dataObject.location[1]);
            }
            else
            {
                Debug.LogWarning("Failed to parse AGV data.");
            }
        };
    }

    // private void Update()
    // {
    //     if (Input.GetKey(KeyCode.Space))
    //     {
    //         ws.Send("Hello"); //Test
    //     }
    // }


    public Vector2Int ReturnAGVLocation(int id)
    {
        if (id==1)
        {
            return AGV1Cordinate;
        }
        else
        {
            return new Vector2Int(0, 0);
        }
        
    }
}
