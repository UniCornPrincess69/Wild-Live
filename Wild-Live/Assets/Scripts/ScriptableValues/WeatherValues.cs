using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weather Values", menuName = "ScriptableObjects/Weather Values")]
public class WeatherValues : ScriptableObject
{
    [field: SerializeField]
    public float WindStrength { get; set; } = 0f;
    [field: SerializeField]
    public float CausticSpeed { get; set; } = 0f;
    [field: SerializeField]
    public float Wetness { get; set; } = 0f;
    [field: SerializeField]
    public float CloudSpawn { get; set; } = 0f;
    [field: SerializeField]
    public float RippleIntensity { get; set; } = 0f;
    [field: SerializeField]
    public float FlameRate { get; set; } = 0f;
    [field: SerializeField]
    public float FlameSize { get; set; } = 0f;
    [field: SerializeField]
    public float SmokeRate { get; set; } = 0f;
    [field: SerializeField]
    public float RainSpawnRate { get; set; } = 0f;
    [field: SerializeField]
    public float RainStretch { get; set; } = 0f;
    [field: SerializeField]
    public Vector4 CloudColor { get; set; } = Vector4.zero;
    [field: SerializeField]
    public bool FireIsBurning { get; set; } = false;
    [field: SerializeField]
    public bool ThunderIsPossible { get; set; } = false;
}
