using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomNoise : MonoBehaviour
{
    private int _resolution = 0;
    private float[] _noiseMap = null;

    private void GenerateNoiseMap()
    {
        _noiseMap = new float[_resolution * _resolution];
    }
}
