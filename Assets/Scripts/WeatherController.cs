using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;




public class WeatherController : MonoBehaviour
{
    public int weatherUpdateTimer = 1; // Time in minutes between weather updates
    private WeatherManager weatherManager;
    private float cloudNormalized = 1.0f;
    //[SerializeField] float newExposureWeight = 1.0f; 

    [Range(0f, 1.1f)] public float SunlightNormalized;
    

    
    public Volume globalVolumeProfile;
    public Material skyMaterial;
    
    void Start()
    {
        weatherManager = FindObjectOfType<WeatherManager>();
        

        skyMaterial.EnableKeyword("_EmissiveExposureWeight");



        
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(GetWeatherData());
        
    }

    IEnumerator GetWeatherData()
    {
        yield return new WaitForSeconds(weatherUpdateTimer);
        cloudNormalized = weatherManager.NormalizedCloudiness;
        SetFixedExposure(cloudNormalized);

        SetEmission(SunlightNormalized);
        
    
    }


    private void SetFixedExposure(float exposureValue)
    {
        if (globalVolumeProfile.profile.TryGet<Exposure>(out var exposure))
        {
            exposure.fixedExposure.overrideState = true;
            exposure.fixedExposure.value = 11.0f- (11.0f-7.0f)*exposureValue;
        }
        // else
        // {
        //     // If Exposure override doesn't exist, add it
        //     exposure = globalVolumeProfile.profile.Add<Exposure>();
        //     exposure.fixedExposure.overrideState = true;
        //     exposure.fixedExposure.value = exposureValue;
        // }
    }


        private void SetEmission(float exposureValue)
    {
        //skyMaterial.SetFloat("_ExposureWeight", newExposureWeight);
        //skyMaterial.SetFloat("_EmissiveIntensity", newExposureWeight);
        skyMaterial.SetFloat("_EmissiveExposureWeight", 1.1f-exposureValue);
        

    }
}
