using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultTerrain", menuName = "ScriptableObjects/TerrainData")]
public class TerrainData :ScriptableObject
{
    [field: SerializeField]
    public int MapSize { get; set; } = 100;
    [field: SerializeField]
    public int Resolution { get; set; } = 50;
    [field:SerializeField]
    public Vector3 TerrainPosition { get; set; } = Vector3.zero;
    [field: SerializeField]
    public bool IncludeNoise { get; set; } = false;
    [field: SerializeField]
    public float NoiseIntensity { get; set; } = 0f;
    [field: SerializeField]
    public Texture2D Heightmap { get; set; } = null;
    [field: SerializeField]
    public Material Material { get; set; } = null;

}
