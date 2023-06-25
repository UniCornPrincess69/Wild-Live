using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TerrainGenerator : MonoBehaviour
{
    #region Variables
    private MeshFilter _filter = null;
    private MeshRenderer _renderer = null;
    private Mesh _mesh = null;
    private Vector3[] _verts = null;
    #endregion

    #region Serialized
    [SerializeField]
    private bool _isNoiseUsed = false;

    [SerializeField]
    private int _terrainSize = 20;

    [SerializeField]
    private Material _material = null;

    [SerializeField, Range(30, 255)]
    private int _resolution = 30;

    [SerializeField, Range(0f, 40f)]
    private float _noiseIntensity = 0f;
    #endregion

    

    private void Awake()
    {
        _filter = GetComponent<MeshFilter>();
        _renderer = GetComponent<MeshRenderer>();
        _mesh = new();
        _renderer.sharedMaterial = _material;
        _filter.sharedMesh = _mesh;
        _mesh.name = "TerrainMesh";
    }

    private void Update()
    {
        GenerateTerrain();
    }

    private void GenerateTerrain()
    {
        _verts = new Vector3[_resolution * _resolution];
        //*2 für die Anzahl an Triangles pro Quad
        //*3 für die Anzahl and Indices pro Triangle
        int[] tris = new int[(_resolution - 1) * (_resolution - 1) * 2 * 3];
        Vector3 startPos = (Vector3.left + Vector3.back) * _terrainSize * 0.5f;
        int triIdx = 0;
        for (int y = 0, i = 0; y < _resolution; y++)
        {
            for (int x = 0; x < _resolution; x++, i++)
            {
                float n = 0f;
                if (_isNoiseUsed)
                {
                    n = Mathf.PerlinNoise(x * .3f, y * .3f) * _noiseIntensity;
                }

                Vector2 percent = new Vector2(x, y) / (_resolution - 1);
                Vector3 vertPos = startPos + (Vector3.right * percent.x + Vector3.forward * percent.y) * _terrainSize;

                _verts[i] = new Vector3(vertPos.x, n, vertPos.z);

                if (x < _resolution - 1 && y < _resolution - 1)
                {
                    //Vertex kann ein Quad generieren!
                    tris[triIdx + 0] = i;
                    tris[triIdx + 1] = i + _resolution;
                    tris[triIdx + 2] = i + _resolution + 1;

                    tris[triIdx + 3] = i;
                    tris[triIdx + 4] = i + _resolution + 1;
                    tris[triIdx + 5] = i + 1;

                    //+6 weil wir 6 neue Indices hinzugefügt haben
                    triIdx += 6;
                }
            }
        }

        _mesh.Clear();
        _mesh.vertices = _verts;
        _mesh.triangles = tris;
        _mesh.RecalculateNormals();
    }

    //private void OnDrawGizmos()
    //{
    //    if (_verts == null) return;

    //    for (int i = 0; i < _verts.Length; i++)
    //    {
    //        Gizmos.DrawSphere(_verts[i], .1f);
    //    }
    //}
}
