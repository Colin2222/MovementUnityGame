using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneWeatherManager : MonoBehaviour
{
    public enum WeatherType { None, Rain }
    public SceneManager sceneManager;
    public GameObject rainAudioPrefab;
    public WeatherType potentialWeatherType = WeatherType.None;
    [System.NonSerialized]
    public WeatherType currentWeatherType = WeatherType.None;
    public GameObject particleSystem;
    // Start is called before the first frame update
    void Start()
    {
        if (potentialWeatherType == WeatherType.None)
        {
            currentWeatherType = WeatherType.None;
            particleSystem.SetActive(false);
            return;
        }

        int currentWeatherInt = SessionManager.Instance.GetIntegerMarker("weather_marker");
        currentWeatherType = (WeatherType)currentWeatherInt;

        if (currentWeatherType == WeatherType.None)
        {
            particleSystem.SetActive(false);
        }
        else
        {
            particleSystem.SetActive(true);
            if (sceneManager.vcam == null)
            {
                Instantiate(rainAudioPrefab, GameObject.FindWithTag("MainCamera").transform).transform.localPosition = Vector3.zero;
            }
            else
            {
                Instantiate(rainAudioPrefab, sceneManager.vcam.transform).transform.localPosition = Vector3.zero; 
            }
        }
    }
}
