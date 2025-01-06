// using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;
// using TMPro;

// public class AgvView : MonoBehaviour
// {
//     private Camera mainCamera; 
//     public float cameraSpeed = 5f; 
//     public List<GameObject> AGVs = new List<GameObject>(); 
//     public int currentAgvIndex = 0; 
//     private Coroutine currentFocusCoroutine; 

//     public GameObject agvDetailPrefab; 
//     private GameObject currentAGVDetailUI; 
//     private GameObject currentFocusedAgv; 

//     private bool isFocusing = false; 

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
//         UpdateAgvList();

//         if (AGVs.Count == 0)
//         {
//             return;
//         }

//         // Switch to the next agv when '3' is pressed
//         if (Input.GetKeyDown(KeyCode.Alpha3) && AGVs.Count > 0)
//         {
//             currentAgvIndex = (currentAgvIndex + 1) % AGVs.Count;

//             // Stop the current coroutine (if any) before starting a new one
//             if (currentFocusCoroutine != null)
//             {
//                 StopCoroutine(currentFocusCoroutine);
//             }

//             // Start focusing the camera on the selected agv
//             currentFocusCoroutine = StartCoroutine(FocusOnAgv(AGVs[currentAgvIndex].transform));
//         }
//     }

//     private void UpdateAgvList()
//     {
//         GameObject[] allAgvs = GameObject.FindGameObjectsWithTag("AGV");

//         foreach (GameObject agv in allAgvs)
//         {
//             if (!AGVs.Contains(agv))
//             {
//                 AGVs.Add(agv);
//             }
//         }

//         // Remove null entries (in case agvs are destroyed)
//         AGVs.RemoveAll(agv => agv == null);
//     }

//     private IEnumerator FocusOnAgv(Transform agvTransform)
//     {
//         isFocusing = true; // Set the focusing flag
//         currentFocusedAgv = agvTransform.gameObject;

//         while (isFocusing && currentFocusedAgv != null)
//         {
//             // Calculate the target position for the camera
//             Vector3 targetPosition = agvTransform.position + agvTransform.forward * -5 + Vector3.up * 2;

//             // Smoothly move the camera to the target position
//             mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, Time.deltaTime * cameraSpeed);

//             // Make the camera look at the AGV
//             mainCamera.transform.LookAt(agvTransform.position);

//             yield return null; // Wait for the next frame
//         }

//         // Final adjustments if necessary
//         if (currentFocusedAgv != null)
//         {
//             Vector3 finalPosition = agvTransform.position + agvTransform.forward * -5 + Vector3.up * 2;
//             mainCamera.transform.position = finalPosition;
//             mainCamera.transform.LookAt(agvTransform.position);
//         }
//     }


//     private void ShowAgvDetails(GameObject agv)
//     {
//         if (currentAGVDetailUI != null)
//         {
//             Destroy(currentAGVDetailUI);
//         }

//         GameObject guiCanva = GameObject.Find("GUICanva");
//         if (guiCanva == null)
//         {
//             Debug.LogError("GUICanva object not found in the scene!");
//             return;
//         }

//         if (agvDetailPrefab != null)
//         {
//             currentAGVDetailUI = Instantiate(agvDetailPrefab, guiCanva.transform);
//             currentAGVDetailUI.transform.position = mainCamera.transform.position + mainCamera.transform.forward * 4;
//             currentAGVDetailUI.transform.rotation = Quaternion.LookRotation(mainCamera.transform.forward);
//             currentAGVDetailUI.name = "SF WindowAGV";
//         }
//     }

//     // private IEnumerator UpdateAgvDetails()
//     // {
//     //     while (isFocusing && currentFocusedAgv != null)
//     //     {
//     //         if (currentAGVDetailUI != null)
//     //         {
//     //             Transform detailsTransform = currentAGVDetailUI.transform.Find("Details");
//     //             if (detailsTransform != null)
//     //             {
//     //                 TMP_Text detailsText = detailsTransform.GetComponent<TMP_Text>();
//     //                 if (detailsText != null)
//     //                 {
//     //                     detailsText.text = FormatJsonForDisplay(currentFocusedAgv);
//     //                 }
//     //             }
//     //         }
//     //         yield return new WaitForSeconds(1f); // Update every second
//     //     }
//     // }




//     private IEnumerator UpdateAgvDetails()
//     {
//         while (isFocusing && currentFocusedAgv != null)
//         {
//             if (currentAGVDetailUI != null)
//             {
//                 // Update the position of the UI to follow the AGV
//                 Vector3 uiPosition = currentFocusedAgv.transform.position + currentFocusedAgv.transform.forward * -2 + Vector3.up * 2;
//                 currentAGVDetailUI.transform.position = uiPosition;

