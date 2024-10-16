using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AGVController : MonoBehaviour
{

    GetLocationOfSphere getLocationOfSphere;
    WebSocketDataReceive webSocketDataReceive;
    public int numberOfAGVs = 4;

    public bool[] AGVPresent;

    public int[] agvID = new [] {100, 100, 100, 100};  // not assigned yet...
    public Vector2Int[] agvLocations;
    public int[] agvStatus;

    public GameObject agvPrefab;
    
    

    // Start is called before the first frame update
    void Start()
    {   numberOfAGVs = 4;
        getLocationOfSphere = FindObjectOfType<GetLocationOfSphere>();
        webSocketDataReceive = FindObjectOfType<WebSocketDataReceive>();

        numberOfAGVs = webSocketDataReceive.numberOfAGV;

        //agvID = new int [numberOfAGVs];
        for (int i = 0; i < numberOfAGVs;  i++)
        {
            agvID[i] = 100;  //initially no AGV ID...
        }

        AGVPresent = new bool[numberOfAGVs];
        for (int i = 0; i < numberOfAGVs;  i++)
        {
            AGVPresent[i] = false;  //initially no AGV present...
        }

        agvLocations = new Vector2Int [numberOfAGVs];
        for (int i = 0; i < numberOfAGVs;  i++)
        {
            agvLocations[i] = new Vector2Int (0,0);  //initially no AGV present...
        }

        agvStatus = new int [numberOfAGVs];
        for (int i = 0; i < numberOfAGVs;  i++)
        {
            agvStatus[i] = 0;  //initially no AGV present...
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        if (webSocketDataReceive.receivedNextData)
        {
            for (int i = 0; i < numberOfAGVs; i++)
            {
                if (webSocketDataReceive.isDataReceivedforAGVs[i] == true)
                {
                    if (AGVPresent[i] == false)
                    {
                        SpawnAGVInLocation(agvLocations[i]);    // Spawn AGV at the given location
                        AGVPresent[i] = true;
                    }


                }

                else 
                {
                    // show red alerts connection lost
                }
    
            }



            agvID [0]= webSocketDataReceive.AGV1ID;
        

            
            for (int i=0; i<numberOfAGVs; i++)
            {
                if (webSocketDataReceive.isDataReceivedforAGVs[i] == true)
                {
                    Vector2Int receirvedCordinate = webSocketDataReceive.ReturnAGVLocation(i);
                    agvLocations[i] = getLocationOfSphere.ReturnLocationCordinate(receirvedCordinate);
                    agvStatus[i]= webSocketDataReceive.ReturnAGVStatus(i);
                    Debug.Log("AGV"+ (i+1).ToString() + " Cordinate: " + receirvedCordinate.ToString() +" World Location: " + agvLocations[i]);  // WORLD COORDINATE OF AGV
                    //Debug.Log("Test");
                }
            }

            webSocketDataReceive.receivedNextData = false;


        }


    }



    

    void SpawnAGVInLocation(Vector2Int location)
    {
        // Convert Vector2Int to Vector3 for 3D space here y is Z actually all y are 0
        Vector3 spawnPosition = new Vector3(location.x, 0, location.y);
        // Spawn the AGV at the given location with no rotation (Quaternion.identity)
        Instantiate(agvPrefab, spawnPosition, Quaternion.identity);
    }


}
