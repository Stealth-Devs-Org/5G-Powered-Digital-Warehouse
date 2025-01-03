using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SensorView : MonoBehaviour
{
    private Camera mainCamera; // Reference to the main camera
    public float cameraSpeed = 5f; // Speed at which the camera moves to the target
    public List<GameObject> tempSensors = new List<GameObject>(); // List to store unique TempSensor objects
    private int currentSensorIndex = 0; // Keeps track of the current sensor to focus on
    private Coroutine currentFocusCoroutine; // Reference to the currently running coroutine

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
        // Dynamically update the list of TempSensor objects
        UpdateSensorList();

        // Log a warning if no sensors are found
        if (tempSensors.Count == 0)
        {
            Debug.LogWarning("No TempSensor objects found in the scene!");
            return;
        }

        // Switch to the next sensor when '2' is pressed
        if (Input.GetKeyDown(KeyCode.Alpha2) && tempSensors.Count > 0)
        {
            // Move to the next sensor in the list
            currentSensorIndex = (currentSensorIndex + 1) % tempSensors.Count;

            // Stop the current coroutine (if any) before starting a new one
            if (currentFocusCoroutine != null)
            {
                StopCoroutine(currentFocusCoroutine);
            }

            // Start focusing the camera on the selected sensor
            currentFocusCoroutine = StartCoroutine(FocusOnSensor(tempSensors[currentSensorIndex].transform));
        }
    }

    private void UpdateSensorList()
    {
        // Find all objects tagged as TempSensor in the scene
        GameObject[] allSensors = GameObject.FindGameObjectsWithTag("TempSensor");

        // Add new sensors to the list, avoiding duplicates
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
        // Smoothly move the camera to the sensor's position
        Vector3 targetPosition = sensorTransform.position + sensorTransform.forward * -5 + Vector3.up * 2; // Adjust offset as needed
        while (Vector3.Distance(mainCamera.transform.position, targetPosition) > 0.1f)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, Time.deltaTime * cameraSpeed);
            mainCamera.transform.LookAt(sensorTransform.position);
            yield return null;
        }

        // Ensure the camera is correctly positioned at the end
        mainCamera.transform.position = targetPosition;
        mainCamera.transform.LookAt(sensorTransform.position);
    }
}