//                 // Ensure the UI faces the camera
//                 currentAGVDetailUI.transform.rotation = Quaternion.LookRotation(mainCamera.transform.forward);

//                 // Update the text details
//                 Transform detailsTransform = currentAGVDetailUI.transform.Find("Details");
//                 if (detailsTransform != null)
//                 {
//                     TMP_Text detailsText = detailsTransform.GetComponent<TMP_Text>();
//                     if (detailsText != null)
//                     {
//                         detailsText.text = FormatJsonForDisplay(currentFocusedAgv);
//                     }
//                 }
//             }

//             yield return new WaitForSeconds(0.1f); // Update frequently to keep up with AGV movement
//         }
//     }



//     private string FormatJsonForDisplay(GameObject agv)
//     {
//         AgvData agvData = agv.GetComponent<AgvData>();
//         if (agvData == null)
//         {
//             Debug.LogError("AgvData component not found on agv object!");
//             return "No Data Available";
//         }

//         string agvMessage = agvData.GetAgvData();
//         AgvData.AgvMessage message = JsonUtility.FromJson<AgvData.AgvMessage>(agvMessage);

//         string formattedText = $"AGV ID: {message.agv_id}\n\n" +
//                                  $"Location: {message.agv_location}\n\n" +
//                                  $"Segment: {message.agv_segment}\n\n" +
//                                  $"Reading: {message.agv_status}\n\n" +
//                                  $"Status Code: {message.segment_timestamp}";   
            

//         return formattedText;
//     }
// }

















// using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;
// using TMPro;

// public class AgvView : MonoBehaviour
// {
//     private Camera mainCamera; 
//     public float cameraSpeed = 5f; 
//     public List<GameObject> AGVs = new List<GameObject>(); 
//     public int currentAgvIndex = 0; 
//     private Coroutine currentFocusCoroutine; 

//     public GameObject agvDetailPrefab; 
//     private GameObject currentAGVDetailUI; 
//     private GameObject currentFocusedAgv; 

//     private bool isFocusing = false; 

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
//         UpdateAgvList();

//         if (AGVs.Count == 0)
//         {
//             return;
//         }

//         // Switch to the next AGV when '3' is pressed
//         if (Input.GetKeyDown(KeyCode.Alpha3) && AGVs.Count > 0)
//         {
//             currentAgvIndex = (currentAgvIndex + 1) % AGVs.Count;

//             // Stop the current coroutine (if any) before starting a new one
//             if (currentFocusCoroutine != null)
//             {
//                 StopCoroutine(currentFocusCoroutine);
//             }

//             // Start focusing the camera on the selected AGV
//             currentFocusCoroutine = StartCoroutine(FocusOnAgv(AGVs[currentAgvIndex].transform));
//         }
//     }

//     private void UpdateAgvList()
//     {
//         GameObject[] allAgvs = GameObject.FindGameObjectsWithTag("AGV");

//         foreach (GameObject agv in allAgvs)
//         {
//             if (!AGVs.Contains(agv))
//             {
//                 AGVs.Add(agv);
//             }
//         }

//         // Remove null entries (in case AGVs are destroyed)
//         AGVs.RemoveAll(agv => agv == null);
//     }

//     private IEnumerator FocusOnAgv(Transform agvTransform)
//     {
//         isFocusing = true; 
//         currentFocusedAgv = agvTransform.gameObject;

//         // Show AGV details UI
//         ShowAgvDetails(currentFocusedAgv);

//         while (isFocusing && currentFocusedAgv != null)
//         {
//             Vector3 targetPosition = agvTransform.position + agvTransform.forward * -5 + Vector3.up * 4;

//             // Smoothly move the camera to the target position
//             mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, Time.deltaTime * cameraSpeed);

//             // Make the camera look at the AGV
//             mainCamera.transform.LookAt(agvTransform.position);

//             yield return null; 
//         }

//         // Final adjustments if necessary
//         if (currentFocusedAgv != null)
//         {
//             Vector3 finalPosition = agvTransform.position + agvTransform.forward * -5 + Vector3.up * 3;
//             mainCamera.transform.position = finalPosition;
//             mainCamera.transform.LookAt(agvTransform.position);
//         }
//     }

//     private void ShowAgvDetails(GameObject agv)
//     {
//         if (currentAGVDetailUI != null)
//         {
//             Destroy(currentAGVDetailUI);
//         }

//         GameObject guiCanva = GameObject.Find("GUICanva");
//         if (guiCanva == null)
//         {
//             Debug.LogError("GUICanva object not found in the scene!");
//             return;
//         }

