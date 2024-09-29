using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AGVController : MonoBehaviour
{

    GetLocationOfSphere getLocationOfSphere;
    WebSocketDataReceive webSocketDataReceive;
    public int numberOfAGV = 4;

    public bool[] AGVPresent;
    
    

    // Start is called before the first frame update
    void Start()
    {
        getLocationOfSphere = GetComponent<GetLocationOfSphere>();
        webSocketDataReceive = GetComponent<WebSocketDataReceive>();

        AGVPresent = new bool[numberOfAGV];
        for (int i = 0; i < numberOfAGV; i++)
        {
            AGVPresent[i] = false;  //initially no AGV present...
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        
      

        
    }
}
