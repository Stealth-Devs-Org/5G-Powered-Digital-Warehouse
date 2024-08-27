using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class WeatherController : MonoBehaviour
{
    [SerializeField] GameObject GlobalVolume; // Reference to the Global Volume in the scene
    [SerializeField] Material skyboxManual; // Reference to the skybox material

    private float fullIntensity; // Stores the initial intensity for lights
    private Exposure exposureComponent; // Reference to the Exposure component in Global Volume

    public string cityName = "Kalmunai,LK"; // City name for weather data

    void OnEnable()
    {
        Messenger.AddListener(GameEvent.WEATHER_UPDATED, OnWeatherUpdated); // Subscribes to weather updates
    }

    void OnDisable()
    {
        Messenger.RemoveListener(GameEvent.WEATHER_UPDATED, OnWeatherUpdated); // Unsubscribes from weather updates
    }

    void Start()
    {
        // Get the Exposure component from the Global Volume
        if (GlobalVolume.TryGetComponent<Volume>(out Volume volume))
        {
            if (volume.profile.TryGet<Exposure>(out exposureComponent))
            {
                fullIntensity = exposureComponent.limitMin.value; // Store the initial limit min value
            }
        }
    }

    private void OnWeatherUpdated()
    {
        // Update the overcast settings based on the weather manager's cloud value
        SetOvercast(Managers.Weather.cloudValue);
    }

    private void SetOvercast(float value)
    {
        if (exposureComponent != null)
        {
            // Adjust the limit min value based on the cloud cover
            float adjustedIntensity = fullIntensity - (fullIntensity * value);
            exposureComponent.limitMin.value = adjustedIntensity;
        }
    }
}