//         if (agvDetailPrefab != null)
//         {
//             currentAGVDetailUI = Instantiate(agvDetailPrefab, guiCanva.transform);
//             currentAGVDetailUI.transform.position = mainCamera.transform.position + mainCamera.transform.forward * 4;
//             currentAGVDetailUI.transform.rotation = Quaternion.LookRotation(mainCamera.transform.forward);
//             currentAGVDetailUI.name = "AGV Detail UI";

//             // Start updating the AGV details
//             StartCoroutine(UpdateAgvDetails());
//         }
//     }

//     private IEnumerator UpdateAgvDetails()
//     {
//         while (isFocusing && currentFocusedAgv != null)
//         {
//             if (currentAGVDetailUI != null)
//             {
//                 // Update the position of the UI to follow the AGV
//                 Vector3 uiPosition = currentFocusedAgv.transform.position + currentFocusedAgv.transform.forward * -2 + Vector3.up * 2;
//                 currentAGVDetailUI.transform.position = uiPosition;

//                 // Ensure the UI faces the camera
//                 currentAGVDetailUI.transform.rotation = Quaternion.LookRotation(mainCamera.transform.forward);

//                 // Update the text details
//                 Transform detailsTransform = currentAGVDetailUI.transform.Find("Details");
//                 if (detailsTransform != null)
//                 {
//                     TMP_Text detailsText = detailsTransform.GetComponent<TMP_Text>();
//                     if (detailsText != null)
//                     {
//                         detailsText.text = FormatJsonForDisplay(currentFocusedAgv);
//                     }
//                 }
//             }

//             yield return new WaitForSeconds(0.1f); 
//         }
//     }

//     private string FormatJsonForDisplay(GameObject agv)
//     {
//         AgvData agvData = agv.GetComponent<AgvData>();
//         if (agvData == null)
//         {
//             Debug.LogError("AgvData component not found on AGV object!");
//             return "No Data Available";
//         }

//         string agvMessage = agvData.GetAgvData();
//         AgvData.AgvMessage message = JsonUtility.FromJson<AgvData.AgvMessage>(agvMessage);

//         string formattedText = $"AGV ID: {message.agv_id}\n" +
//                                $"Location: {message.agv_location}\n" +
//                                $"Segment: {message.agv_segment}\n" +
//                                $"Reading: {message.agv_status}\n" +
//                                $"Timestamp: {message.segment_timestamp}";

//         return formattedText;
//     }
// }












































// using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;
// using TMPro;

// public class AgvView : MonoBehaviour
// {
//     private Camera mainCamera;
//     public float cameraSpeed = 5f;
//     public List<GameObject> AGVs = new List<GameObject>();
//     public int currentAgvIndex = 0;
//     private Coroutine currentFocusCoroutine;

//     public GameObject agvDetailPrefab;
//     private GameObject currentAGVDetailUI;
//     private GameObject currentFocusedAgv;

//     private bool isFocusing = false;

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
//         UpdateAgvList();

//         if (AGVs.Count == 0)
//         {
//             return;
//         }

//         // Switch to the next AGV when '3' is pressed
//         if (Input.GetKeyDown(KeyCode.Alpha3) && AGVs.Count > 0)
//         {
//             currentAgvIndex = (currentAgvIndex + 1) % AGVs.Count;

//             // Stop the current coroutine (if any) before starting a new one
//             if (currentFocusCoroutine != null)
//             {
//                 StopCoroutine(currentFocusCoroutine);
//             }

//             // Start focusing the camera on the selected AGV
//             currentFocusCoroutine = StartCoroutine(FocusOnAgv(AGVs[currentAgvIndex].transform));
//         }
//     }

//     private void UpdateAgvList()
//     {
//         GameObject[] allAgvs = GameObject.FindGameObjectsWithTag("AGV");

//         foreach (GameObject agv in allAgvs)
//         {
//             if (!AGVs.Contains(agv))
//             {
//                 AGVs.Add(agv);
//             }
//         }

//         // Remove null entries (in case AGVs are destroyed)
//         AGVs.RemoveAll(agv => agv == null);
//     }

//     private IEnumerator FocusOnAgv(Transform agvTransform)
//     {
//         isFocusing = true;
//         currentFocusedAgv = agvTransform.gameObject;

//         // Show AGV details UI
//         ShowAgvDetails(currentFocusedAgv);

//         while (isFocusing && currentFocusedAgv != null)
//         {
//             Vector3 targetPosition = agvTransform.position + agvTransform.forward * -5 + Vector3.up * 4;

