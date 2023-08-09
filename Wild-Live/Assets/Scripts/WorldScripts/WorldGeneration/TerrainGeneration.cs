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

        _terrain.terrainData.SetHeights(0, 0, heightsReduced);
        Profiler.EndSample();
    }


}
