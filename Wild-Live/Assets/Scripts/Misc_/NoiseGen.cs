using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public static class EditorNoiseGen
{
    public static float[,] GenerateNoiseMap(int width, int height, float scale, float octaves)
    {
        float[,] noiseMap = new float[width, height];

        if (scale <= 0) scale = 0.001f;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var xCoord = (float)x / width;
                var yCoord = (float)y / height;

                float noiseScale = scale;
                float noiseVal = 0f;
                float intensity = 1f;
                float maxHeight = 0f;

                for (int i = 0; i < octaves; i++)
                {
                    noiseVal += Mathf.PerlinNoise(xCoord * scale, yCoord * scale) * intensity;
                    maxHeight += intensity;
                    intensity *= 0.5f;
                    noiseScale *= 2f;
                }

                noiseVal /= maxHeight;
                noiseMap[x, y] = noiseVal;
            }
        }

        return noiseMap;
    }
}
