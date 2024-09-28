using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AGVController : MonoBehaviour
{

    GetLocationOfSphere getLocationOfSphere;
    WebSocketDataReceive webSocketDataReceive;

    

    // Start is called before the first frame update
    void Start()
    {
        getLocationOfSphere = GetComponent<GetLocationOfSphere>();
        webSocketDataReceive = GetComponent<WebSocketDataReceive>();
        
    }

    // Update is called once per frame
    void Update()
    {
        webSocketDataReceive. 
        getLocationOfSphere.GetLocation();

        
    }
}
