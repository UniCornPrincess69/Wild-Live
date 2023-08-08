using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Threading;
using UnityEngine;
using UnityEngine.Profiling;

[RequireComponent(typeof(Terrain))]
public class TerrainGeneration : MonoBehaviour
{
    private Terrain _terrain = null;
    [SerializeField] private int _chunkAmount = 4;
    private int _terrainSize = 0;
    private Vector2 _terrainPos = Vector2.zero; 



    private void Awake()
    {
        _terrain = GetComponent<Terrain>();
        _terrainSize = _terrain.terrainData.heightmapResolution;
        _terrainPos = new Vector2(transform.position.x, transform.position.z);
    }


    //TODO: Before and after Thread screenshots
    public void GenerateTerrain(float[,] heights)
    {
        Profiler.BeginSample("Terrain Gen " + gameObject.name);

        //float[,] heights = new float[_terrainSize, _terrainSize];
        //for (int y = 0; y < _terrainSize; y++)
        //{
        //    for (int x = 0; x < _terrainSize; x++)
        //    {
        //        var noiseVal = GetHeightVal(x, y);
        //        var heightMapVal = 0f;
        //        if (_useHeightmap) heightMapVal = GetHeighmapVal(x, y);
        //        heights[x, y] = noiseVal;
        //    }
        //}

        //TODO: Multithreaded recalculation of the heights
        var heightsReduced = new float[_terrainSize, _terrainSize];
        var offset = new Vector2Int(Mathf.RoundToInt(_terrainPos.x / 2), Mathf.RoundToInt(_terrainPos.y / 2));
        var max = 0;
        for (int y = 0; y < _terrainSize; y++)
        {
            for (int x = 0; x < _terrainSize; x++)
            {
                if (x + offset.x > max) max = x + offset.x;
                heightsReduced[y, x] = heights[x + offset.x, y + offset.y];
            }
        }
        Debug.Log($"{max}: {heights[max, heights.GetLength(1)-1]}");

        //_terrain.terrainData.SetHeights((int)_terrainPos.x / 2, (int)_terrainPos.y / 2, heigts);
        _terrain.terrainData.SetHeights(0, 0, heightsReduced);
        Profiler.EndSample();
    }


    //private float GetHeighmapVal(float x, float y)
    //{
    //    var result = 0.0f;

    //    Color color = _texture.GetPixel((int)x, (int)y);

    //    result = color.grayscale * _heightMapIntensity;

    //    return result;
    //}

    //private float GetHeightVal(float x, float y)
    //{
    //    // calculating the Pixel index of the PerlinNoise texture using the World position of the Vertex
    //    var xCoord = x / (_terrainSize * 2f) + _offsetCoord.y;
    //    var yCoord = y / (_terrainSize * 2f) + _offsetCoord.x;

    //    // adding a offset for the PerlinNoisePixel if the terrains are moved
    //    xCoord -= (_offsetCoord.y > 0 ? 1f / 1026f : 0f);
    //    yCoord -= (_offsetCoord.x > 0 ? 1f / 1026f : 0f);


    //    float scale = _scale;
    //    float intensity = 1f;
    //    float maxHeight = 0f;
    //    float noiseVal = 0f;

    //    for (int i = 0; i < _octaveCount; i++)
    //    {
    //        noiseVal += Mathf.PerlinNoise(xCoord * scale + _offset.x, yCoord * scale + _offset.y) * intensity;
    //        maxHeight += intensity;

    //        scale *= 2;
    //        intensity *= 0.5f;
    //    }

    //    return noiseVal / maxHeight;
    //}

   
}
