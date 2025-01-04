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
using TMPro; // Add this to use TextMesh Pro

public class SensorView : MonoBehaviour
{
    private Camera mainCamera; // Reference to the main camera
    public float cameraSpeed = 5f; // Speed at which the camera moves to the target
    public List<GameObject> tempSensors = new List<GameObject>(); // List to store unique TempSensor objects
    private int currentSensorIndex = 0; // Keeps track of the current sensor to focus on
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
            Debug.LogWarning("No TempSensor objects found in the scene!");
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
            }
        }

        tempSensors.RemoveAll(sensor => sensor == null);
    }

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

        Transform detailsTransform = guiCanva.transform.Find("SF Window/Details");
        if (detailsTransform == null)
        {
            Debug.LogError("Details object not found as a child of grandchild under GUICanva!");
            return;
        }

        TMP_Text detailsText = detailsTransform.GetComponent<TMP_Text>();
        if (detailsText == null)
        {
            Debug.LogError("Details object does not contain a TextMesh Pro component!");
            return;
        }

        if (sensorDetailPrefab != null)
        {
            // Instantiate the prefab as a child of GUICanva
            currentSensorDetailUI = Instantiate(sensorDetailPrefab, guiCanva.transform);

            // Position and rotate the prefab so it faces the camera
            currentSensorDetailUI.transform.position = mainCamera.transform.position + mainCamera.transform.forward * 4;
            currentSensorDetailUI.transform.rotation = Quaternion.LookRotation(mainCamera.transform.forward);
        }

        // Retrieve and display sensor details
        string sensorJson = GetSensorDetailsAsJson(sensor); 
        detailsText.text = FormatJsonForDisplay(sensorJson);
    }



    private string GetSensorDetailsAsJson(GameObject sensor)
    {
        // Example: Simulating JSON data. 
        // return "{\n  \"SensorID\": \"12345\",\n  \"Temperature\": \"28°C\",\n  \"Location\": \"Room A\",\n  \"Status\": \"Active\"\n}";
        SensorData sensorData = sensor.GetComponent<SensorData>();

        // search for respective sensor data under temp sensor object
        if (sensorData != null)
        {
            return sensorData.GetSensorData();
        }
        else
        {
            Debug.LogError("SensorData script not found in the sensor object!");
            return "";
        }

    }

    private string FormatJsonForDisplay(string json)
    {
        // Optional: Format JSON  readability
        return json.Replace(",", "\n").Replace("{", "").Replace("}", "").Replace("\"", "").Trim();
    }

}























// using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;
// using TMPro; // Add this to use TextMesh Pro

// public class SensorView : MonoBehaviour
// {
//     private Camera mainCamera; // Reference to the main camera
//     public float cameraSpeed = 5f; // Speed at which the camera moves to the target
//     public List<GameObject> tempSensors = new List<GameObject>(); // List to store unique TempSensor objects
//     private int currentSensorIndex = 0; // Keeps track of the current sensor to focus on
//     private Coroutine currentFocusCoroutine; // Reference to the currently running coroutine

//     public GameObject sensorDetailPrefab; // Prefab for displaying sensor details
//     private GameObject currentSensorDetailUI; // Current instance of the sensor detail UI

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

//         // Display the sensor detail UI
//         ShowSensorDetails(sensorTransform.gameObject);
//     }

//     private void ShowSensorDetails(GameObject sensor)
//     {
//         // Destroy the current UI if it exists
//         if (currentSensorDetailUI != null)
//         {
//             Destroy(currentSensorDetailUI);
//         }

//         // Find the GUICanva object in the scene
//         GameObject guiCanva = GameObject.Find("GUICanva");
//         if (guiCanva == null)
//         {
//             Debug.LogError("GUICanva object not found in the scene!");
//             return;
//         }

//         // Find the child of grandchild 'Details' under 'GUICanva'
//         Transform detailsTransform = guiCanva.transform.Find("ChildObject/ChildOfChild/Details");
//         if (detailsTransform == null)
//         {
//             Debug.LogError("Details object not found as a child of grandchild under GUICanva!");
//             return;
//         }

//         // Get the TextMesh Pro component on the Details object
//         TMP_Text detailsText = detailsTransform.GetComponent<TMP_Text>();
//         if (detailsText == null)
//         {
//             Debug.LogError("Details object does not contain a TextMesh Pro component!");
//             return;
//         }

//         // Instantiate the sensor detail prefab and attach it to the GUI canvas
//         if (sensorDetailPrefab != null)
//         {
//             // Instantiate the prefab as a child of GUICanva
//             currentSensorDetailUI = Instantiate(sensorDetailPrefab, guiCanva.transform);

//             // Position and rotate the prefab so it faces the camera
//             currentSensorDetailUI.transform.position = mainCamera.transform.position + mainCamera.transform.forward * 4;
//             currentSensorDetailUI.transform.rotation = Quaternion.LookRotation(mainCamera.transform.forward);
//         }

//         // Retrieve and display sensor details
//         string sensorJson = GetSensorDetailsAsJson(sensor); // Replace with your logic to fetch sensor data as JSON
//         detailsText.text = FormatJsonForDisplay(sensorJson);
//     }

//     private string GetSensorDetailsAsJson(GameObject sensor)
//     {
//         // Example: Simulating JSON data. Replace with actual sensor data fetching logic.
//         return "{\n  \"SensorID\": \"12345\",\n  \"Temperature\": \"28°C\",\n  \"Location\": \"Room A\",\n  \"Status\": \"Active\"\n}";
//     }

//     private string FormatJsonForDisplay(string json)
//     {
//         // Optional: Format JSON string for better readability
//         return json.Replace(",", "\n").Replace("{", "").Replace("}", "").Replace("\"", "").Trim();
//     }
// }
