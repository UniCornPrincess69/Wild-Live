using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherSystem : MonoBehaviour
{
    [SerializeField]
    private Material _grass = null;
    [SerializeField]
    private Material _ground = null;
    [SerializeField]
    private Material _water = null;

    private WeatherCondition _conditition;
    private float _windStrength = 0f;
    private int _rng = 0;


    private enum WeatherCondition { Clear, Windy, Rainy, Stormy }


    private void Awake()
    {
        GameWorldManager.Instance.WeatherSystem = this;
        _rng = Random.Range(0, 3);
        _conditition = (WeatherCondition)_rng;
    }

    private void Update()
    {
        ChangeWeather(_conditition);
    }

    private void ChangeWeather(WeatherCondition condition)
    {
        switch (condition)
        {
            case WeatherCondition.Clear:
                _windStrength = 2.5f;
                break; 

            case WeatherCondition.Windy:
                break;

            case WeatherCondition.Rainy:
                break;

            case WeatherCondition.Stormy:
                _windStrength = 10f;
                _grass.SetFloat("_Wind_Strength", _windStrength);
                break;
        }
    }
}
