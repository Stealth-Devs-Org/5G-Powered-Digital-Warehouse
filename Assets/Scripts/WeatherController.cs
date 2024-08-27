using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherController : MonoBehaviour
{
    public int weatherupdateTimer= 1;
    WeatherManager weatherManager;

    public GameObject GlobalVolume; 
    
    void Start()
    {
        weatherManager = FindObjectOfType<WeatherManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator GetWeatherData()
    {
        yield return new WaitForSeconds(weatherupdateTimer);

        
        


        
    }
}
