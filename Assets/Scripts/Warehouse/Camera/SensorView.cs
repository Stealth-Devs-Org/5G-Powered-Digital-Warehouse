// using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;

// public class SensorView : MonoBehaviour
// {
//     private Camera mainCamera; // Reference to the main camera
//     public float cameraSpeed = 5f; // Speed at which the camera moves to the target
//     public List<GameObject> tempSensors = new List<GameObject>(); // List to store unique TempSensor objects
//     private int currentSensorIndex = 0; // Keeps track of the current sensor to focus on
//     private Coroutine currentFocusCoroutine; // Reference to the currently running coroutine

//     void Start()
//     {
//         // Automatically assign the camera if not set
//         if (mainCamera == null)
//         {
//             mainCamera = GetComponent<Camera>();
//             if (mainCamera == null)
//             {
//                 Debug.LogError("Camera component is not assigned or found!");
//                 return;
//             }
//         }
//     }

//     void Update()
//     {
//         // Dynamically update the list of TempSensor objects
//         UpdateSensorList();

//         // Log a warning if no sensors are found
//         if (tempSensors.Count == 0)
//         {
//             Debug.LogWarning("No TempSensor objects found in the scene!");
//             return;
//         }

//         // Switch to the next sensor when '2' is pressed
//         if (Input.GetKeyDown(KeyCode.Alpha2) && tempSensors.Count > 0)
//         {
//             // Move to the next sensor in the list
//             currentSensorIndex = (currentSensorIndex + 1) % tempSensors.Count;

//             // Stop the current coroutine (if any) before starting a new one
//             if (currentFocusCoroutine != null)
//             {
//                 StopCoroutine(currentFocusCoroutine);
//             }

//             // Start focusing the camera on the selected sensor
//             currentFocusCoroutine = StartCoroutine(FocusOnSensor(tempSensors[currentSensorIndex].transform));
//         }
//     }

//     private void UpdateSensorList()
//     {
//         // Find all objects tagged as TempSensor in the scene
//         GameObject[] allSensors = GameObject.FindGameObjectsWithTag("TempSensor");

//         // Add new sensors to the list, avoiding duplicates
//         foreach (GameObject sensor in allSensors)
//         {
//             if (!tempSensors.Contains(sensor))
//             {
//                 tempSensors.Add(sensor);
//             }
//         }

//         // Remove null entries (in case sensors are destroyed)
//         tempSensors.RemoveAll(sensor => sensor == null);
//     }

//     private IEnumerator FocusOnSensor(Transform sensorTransform)
//     {
//         // Smoothly move the camera to the sensor's position
//         Vector3 targetPosition = sensorTransform.position + sensorTransform.forward * -5 + Vector3.up * 2; // Adjust offset as needed
//         while (Vector3.Distance(mainCamera.transform.position, targetPosition) > 0.1f)
//         {
//             mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, Time.deltaTime * cameraSpeed);
//             mainCamera.transform.LookAt(sensorTransform.position);
//             yield return null;
//         }

//         // Ensure the camera is correctly positioned at the end
//         mainCamera.transform.position = targetPosition;
//         mainCamera.transform.LookAt(sensorTransform.position);
//     }
// }














using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using NUnit.Framework.Constraints; // Add this to use TextMesh Pro

public class SensorView : MonoBehaviour
{
    private Camera mainCamera; // Reference to the main camera
    public float cameraSpeed = 5f; // Speed at which the camera moves to the target
    public List<GameObject> tempSensors = new List<GameObject>(); // List to store unique TempSensor objects
    public int currentSensorIndex = 0; // Keeps track of the current sensor to focus on
    private Coroutine currentFocusCoroutine; // Reference to the currently running coroutine

    public GameObject sensorDetailPrefab; // Prefab for displaying sensor details
    private GameObject currentSensorDetailUI; // Current instance of the sensor detail UI

    void Start()
    {
        // Automatically assign the camera if not set
        if (mainCamera == null)
        {
            mainCamera = GetComponent<Camera>();
            if (mainCamera == null)
            {
                Debug.LogError("Camera component is not assigned or found!");
                return;
            }
        }
    }

    void Update()
    {

        UpdateSensorList();
        if (tempSensors.Count == 0)
        {
            //Debug.LogWarning("No TempSensor objects found in the scene!");
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && tempSensors.Count > 0)  // swtich when 2 is pressed
        {
            // Move to the next sensor in the list
            currentSensorIndex = (currentSensorIndex + 1) % tempSensors.Count;

            // Stop the current coroutine (if any) before starting a new one
            if (currentFocusCoroutine != null)
            {
                StopCoroutine(currentFocusCoroutine);
            }

            // Start focusing the camera 
            currentFocusCoroutine = StartCoroutine(FocusOnSensor(tempSensors[currentSensorIndex].transform));
        }
    }

    private void UpdateSensorList()
    {

        GameObject[] allSensors = GameObject.FindGameObjectsWithTag("TempSensor");


        foreach (GameObject sensor in allSensors)
        {
            if (!tempSensors.Contains(sensor))
            {
                tempSensors.Add(sensor);
                //Debug.Log(sensor.name);
            }
        }

        tempSensors.RemoveAll(sensor => sensor == null);  //Remove null entries (in case sensors are destroyed)
    }


