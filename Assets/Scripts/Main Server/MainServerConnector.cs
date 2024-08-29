using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class AGVClient : MonoBehaviour
{
    private string serverUrl = "http://localhost:5000";

    public void SendAGVLocation(string agvId, string location, string segment, int status, string timestamp)
    {
        StartCoroutine(PostAGVLocation(agvId, location, segment, status, timestamp));
    }

    private IEnumerator PostAGVLocation(string agvId, string location, string segment, int status, string timestamp)
    {
        var agvData = new
        {
            agv_id = agvId,
            location = location,
            segment = segment,
            status = status,
            timestamp = timestamp
        };

        string json = JsonUtility.ToJson(agvData);
        var request = new UnityWebRequest(serverUrl + "/update_agv_location", "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

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

    public void GetAGVLocations()
    {
        StartCoroutine(GetAGVLocationsCoroutine());
    }

    private IEnumerator GetAGVLocationsCoroutine()
    {
        var request = UnityWebRequest.Get(serverUrl + "/get_agv_locations");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            Debug.Log("Response: " + request.downloadHandler.text);
            // Process the received data here
        }
    }
}
