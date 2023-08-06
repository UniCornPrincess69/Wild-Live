using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NoiseGen
{
    public static float[,] GenerateNoiseMap(int width, int height, float scale, float octaves)
    {
        float[,] noiseMap = new float[width, height];

        if (scale <= 0) scale = 0.001f;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {

                for (int i = 0; i < octaves; i++)
                {
                    float xCoord = x / scale;
                    float yCoord = y / scale;
                    float intensity = 1f;

                    float perlinValue = Mathf.PerlinNoise(xCoord, yCoord) * intensity;
                    intensity *= 0.5f;
                    noiseMap[x, y] = perlinValue;
                }
            }
        }

        return noiseMap;
    }
}
