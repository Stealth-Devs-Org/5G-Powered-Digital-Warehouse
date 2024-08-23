using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class NetworkService {

    WeatherController weatherController = GameObject.FindObjectOfType<WeatherController>();
    
    
    private string cityName;
    private string xmlApi;



    private IEnumerator CallAPI(string url, Action<string> callback) 
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url)) {

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            
        } 
        
        else if (request.result == UnityWebRequest.Result.ProtocolError) 
        {
            Debug.LogError($"response error: {request.responseCode}");
        } 
        
        else 
        {
            callback(request.downloadHandler.text);
        }

        }
    }

    public IEnumerator GetWeatherXML(Action<string> callback) {
    cityName = weatherController.cityName;
    xmlApi ="https://api.openweathermap.org/data/2.5/weather?q="+cityName+"&mode=xml&appid=46642be04341c1815b314eba42613bff";
    return CallAPI(xmlApi, callback);
    }
}