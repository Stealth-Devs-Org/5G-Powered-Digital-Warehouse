using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Runtime.CompilerServices;


// Mapping between enum values and city names

public class WeatherManager : MonoBehaviour
{
    private string apiKey = "46642be04341c1815b314eba42613bff"; 
    private string apiUrl;

    [Range(0f, 1f)] public float CloudinessNormalized;
    [Range(28f, 40f)] public float tempreature;
    [Range(0f, 1.1f)] public float SunlightNormalized;

    private string cityName;

    public enum City
    {
        Kalmunai,
        Moratuwa,
        Kandy,
        Colombo,
        Ampara,
        Galle,
        Jaffna,
        London
    }

    public City selectedCity = City.Kalmunai;

    private string GetCityName(City city)
    {
        switch (city)
        {
            case City.Kalmunai:
                return "Kalmunai,LK";
            case City.Moratuwa:
                return "Moratuwa,LK";
            case City.Kandy:
                return "Kandy,LK";
            case City.Colombo:
                return "Colombo,LK";
            case City.Ampara:
                return "Ampara,LK";
            case City.Galle:
                return "Galle,LK";
            case City.Jaffna:
                return "Jaffna,LK";
            case City.London:
                return "London,UK";
            default:
                return "Kalmunai,LK";
        }
    }


    void Update()
    {
        cityName = GetCityName(selectedCity);
        StartCoroutine(GetWeatherData());

        //Debug.Log(CloudinessNormalized);
    }

    private IEnumerator GetWeatherData() 
    {   
        apiUrl = $"https://api.openweathermap.org/data/2.5/weather?q={cityName}&appid={apiKey}&units=metric";
        using (UnityWebRequest www = UnityWebRequest.Get(apiUrl))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + www.error);
            }
            else
            {
                // Parse the JSON data
                string jsonResponse = www.downloadHandler.text;
                WeatherResponse weatherResponse = JsonUtility.FromJson<WeatherResponse>(jsonResponse);

                // Display all relevant weather data
                //Debug.Log("City: " + cityName);
                //Debug.Log("Weather: " + weatherResponse.weather[0].description);
                //Debug.Log("Temperature: " + weatherResponse.main.temp + "°C");
                //Debug.Log("Cloudiness: " + weatherResponse.clouds.all + "%");
                // Debug.Log("Sunrise: " + UnixTimeStampToDateTime(weatherResponse.sys.sunrise).ToString("HH:mm:ss"));
                //Debug.Log("Sunset: " + UnixTimeStampToDateTime(weatherResponse.sys.sunset).ToString("HH:mm:ss"));


                CloudinessNormalized  = weatherResponse.clouds.all/100.0f;
                tempreature = weatherResponse.main.temp;
            

            }
        }

        yield return new WaitForSeconds(2);
    }

    private System.DateTime UnixTimeStampToDateTime(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        System.DateTime dtDateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
        return dtDateTime;
    }

    [System.Serializable]
    public class WeatherResponse
    {
        public Weather[] weather;
        public Main main;
        
        public Clouds clouds;
        public Sys sys;
    }

    [System.Serializable]
    public class Weather
    {
        public string description;
    }

    [System.Serializable]
    public class Main
    {
        public float temp;
        
    }

    [System.Serializable]
    public class Clouds
    {
        public int all;
    }

    [System.Serializable]
    public class Sys
    {
        public double sunrise;
        public double sunset;
    }
}

