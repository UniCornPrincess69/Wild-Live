using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class WeatherSystem : MonoBehaviour
{
    [SerializeField]
    private Material _grass = null;
    [SerializeField]
    private Material _ground = null;
    [SerializeField]
    private Material _water = null;
    [SerializeField]
    private List<GameObject> _vfx = null;
    [SerializeField]
    private List<string> _list = new List<string>();

    private WeatherCondition _previousCondition = WeatherCondition.None;
    [SerializeField]
    private WeatherCondition _condition = WeatherCondition.None;

    [SerializeField]
    private WeatherValues _values = null;


    private int _rng = 0;

    private Dictionary<WeatherCondition, Dictionary<string, float>> _conditions = null;
    private Dictionary<string, dynamic> _stormy = null;
    private Dictionary<string, dynamic> _clear = null;
    private Dictionary<string, dynamic> _windy = null;
    private Dictionary<string, dynamic> _rainy = null;


    private Dictionary<string, dynamic> _properties = null;


    private enum WeatherCondition { Clear, Windy, Rainy, Stormy, None }


    private void Awake()
    {
        GameWorldManager.Instance.WeatherSystem = this;
        _rng = Random.Range(0, 4);
        _condition = (WeatherCondition)_rng;

        _stormy = new Dictionary<string, dynamic>()
        {
            { nameof(_values.WindStrength), _values.WindStrength },

        };
        _properties = new Dictionary<string, dynamic>()
        {
            { _list[0], _values.WindStrength },
            { _list[1], _values.CausticSpeed },
            { _list[2], _values.Wetness },
            { _list[3], _values.WindStrength },
            { _list[4], _values.CloudSpawn },
            { _list[5], _values.FlameRate },
            { _list[6], _values.FlameSize },
            { _list[7], _values.SmokeRate },
            { _list[8], _values.RainSpawnRate },
            { _list[9], _values.RainStretch },
            { _list[10], _values.CloudColor },
        }
        ;
    }

    private void Start()
    {
        _values.Wetness = 100f;
        ChangeWeather(_condition);
    }

    private void Update()
    {


    }

    [ContextMenu("WeatherChange")]
    private void ChangeWeather(WeatherCondition condition)
    {
        if (condition == _previousCondition) return;

        if (condition == WeatherCondition.Stormy) _values.ThunderIsPossible = true;

        if (condition == WeatherCondition.Clear || condition == WeatherCondition.Windy &&
            _previousCondition == WeatherCondition.Rainy || _previousCondition == WeatherCondition.Stormy)
        {
            float timer = 0f;
            float step = 0.02f;
            while (_values.Wetness > 0f)
            {
                _values.Wetness = Mathf.Lerp(_values.Wetness, 0f, timer);
                if (condition == WeatherCondition.Clear) _values.WindStrength = Mathf.Lerp(_values.WindStrength, 2.5f, timer);
                else _values.WindStrength = Mathf.Lerp(_values.WindStrength, 10f, timer);
                SetProperties();
                timer += step;
            }


        }

        switch (condition)
        {
            case WeatherCondition.Clear:
                _values.WindStrength = 2.5f;
                _values.CausticSpeed = 2f;
                _values.Wetness = 0f;
                _values.CloudSpawn = 500f;
                _values.FlameRate = 0f;
                _values.FlameSize = 0f;
                _values.SmokeRate = 0f;
                _values.RainSpawnRate = 0f;
                _values.RainStretch = 0f;
                break;

            case WeatherCondition.Windy:
                _values.WindStrength = 10f;
                break;

            case WeatherCondition.Rainy:
                break;

            case WeatherCondition.Stormy:
                _values.WindStrength = 10f;
                _grass.SetFloat("_Wind_Strength", _values.WindStrength);
                break;
        }
        Debug.Log(condition);
        _previousCondition = condition;
    }

    [ContextMenu("SetProperties")]
    private void SetProperties()
    {
        return;
    }


    //TODO: Create all needed dictionaries. Property names via list
    //Set properties in correlation to the dictionary
    private Dictionary<dynamic, dynamic> CreateDictionary()
    {
        var result = new Dictionary<dynamic, dynamic>();
        return result;
    }
}
