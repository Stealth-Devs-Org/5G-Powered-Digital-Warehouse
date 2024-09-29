using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AGVController : MonoBehaviour
{

    GetLocationOfSphere getLocationOfSphere;
    WebSocketDataReceive webSocketDataReceive;
    private int numberOfAGVs = 0;

    public bool[] AGVPresent;

    public Vector2Int[] agvLocations;

    public GameObject agvPrefab;
    
    

    // Start is called before the first frame update
    void Start()
    {   
        getLocationOfSphere = FindObjectOfType<GetLocationOfSphere>();
        webSocketDataReceive = FindObjectOfType<WebSocketDataReceive>();

        numberOfAGVs = webSocketDataReceive.numberOfAGV;

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

        
    }

    // Update is called once per frame
    void Update()
    {

        
        for (int i=0; i<numberOfAGVs; i++)
        {
            if (webSocketDataReceive.isDataReceivedforAGVs[i] == true)
            {
                agvLocations[i] = getLocationOfSphere.ReturnLocationCordinate(webSocketDataReceive.ReturnAGVLocation(i));
                Debug.Log("AGV Location: " + agvLocations[i]);
            }
        }





        for (int i = 0; i < numberOfAGVs; i++)
        {
            if (webSocketDataReceive.isDataReceivedforAGVs[i] == true)
            {
                if (AGVPresent[i] == false)
                {
                    //SpawnAGVInLocation(location);
                    AGVPresent[i] = true;
                }


        }
 
        }
        

    }



    

    // Method to spawn an AGV at a given location
    void SpawnAGVInLocation(Vector2Int location)
    {
        // Convert Vector2Int to Vector3 for 3D space here y is Z actually all y are 0
        Vector3 spawnPosition = new Vector3(location.x, 0, location.y);

        // Spawn the AGV at the given location with no rotation (Quaternion.identity)
        Instantiate(agvPrefab, spawnPosition, Quaternion.identity);
    }


}
