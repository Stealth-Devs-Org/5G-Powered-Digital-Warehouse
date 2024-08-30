using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class MainServerAGV : MonoBehaviour
{
    private string url = "http://localhost:5000/AGV_Communications"; 

    void Start()
    {
        //StartCoroutine(GetAGVData());
    }

    void Update()
    {
        StartCoroutine(GetAGVData());
    }

    IEnumerator GetAGVData()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                
                string jsonResponse = webRequest.downloadHandler.text;  // gett JSON data
                AGVsResponse agvsData = JsonUtility.FromJson<AGVsResponse>(jsonResponse);

                
                PrintAGVData(agvsData);
            }
        }
    }

    void PrintAGVData(AGVsResponse agvsData)
    {
        if (agvsData.agv1 != null)
        {
            Debug.Log($"AGV1 ID: {agvsData.agv1.agv_id}, Location: {agvsData.agv1.location[0]}, {agvsData.agv1.location[1]}, Status: {agvsData.agv1.status}, Timestamp: {agvsData.agv1.timestamp}");
        }

        if (agvsData.agv2 != null)
        {
            Debug.Log($"AGV2 ID: {agvsData.agv2.agv_id}, Location: {agvsData.agv2.location[0]}, {agvsData.agv2.location[1]}, Status: {agvsData.agv2.status}, Timestamp: {agvsData.agv2.timestamp}");
        }

        if (agvsData.agv3 != null)
        {
            Debug.Log($"AGV3 ID: {agvsData.agv3.agv_id}, Location: {agvsData.agv3.location[0]}, {agvsData.agv3.location[1]}, Status: {agvsData.agv3.status}, Timestamp: {agvsData.agv3.timestamp}");
        }

        if (agvsData.agv4 != null)
        {
            Debug.Log($"AGV4 ID: {agvsData.agv4.agv_id}, Location: {agvsData.agv4.location[0]}, {agvsData.agv4.location[1]}, Status: {agvsData.agv4.status}, Timestamp: {agvsData.agv4.timestamp}");
        }
    }

[System.Serializable]
public class AGVData
{
    public string agv_id;
    public int[] location;
    public int[][] segment;
    public int status;
    public string timestamp;
}

[System.Serializable]
public class AGVsResponse
{
    public AGVData agv1;
    public AGVData agv2;
    public AGVData agv3;
    public AGVData agv4;
}


}



