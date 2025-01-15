using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class TempSensorView : MonoBehaviour
{
    private Camera mainCamera; // Reference to the main camera
    public float cameraSpeed = 5f; // Speed at which the camera moves to the target
    public List<GameObject> tempSensors = new List<GameObject>(); // List to store unique TempSensor objects
    public int currentSensorIndex = 0; // Keeps track of the current sensor to focus on
    private Coroutine currentFocusCoroutine; // Reference to the currently running coroutine

    public GameObject sensorDetailPrefab; // Prefab for displaying sensor details
    private GameObject currentSensorDetailUI; // Current instance of the sensor detail UI
    private GameObject currentFocusedSensor; // Currently focused sensor

    private bool isFocusing = false; // Flag to check if the camera is focusing on a sensor

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
            return;
        }

        // Switch to the next sensor when '2' is pressed
        if (Input.GetKeyDown(KeyCode.Alpha2) && tempSensors.Count > 0  && !Input.GetKeyDown(KeyCode.Space) && !Input.GetKeyDown(KeyCode.Alpha3)&& !Input.GetKeyDown(KeyCode.Alpha4)&& !Input.GetKeyDown(KeyCode.Alpha5)&& !Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentSensorIndex = (currentSensorIndex + 1) % tempSensors.Count;

            // Stop the current coroutine (if any) before starting a new one
            if (currentFocusCoroutine != null)
            {
                StopCoroutine(currentFocusCoroutine);
            }

            // Start focusing the camera on the selected sensor
            currentFocusCoroutine = StartCoroutine(FocusOnSensor(tempSensors[currentSensorIndex].transform));
        }


                
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Alpha3)|| Input.GetKeyDown(KeyCode.Alpha4)|| Input.GetKeyDown(KeyCode.Alpha5)|| Input.GetKeyDown(KeyCode.Alpha1))
        {
            // Stop focusing the camera
            StopFocusing(mainCamera.transform.position);
            // StopFocusing();
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

        // Remove null entries (in case sensors are destroyed)
        tempSensors.RemoveAll(sensor => sensor == null);
    }

    private IEnumerator FocusOnSensor(Transform sensorTransform)
    {
        isFocusing = true; // Set the focusing flag
        currentFocusedSensor = sensorTransform.gameObject;

        Vector3 targetPosition = sensorTransform.position + sensorTransform.forward * -5 + Vector3.up * 2;
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

        // Start dynamically updating the text
        StartCoroutine(UpdateSensorDetails());
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

        if (sensorDetailPrefab != null)
        {
            currentSensorDetailUI = Instantiate(sensorDetailPrefab, guiCanva.transform);
            currentSensorDetailUI.transform.position = mainCamera.transform.position + mainCamera.transform.forward * 4;
            currentSensorDetailUI.transform.rotation = Quaternion.LookRotation(mainCamera.transform.forward);
            currentSensorDetailUI.name = "SF WindowSensor";
        }
    }

    private IEnumerator UpdateSensorDetails()
    {
        while (isFocusing && currentFocusedSensor != null)
        {
            if (currentSensorDetailUI != null)
            {
                Transform detailsTransform = currentSensorDetailUI.transform.Find("Details");
                if (detailsTransform != null)
                {
                    TMP_Text detailsText = detailsTransform.GetComponent<TMP_Text>();
                    if (detailsText != null)
                    {
                        detailsText.text = FormatJsonForDisplay(currentFocusedSensor);
                    }
                }
            }
            yield return new WaitForSeconds(1f); // Update every second
        }
    }

    private string FormatJsonForDisplay(GameObject sensor)
    {
        SensorData sensorData = sensor.GetComponent<SensorData>();
        if (sensorData == null)
        {
            Debug.LogError("SensorData component not found on sensor object!");
            return "No Data Available";
        }

        string sensorMessage = sensorData.GetSensorData();
        SensorData.SensorMessage message = JsonUtility.FromJson<SensorData.SensorMessage>(sensorMessage);

        string formattedText = $"Sensor Type: {message.sensor_type}\n\n" +
                               $"Sensor ID: {message.sensor_id}\n\n" +
                               $"Partition ID: {message.partition_id}\n\n" +
                               $"Location: {message.sensor_location}\n\n" +
                               $"Reading: {message.reading}\n\n" +
                               $"Status: {message.current_status}";

        return formattedText;
    }




    private void StopFocusing(Vector3 currentCameraPosition)
    {
        isFocusing = false;

        // Destroy the AGV detail UI if it's displayed
        if (currentSensorDetailUI != null)
        {
            Destroy(currentSensorDetailUI);
            currentSensorDetailUI = null;
        }

        mainCamera.transform.position = currentCameraPosition;  
        mainCamera.transform.LookAt(Vector3.zero); 
    }
}
























