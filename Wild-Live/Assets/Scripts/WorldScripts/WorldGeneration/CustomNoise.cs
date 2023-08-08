using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Profiling;

public class CustomNoise : MonoBehaviour
{
    [SerializeField] List<Terrain> _terrains = null;
    [SerializeField] private Texture2D _texture = null;
    [SerializeField] private string _seed = string.Empty;
    [SerializeField] private float _octaveCount = 1f;
    [SerializeField] private float _scale = 1f;
    [SerializeField] private bool _useHeightmap = false;
    [SerializeField, Range(0.1f, 1.0f)] private float _heightMapIntensity = 0.1f;
    [SerializeField] private int _chunkAmount = 1;
    [SerializeField] private int _noiseSize = 1025;

    private Vector2 _offset = Vector2.zero;
    private float[,] _noise = null;

    private void Start()
    {
        Random.InitState(_seed.GetHashCode());
        _offset = new Vector2(Random.Range(0, 100_000), Random.Range(0, 100_000));
        _noise = GenerateWorldNoise();

        //foreach (var terrain in _terrains)
        //{
            //terrain.GetComponent<TerrainGeneration>().GenerateTerrain(_noise);
        //}

        for (int i = 0; i < _terrains.Count; i++)
        {
            _terrains[i].GetComponent<TerrainGeneration>().GenerateTerrain(_noise);
        }

        for (int x = 0; x < _noiseSize; x++)
        {
            for (int y = 0; y < _noiseSize; y++)
            {
                var colVal = _noise[x, y];
                var color = new Color(colVal, colVal, colVal, 1);
                _texture.SetPixel(x, y, color);
            }
        }
        _texture.Apply();
    }

    private float[,] GenerateWorldNoise()
    {
        var chunkSize = _noiseSize / _chunkAmount;
        Vector2 chunk = new Vector2(chunkSize, chunkSize);
        float[,] noise = new float[_noiseSize, _noiseSize];
        List<Thread> threads = new List<Thread>();

        Profiler.BeginSample("Noise Gen " + gameObject.name);

        for (int y = 0; y < _chunkAmount; y++)
        {
            for (int x = 0; x < _chunkAmount; x++)
            {
                var minX = (int)chunk.x * x;
                var minY = (int)chunk.y * y;
                var maxX = minX + chunkSize;
                var maxY = minY + chunkSize;

                //GenerateWorldNoise(_scale, minX, minY, maxX, maxY, ref noise);
                var thread = new Thread(() =>
                {
                    GenerateWorldNoise(minX, maxX, minY, maxY, ref noise);
                    threads.Remove(Thread.CurrentThread);
                });
                thread.Start();
                threads.Add(thread);
            }
        }

        System.Diagnostics.Stopwatch _watch = System.Diagnostics.Stopwatch.StartNew();

        bool threadsAreWorking = true;
        //while (threadsAreWorking)
        //{
        //    threadsAreWorking = false;
        //    for (int i = 0; i < threads.Count; i++)
        //    {
        //        if (threads[i].IsAlive)
        //        {
        //            threadsAreWorking = true;
        //            break;
        //        }
        //    }
        //}
        while (threads.Count > 0)
        {

        }

        Profiler.EndSample();
        //Debug.Log($"TIME: {_watch.ElapsedTicks} Ticks");



        return noise;
    }

    private void GenerateWorldNoise(int minX, int maxX, int minY, int maxY, ref float[,] noise)
    {
        //Debug.Log($"{minX},{maxX},{minY},{maxY}");

        System.Diagnostics.Stopwatch _watch = System.Diagnostics.Stopwatch.StartNew();

        //Debug.Log(Thread.CurrentThread.ManagedThreadId);

        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                var xCoord = (float)x / _noiseSize;
                var yCoord = (float)y / _noiseSize;

                float noiseVal = 0f;
                float intensity = 1f;
                float scale = _scale;
                float maxHeight = 0f;

                for (int i = 0; i < _octaveCount; i++)
                {
                    noiseVal += Mathf.PerlinNoise(xCoord * scale + _offset.x, yCoord * scale + _offset.y) * intensity;
                    maxHeight += intensity;
                    intensity *= 0.5f;
                    scale *= 2f;
                }

                noiseVal /= maxHeight;
                noise[x,y] = noiseVal;
            }
        }

        Debug.Log($"{Thread.CurrentThread.ManagedThreadId}: {_watch.ElapsedTicks}");
        _watch.Stop();
    }
}
