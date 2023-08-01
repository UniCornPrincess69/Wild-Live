using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(VisualEffect))]
public class Fire : MonoBehaviour
{
    private VisualEffect _fire = null;
    
    private IEnumerator Start()
    {
        yield return null;
        _fire = GetComponent<VisualEffect>();
        GameWorldManager.Instance.WeatherSystem.OnFireValuesChanged += SetValues;
        SetValues(GameWorldManager.Instance.WeatherSystem.GetFireValues());
    }

    private void SetValues(FireValues vals)
    {
        _fire.SetFloat("FlameRate", vals.FlameRate);
        _fire.SetFloat("FlameSize", vals.FlameSize);
        _fire.SetFloat("SmokeRate", vals.SmokeRate);
        _fire.SetFloat("SparkRate", vals.SparkRate);
    }

    private void OnDestroy()
    {
        GameWorldManager.Instance.WeatherSystem.OnFireValuesChanged -= SetValues;
    }
}
