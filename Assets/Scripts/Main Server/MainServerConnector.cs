using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class TestFlaskClient : MonoBehaviour
{
    private string serverUrl = "http://localhost:5000";

    void Start()
    {
        // Test connection to the /test endpoint
        StartCoroutine(GetTestMessage());
    }

    private IEnumerator GetTestMessage()
    {
        var request = UnityWebRequest.Get(serverUrl + "/test");

        Debug.Log("Sending request to /test");  // Debug log
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            Debug.Log("Response: " + request.downloadHandler.text);
        }
    }
}