    // get the name of the camera focusing sensor and return it name



    private IEnumerator FocusOnSensor(Transform sensorTransform)
    {

        Vector3 targetPosition = sensorTransform.position + sensorTransform.forward * -5 + Vector3.up * 2; // Adjust offset as needed
        while (Vector3.Distance(mainCamera.transform.position, targetPosition) > 0.1f)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, Time.deltaTime * cameraSpeed);
            mainCamera.transform.LookAt(sensorTransform.position);
            yield return null;
        }

        mainCamera.transform.position = targetPosition;
        mainCamera.transform.LookAt(sensorTransform.position);

        // Display the sensor detail UI
        ShowSensorDetails(sensorTransform.gameObject);

        
    }





    // private void ShowSensorDetails(GameObject sensor)
    // {
    //     if (currentSensorDetailUI != null)
    //     {
    //         Destroy(currentSensorDetailUI);
    //     }


    //     GameObject guiCanva = GameObject.Find("GUICanva");
    //     if (guiCanva == null)
    //     {
    //         Debug.LogError("GUICanva object not found in the scene!");
    //         return;
    //     }

    //     if (sensorDetailPrefab != null)
    //     {
    //         // Instantiate the prefab as a child of GUICanva under child of GUICanva
    //         currentSensorDetailUI = Instantiate(sensorDetailPrefab, guiCanva.transform);
    //         // rename the object to the sensor name
    //         currentSensorDetailUI.name = "SF Window";

    //         // assign this to child of GUICanva
    //         currentSensorDetailUI.transform.SetParent(guiCanva.transform);

    //         // Position and rotate the prefab so it faces the camera
    //         currentSensorDetailUI.transform.position = mainCamera.transform.position + mainCamera.transform.forward * 4;
    //         currentSensorDetailUI.transform.rotation = Quaternion.LookRotation(mainCamera.transform.forward);
    //     }

    //     Transform detailsTransform = guiCanva.transform.Find("SF Window/Details");
    //     if (detailsTransform == null)
    //     {
    //         Debug.LogError("Details object not found");
    //         return;
    //     }

    //     TMP_Text detailsText = detailsTransform.GetComponent<TMP_Text>();
    //     if (detailsText == null)
    //     {
    //         Debug.LogError("Details object does not contain a TextMesh Pro component!");
    //         return;
    //     }



    //     detailsText.text = FormatJsonForDisplay(sensor);
    //     Debug.Log("Focusing on sensor: " + sensor.name);
    // }



    private void ShowSensorDetails(GameObject sensor)
    {
        
        if (currentSensorDetailUI != null)
        {
            Destroy(currentSensorDetailUI);
        }

        
        GameObject guiCanva = GameObject.Find("GUICanva");
        if (guiCanva == null)
        {
            Debug.LogError("GUICanva object not found in the scene!");
            return;
        }

        if (sensorDetailPrefab != null)
        {
            
            currentSensorDetailUI = Instantiate(sensorDetailPrefab, guiCanva.transform);

            
            currentSensorDetailUI.transform.position = mainCamera.transform.position + mainCamera.transform.forward * 4;
            currentSensorDetailUI.transform.rotation = Quaternion.LookRotation(mainCamera.transform.forward);

            
            currentSensorDetailUI.name = "SF Window";
        }

        
        Transform detailsTransform = currentSensorDetailUI.transform.Find("Details");
        if (detailsTransform == null)
        {
            Debug.LogError("Details object not found in the sensor detail UI!");
            return;
        }

        
        TMP_Text detailsText = detailsTransform.GetComponent<TMP_Text>();
        if (detailsText == null)
        {
            Debug.LogError("Details object does not contain a TextMesh Pro component!");
            return;
        }

        
        detailsText.text = FormatJsonForDisplay(sensor);

        Debug.Log("Focusing on sensor: " + sensor.name);
    }



    private string FormatJsonForDisplay(GameObject sensor)
    {
        SensorData sensorData = sensor.GetComponent<SensorData>();
        // // under sensor object, get the SensorData script
        // SensorData sensorData = tempSensors[currentSensorIndex].GetComponent<SensorData>();
        // // get the sensor data from the SensorData script
        string sensorMessage = sensorData.GetSensorData();
        // // assign the sensor data to the SensorMessage object
        SensorData.SensorMessage message = JsonUtility.FromJson<SensorData.SensorMessage>(sensorMessage);



        
       
        
        // Extract message fields
        string sensorType = message.sensor_type;
        string sensorId = message.sensor_id;
        string partitionId = message.partition_id.ToString();
        string sensorLocation = message.sensor_location;
        string reading = message.reading.ToString();
        string status = message.status.ToString();

        // Format the text
        string formattedText = $"Sensor Type: {sensorType}\n\n" +
                               $"Sensor ID: {sensorId}\n\n" +
                               $"Partition ID: {partitionId}\n\n" +
                               $"Location: {sensorLocation}\n\n" +
                               $"Reading: {reading}\n\n" +
                               $"Status: {status}";

        return formattedText;
    }


}







