using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Terrain))]
public class TerraingenByStephan : MonoBehaviour
{
    private Terrain ter = null;
    [SerializeField] private Texture2D tex = null;
    [SerializeField] private float scale = 1f;
    [SerializeField] private string seed = string.Empty;
    private Vector2 offset = Vector2.zero;
    [SerializeField] private bool generateTexture = false;
    [SerializeField] private int octaveCount = 1;
    [SerializeField] private float coordOffset = 0f;

    private void Awake()
    {
        ter = GetComponent<Terrain>();
    }

    void Start()
    {
        Random.InitState(seed.GetHashCode());
        offset = new Vector2(Random.Range(0, 100_000), Random.Range(0, 100_000));
        Generate();
    }

    private void Update()
    {
    }

    [ContextMenu("Generate")]
    private void Generate()
    {
        var size = ter.terrainData.heightmapResolution;
        float[,] heights = new float[size, size];
        Debug.Log(size);

        for (int y = 0; y < size ; y++)
        {
            for (int x = 0; x < size; x++)
            {
                var noiseVal = GetHeightVal(x, y);

                if (generateTexture)
                {
                    var col = new Color(noiseVal, noiseVal, noiseVal, 1);
                    tex.SetPixel(x, y, col);
                }

                heights[x, y] = noiseVal;
            }
        }

        if (generateTexture) tex.Apply();

        ter.terrainData.SetHeights(0, 0, heights);
    }

    private float GetHeightVal(float x, float y)
    {
        //var xCoord = x / tex.width * scale + offset.x;
        //var yCoord = y / tex.height * scale + offset.y;
        var xCoord = x / tex.width;
        var yCoord = y / tex.height * 0.5f + coordOffset;

        float noiseVal = 0f;
        float scale = this.scale;
        float intensity = 1f;
        float maxHeight = 0f;

        for (int i = 0; i < octaveCount; i++)
        {
            noiseVal += Mathf.PerlinNoise(xCoord * scale + offset.x, yCoord * scale + offset.y) * intensity;
            maxHeight += intensity;

            scale *= 2;
            intensity *= 0.5f;
        }

        return noiseVal / maxHeight;
    }
}