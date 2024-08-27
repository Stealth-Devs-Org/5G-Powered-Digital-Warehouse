using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class WeatherManager : MonoBehaviour
{
    private string apiKey = "46642be04341c1815b314eba42613bff"; // Replace with your API key
    public string cityName = "Kalmunai,LK"; // Replace with your city name
    private string apiUrl;

    private void Start()
    {
        apiUrl = $"https://api.openweathermap.org/data/2.5/weather?q={cityName}&appid={apiKey}&units=metric"; // Metric units for temperature in Celsius
        StartCoroutine(GetWeatherData());
    }

    private IEnumerator GetWeatherData()
    {
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
                Debug.Log("City: " + cityName);
                Debug.Log("Weather: " + weatherResponse.weather[0].description);
                Debug.Log("Temperature: " + weatherResponse.main.temp + "°C");
                Debug.Log("Cloudiness: " + weatherResponse.clouds.all + "%");
                Debug.Log("Sunrise: " + UnixTimeStampToDateTime(weatherResponse.sys.sunrise).ToString("HH:mm:ss"));
                Debug.Log("Sunset: " + UnixTimeStampToDateTime(weatherResponse.sys.sunset).ToString("HH:mm:ss"));


                float Normalized_cloudiness = weatherResponse.clouds.all/100;
                float tempreature = weatherResponse.main.temp;
            

            }
        }
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