//             // Smoothly move the camera to the target position
//             mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, Time.deltaTime * cameraSpeed);

//             // Make the camera look at the AGV
//             mainCamera.transform.LookAt(agvTransform.position);

//             // Update UI position based on camera movement
//             UpdateAGVDetailUIPosition();

//             yield return null;
//         }

//         // Final adjustments if necessary
//         if (currentFocusedAgv != null)
//         {
//             Vector3 finalPosition = agvTransform.position + agvTransform.forward * -5 + Vector3.up * 3;
//             mainCamera.transform.position = finalPosition;
//             mainCamera.transform.LookAt(agvTransform.position);

//             // Ensure the UI updates once more after camera stops moving
//             UpdateAGVDetailUIPosition();
//         }
//     }

//     private void ShowAgvDetails(GameObject agv)
//     {
//         if (currentAGVDetailUI != null)
//         {
//             Destroy(currentAGVDetailUI);
//         }

//         GameObject guiCanva = GameObject.Find("GUICanva");
//         if (guiCanva == null)
//         {
//             Debug.LogError("GUICanva object not found in the scene!");
//             return;
//         }

//         if (agvDetailPrefab != null)
//         {
//             currentAGVDetailUI = Instantiate(agvDetailPrefab, guiCanva.transform);
//             currentAGVDetailUI.transform.position = mainCamera.transform.position + mainCamera.transform.forward * 4;
//             currentAGVDetailUI.transform.rotation = Quaternion.LookRotation(mainCamera.transform.forward);
//             currentAGVDetailUI.name = "AGV Detail UI";
//         }
//     }

//     private void UpdateAGVDetailUIPosition()
//     {
//         if (currentAGVDetailUI != null && currentFocusedAgv != null)
//         {
//             // Update the position of the UI to follow the AGV
//             Vector3 uiPosition = currentFocusedAgv.transform.position + currentFocusedAgv.transform.forward * -3 + Vector3.up * 2.5f;
//             currentAGVDetailUI.transform.position = uiPosition;

//             // Ensure the UI faces the camera
//             currentAGVDetailUI.transform.rotation = Quaternion.LookRotation(mainCamera.transform.forward);

//             // Update the text details
//             Transform detailsTransform = currentAGVDetailUI.transform.Find("Details");
//             if (detailsTransform != null)
//             {
//                 TMP_Text detailsText = detailsTransform.GetComponent<TMP_Text>();
//                 if (detailsText != null)
//                 {
//                     detailsText.text = FormatJsonForDisplay(currentFocusedAgv);
//                 }
//             }
//         }
//     }

//     private string FormatJsonForDisplay(GameObject agv)
//     {
//         AgvData agvData = agv.GetComponent<AgvData>();
//         if (agvData == null)
//         {
//             Debug.LogError("AgvData component not found on AGV object!");
//             return "No Data Available";
//         }

//         string agvMessage = agvData.GetAgvData();
//         AgvData.AgvMessage message = JsonUtility.FromJson<AgvData.AgvMessage>(agvMessage);

//         string formattedText = $"AGV ID: {message.agv_id}\n" +
//                                $"Location: {message.agv_location}\n" +
//                                $"Segment: {message.agv_segment}\n" +
//                                $"Reading: {message.agv_status}\n" +
//                                $"Timestamp: {message.segment_timestamp}";

//         return formattedText;
//     }
// }










































