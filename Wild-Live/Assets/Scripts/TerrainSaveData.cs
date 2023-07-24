using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TerrainSaveData
{
    public int mapSize = -1;
    public int resolution = -1;
    public float[] positions = null;
    public bool noiseIncluded = false;
    public float noiseIntensity = -1.0f;
    public Texture2D heightmap = null;
    public Material material = null;

    public Vector3 Position => new Vector3(positions[0], positions[1], positions[2]);

    public TerrainSaveData(TerrainData data)
    {
        mapSize = data.MapSize;
        resolution = data.Resolution;
        positions = new float[]
        {
            data.TerrainPosition.x,
            data.TerrainPosition.y,
            data.TerrainPosition.z,
        };
        noiseIncluded = data.IncludeNoise;
        noiseIntensity = data.NoiseIntensity;
        heightmap = data.Heightmap;
        material = data.Material;
    }
}
