using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks.Sources;
using UnityEngine;
using UnityEngine.VFX;

public class WeatherSystem : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _effects = null;
    [SerializeField]
    private List<string> _list = new List<string>();

    private WeatherCondition _previousCondition = WeatherCondition.None;
    private WeatherCondition _condition = WeatherCondition.None;

    [SerializeField]
    private WeatherValues _values = null;

    private int _rng = 0;

    private Dictionary<string, dynamic> _properties = null;
    private FireValues fireValues = new FireValues();

    private enum WeatherCondition { Clear, Windy, Rainy, Stormy, None }

    public event System.Action<FireValues> OnFireValuesChanged = val => { };


    //Example from Stephan to have a "Dictionary" in the Inspector
    //[System.Serializable] struct Pair<T1, T2> { [SerializeField] private string name; [SerializeField] private T1 first; [SerializeField] private T2 second; public T1 First => first; public T2 Second => second; }
    //[SerializeField] private List<Pair<WeatherCondition, WeatherValues>> valuesList = new();
    //private Dictionary<WeatherCondition, WeatherValues> values = new();

    //private void OnEnable()
    //{
    //    for (int i = 0; i < valuesList.Count; i++)
    //    {
    //        values.Add(valuesList[i].First, valuesList[i].Second);
    //    }

    //    valuesList.Clear();
    //}


    private void Awake()
    {
        GameWorldManager.Instance.WeatherSystem = this;
        _rng = Random.Range(0, 4);
        _condition = (WeatherCondition)_rng;

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

        //testEffect.SetFloat("FlameRate", 1f);
    }

    private void Update()
    {
        fireValues.FlameRate = Time.time;
        OnFireValuesChanged.Invoke(fireValues);
    }

    [ContextMenu("WeatherChange")]
    private void ChangeWeather(WeatherCondition condition)
    {
        if (condition == _previousCondition) return;

        if (condition == WeatherCondition.Stormy) _values.ThunderIsPossible = true;

        if ((condition == WeatherCondition.Clear || condition == WeatherCondition.Windy) &&
            (_previousCondition == WeatherCondition.Rainy || _previousCondition == WeatherCondition.Stormy))
        {
            IEnumerator SwitchToDry()
            {
                float timer = 0f;
                float step = 0.02f;
                while (_values.Wetness > 0f)
                {
                    _values.Wetness = Mathf.Lerp(_values.Wetness, 0f, timer);
                    if (condition == WeatherCondition.Clear) _values.WindStrength = Mathf.Lerp(_values.WindStrength, 2.5f, timer);
                    else _values.WindStrength = Mathf.Lerp(_values.WindStrength, 10f, timer);
                    SetProperties(null);
                    timer += step;
                    yield return null;
                }
            }

            StartCoroutine(SwitchToDry());

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
                break;
        }
        Debug.Log(condition);
        _previousCondition = condition;
    }

    private void SetProperties(WeatherCondition con)
    {
        return;
    }
    private void SetProperties(WeatherValues vals)
    {
        //waterMat.SetFloat("_CausticDensity", vals.CausticDensity);

        
        
        return;
    }

    public FireValues GetFireValues() => fireValues;

}
