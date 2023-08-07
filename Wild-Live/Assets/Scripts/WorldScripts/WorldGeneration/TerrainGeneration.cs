using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Profiling;

[RequireComponent(typeof(Terrain))]
public class TerrainGeneration : MonoBehaviour
{
    private Terrain _terrain = null;
    private Vector2 _offset = Vector2.zero;
    [SerializeField] private Texture2D _texture = null;
    [SerializeField] private string _seed = string.Empty;
    [SerializeField] private Vector2 _offsetCoord = Vector2.zero;
    [SerializeField] private float _octaveCount = 1f;
    [SerializeField] private float _scale = 1f;
    [SerializeField] private bool _useHeightmap = false;
    [SerializeField, Range(0.1f, 1.0f)] private float _heightMapIntensity = 0.1f;
    private int _terrainWidth = 0;
    private int _terrainHeight = 0;



    private void Awake()
    {
        _terrain = GetComponent<Terrain>();
        _terrainWidth = _terrain.terrainData.heightmapResolution;
        _terrainHeight = _terrain.terrainData.heightmapResolution;
        
    }

    IEnumerator Start()
    {
        yield return new WaitForSeconds(1);
        Random.InitState(_seed.GetHashCode());
        _offset = new Vector2(Random.Range(0, 100_000), Random.Range(0, 100_000));
        if (_useHeightmap) TerrainWithHeightmap();
        else
        {
            GenerateTerrain();
        }
    }

    //If a heightmap is used this method is called. Inspired via ChatGPT
    private void TerrainWithHeightmap()
    {
        float[,] heights = new float[_terrainWidth, _terrainHeight];
        Color[] pixels = _texture.GetPixels();
        int index = 0;
        for (int x = 0; x < _terrainWidth - 1; x++)
        {
            for (int y = 0; y < _terrainHeight - 1; y++)
            {
                float grayScale = pixels[index].grayscale;
                heights[x, y] = grayScale * _scale;
                index++;
            }
        }
    }
    //TODO: Before and after Thread screenshots
    public void GenerateTerrain()
    {
        Profiler.BeginSample("Terrain Gen " + gameObject.name);

        float[,] heights = new float[_terrainWidth, _terrainHeight];
        for (int y = 0; y < _terrainWidth; y++)
        {
            for (int x = 0; x < _terrainWidth; x++)
            {
                var noiseVal = GetHeightVal(x, y);
                var heightMapVal = 0f;
                if (_useHeightmap) heightMapVal = GetHeighmapVal(x, y);
                heights[x, y] = noiseVal + heightMapVal;
            }
        }

        _terrain.terrainData.SetHeights(0, 0, heights);
        Profiler.EndSample();
    }

    private float GetHeighmapVal(float x, float y)
    {
        var result = 0.0f;

        Color color = _texture.GetPixel((int)x, (int)y);

        result = color.grayscale * _heightMapIntensity;

        return result;
    }

    private float GetHeightVal(float x, float y)
    {
        // calculating the Pixel index of the PerlinNoise texture using the World position of the Vertex
        var xCoord = x / _terrainWidth * 0.5f + _offsetCoord.y;
        var yCoord = y / _terrainHeight * 0.5f + _offsetCoord.x;

        // adding a offset for the PerlinNoisePixel if the terrains are moved
        xCoord -= (_offsetCoord.y > 0 ? 1f / 2050 : 0);
        yCoord -= (_offsetCoord.x > 0 ? 1f / 2050 : 0);


        float scale = _scale;
        float intensity = 1f;
        float maxHeight = 0f;
        float noiseVal = 0f;

        for (int i = 0; i < _octaveCount; i++)
        {
            noiseVal += Mathf.PerlinNoise(xCoord * scale + _offset.x, yCoord * scale + _offset.y) * intensity;
            maxHeight += intensity;

            scale *= 2;
            intensity *= 0.5f;
        }

        return noiseVal / maxHeight;
    }
}
