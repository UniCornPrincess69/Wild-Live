using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting;
using UnityEngine;


[RequireComponent(typeof(Terrain))]
public class TerrainGeneration : MonoBehaviour
{
    private Terrain _terrain = null;
    private Vector2 _offset = Vector2.zero;
    [SerializeField] private Texture2D _texture = null;
    [SerializeField] private string _seed = string.Empty;
    [SerializeField] private float _offsetCoord = 0f;
    [SerializeField] private float _octaveCount = 1f;
    [SerializeField] private float _scale = 1f;
    [SerializeField] private bool _useHeightmap = false;
    private int _terrainWidth = 0;
    private int _terrainHeight = 0;



    private void Awake()
    {
        _terrain = GetComponent<Terrain>();
        _terrainWidth = _terrain.terrainData.heightmapResolution;
        _terrainHeight = _terrain.terrainData.heightmapResolution;
    }

    void Start()
    {
        Random.InitState(_seed.GetHashCode());
        _offset = new Vector2(Random.Range(0, 100_000), Random.Range(0, 100_000));
        if (_useHeightmap) TerrainWithHeightmap();
        else GenerateTerrain();
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

    public void GenerateTerrain()
    {
        float[,] heights = new float[_terrainWidth, _terrainHeight];
        for (int y = 0; y < _terrainWidth; y++)
        {
            for (int x = 0; x < _terrainWidth; x++)
            {
                var noiseVal = GetHeightVal(x, y);
                var heightMapVal = 0f;
                if(_useHeightmap) heightMapVal = GetHeighmapVal(x, y);
                heights[x, y] = noiseVal + heightMapVal;
            }
        }
        _terrain.terrainData.SetHeights(0, 0, heights);
    }

    private float GetHeighmapVal(float x, float y)
    {
        var result = 0.0f;

        Color color = _texture.GetPixel((int)x, (int)y);

        result = color.grayscale * 0.1f;

        return result;
    }

    private float GetHeightVal(float x, float y)
    {
        var xCoord = x / _terrainWidth * _scale + _offset.x;
        var yCoord = y / _terrainHeight * _scale + _offset.y;


        float scale = _scale;
        float intensity = 1f;
        float maxHeight = 0f;
        float noiseVal = 0f;

        for (int i = 0; i < _octaveCount; i++)
        {
            noiseVal += Mathf.PerlinNoise(xCoord * scale + _offsetCoord, yCoord * scale + _offsetCoord) * intensity;
            maxHeight += intensity;

            scale *= 2;
            intensity *= 0.5f;
        }

        return noiseVal / maxHeight;
    }
}