using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class AgvView : MonoBehaviour
{
    private Camera mainCamera;
    public float cameraSpeed = 5f;
    public List<GameObject> AGVs = new List<GameObject>();
    public int currentAgvIndex = 0;
    private Coroutine currentFocusCoroutine;

    public GameObject agvDetailPrefab;
    private GameObject currentAGVDetailUI;
    private GameObject currentFocusedAgv;

    private bool isFocusing = false;

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
        UpdateAgvList();

        if (AGVs.Count == 0)
        {
            return;
        }

        // Switch to the next AGV when '3' is pressed
        if (Input.GetKeyDown(KeyCode.Alpha3) && AGVs.Count > 0  && !Input.GetKeyDown(KeyCode.Space) && !Input.GetKeyDown(KeyCode.Alpha2) && !Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentAgvIndex = (currentAgvIndex + 1) % AGVs.Count;

            // Stop the current coroutine (if any) before starting a new one
            if (currentFocusCoroutine != null)
            {
                StopCoroutine(currentFocusCoroutine);
            }

            // Start focusing the camera on the selected AGV
            currentFocusCoroutine = StartCoroutine(FocusOnAgv(AGVs[currentAgvIndex].transform));
        }
        // If any other key is pressed, unfocus the camera and destroy the UI
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Alpha2))
        {
            // Stop focusing the camera
            StopFocusing(mainCamera.transform.position);
            // StopFocusing();
        }
    }

    private void UpdateAgvList()
    {
        GameObject[] allAgvs = GameObject.FindGameObjectsWithTag("AGV");

        foreach (GameObject agv in allAgvs)
        {
            if (!AGVs.Contains(agv))
            {
                AGVs.Add(agv);
            }
        }

        // Remove null entries (in case AGVs are destroyed)
        AGVs.RemoveAll(agv => agv == null);
    }

    private IEnumerator FocusOnAgv(Transform agvTransform)
    {
        isFocusing = true;
        currentFocusedAgv = agvTransform.gameObject;

        // Show AGV details UI
        ShowAgvDetails(currentFocusedAgv);

        while (isFocusing && currentFocusedAgv != null)
        {
            Vector3 targetPosition = agvTransform.position + agvTransform.forward * -5 + Vector3.up * 4;

            // Smoothly move the camera to the target position
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, Time.deltaTime * cameraSpeed);

            // Make the camera look at the AGV
            mainCamera.transform.LookAt(agvTransform.position);

            // Update UI position based on camera movement
            UpdateAGVDetailUIPosition();

            yield return null;
        }

        // Final adjustments if necessary
        if (currentFocusedAgv != null)
        {
            Vector3 finalPosition = agvTransform.position + agvTransform.forward * -5 + Vector3.up * 3;
            mainCamera.transform.position = finalPosition;
            mainCamera.transform.LookAt(agvTransform.position);

            // Ensure the UI updates once more after camera stops moving
            UpdateAGVDetailUIPosition();
        }
    }

    private void ShowAgvDetails(GameObject agv)
    {
        if (currentAGVDetailUI != null)
        {
            Destroy(currentAGVDetailUI);
        }

        GameObject guiCanva = GameObject.Find("GUICanva");
        if (guiCanva == null)
        {
            Debug.LogError("GUICanva object not found in the scene!");
            return;
        }

        if (agvDetailPrefab != null)
        {
            currentAGVDetailUI = Instantiate(agvDetailPrefab, guiCanva.transform);
            currentAGVDetailUI.transform.position = mainCamera.transform.position + mainCamera.transform.forward * 4;
            currentAGVDetailUI.transform.rotation = Quaternion.LookRotation(mainCamera.transform.forward);
            currentAGVDetailUI.name = "AGV Detail UI";
        }
    }

    private void UpdateAGVDetailUIPosition()
    {
        if (currentAGVDetailUI != null && currentFocusedAgv != null)
        {
            // Update the position of the UI to follow the AGV
            Vector3 uiPosition = currentFocusedAgv.transform.position + currentFocusedAgv.transform.forward * -3 + Vector3.up * 2.5f;
            currentAGVDetailUI.transform.position = uiPosition;

            // Ensure the UI faces the camera
            currentAGVDetailUI.transform.rotation = Quaternion.LookRotation(mainCamera.transform.forward);

            // Update the text details
            Transform detailsTransform = currentAGVDetailUI.transform.Find("Details");
            if (detailsTransform != null)
            {
                TMP_Text detailsText = detailsTransform.GetComponent<TMP_Text>();
                if (detailsText != null)
                {
                    detailsText.text = FormatJsonForDisplay(currentFocusedAgv);
                }
            }
        }
    }

    private string FormatJsonForDisplay(GameObject agv)
    {
        AgvData agvData = agv.GetComponent<AgvData>();
        if (agvData == null)
        {
            Debug.LogError("AgvData component not found on AGV object!");
            return "No Data Available";
        }

        string agvMessage = agvData.GetAgvData();
        AgvData.AgvMessage message = JsonUtility.FromJson<AgvData.AgvMessage>(agvMessage);

        string formattedText = $"AGV ID: {message.agv_id}\n" +
                               $"Location: {message.agv_location}\n" +
                               $"Segment: {message.agv_segment}\n" +
                               $"Reading: {message.agv_status}\n" +
                               $"Timestamp: {message.segment_timestamp}";

        return formattedText;
    }

    // Unfocus the camera and destroy the UI
    private void StopFocusing(Vector3 currentCameraPosition)
    {
        isFocusing = false;

        // Destroy the AGV detail UI if it's displayed
        if (currentAGVDetailUI != null)
        {
            Destroy(currentAGVDetailUI);
            currentAGVDetailUI = null;
        }

        mainCamera.transform.position = currentCameraPosition;  
        mainCamera.transform.LookAt(Vector3.zero); 
    }
}
