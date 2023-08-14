using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    private Vector2[] _uvs = null;
    private Terrain _terrain = null;
    private string _seed = string.Empty;
    private Vector2 _offset = Vector2.zero;
    #endregion

    #region Serialized
    [SerializeField]
    private TerrainData _terrainData = null;
    
    [SerializeField]
    private Material _material = null;
    
    //[SerializeField, Range(0f, 40f)]
    //private float _noiseIntensity = 0f;
    #endregion

    

    private void OnEnable()
    {
        _filter = GetComponent<MeshFilter>();
        _renderer = GetComponent<MeshRenderer>();
        _terrain = GetComponent<Terrain>();
        _mesh = new();
        _renderer.sharedMaterial = _material;
        _filter.sharedMesh = _mesh;
        _mesh.name = "TerrainMesh";
        GameWorldManager.Instance.TerrainGenerator = this;
    }


    private void Update()
    {
        GenerateTerrain();
    }
#if UNITY_EDITOR
    public void EditorWorldGeneration(TerrainData terrain, Material defaultMat)
    {
        
        _terrainData = terrain;
        _material = new Material(defaultMat);
        _filter = GetComponent<MeshFilter>();
        _renderer = GetComponent<MeshRenderer>();
        _mesh = new();
        _renderer.sharedMaterial = _material;
        _filter.sharedMesh = _mesh;
        _mesh.name = "Default Mesh";
        _terrainData.TerrainPosition = transform.position;
        _seed = _terrainData.Seed;
        GenerateTerrain();
    }
#endif
    private void GenerateTerrain()
    {
        Random.InitState(_seed.GetHashCode());
        _offset = new Vector2(Random.Range(0f, 9_999f), Random.Range(0f, 9_999f));

        var mapSize = _terrainData.MapSize;
        var resolution = _terrainData.Resolution;
        var position = _terrainData.TerrainPosition;
        var isNoiseUsed = _terrainData.IncludeNoise;
        var noiseIntensity = _terrainData.NoiseIntensity;
        float[,] noiseVals = default;
        this.transform.position = position;
        _verts = new Vector3[resolution * resolution];
        _uvs = new Vector2[resolution * resolution];
        //*2 für die Anzahl an Triangles pro Quad
        //*3 für die Anzahl and Indices pro Triangle
        int[] tris = new int[(resolution - 1) * (resolution - 1) * 2 * 3];
        Vector3 startPos = (Vector3.left + Vector3.back) * mapSize * 0.5f;
        int triIdx = 0;

        if (isNoiseUsed)
        {
            noiseVals = EditorNoiseGen.GenerateNoiseMap(resolution, resolution, 8, 8, _offset);
        }

        for (int y = 0, i = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++, i++)
            {

                Vector2 percent = new Vector2(x, y) / (resolution - 1);
                //Vector2 percent = new Vector2((float)x / (_resolution - 1), (float)y / (_resolution - 1));
                Vector3 vertPos = startPos + (Vector3.right * percent.x + Vector3.forward * percent.y) * mapSize;

                if (isNoiseUsed) _verts[i] = new Vector3(vertPos.x, noiseVals[x,y] * noiseIntensity, vertPos.z);
                else _verts[i] = new Vector3(vertPos.x, 0, vertPos.z);
                _uvs[i] = percent;

                if (x < resolution - 1 && y < resolution - 1)
                {
                    //Vertex kann ein Quad generieren!
                    tris[triIdx + 0] = i;
                    tris[triIdx + 1] = i + resolution;
                    tris[triIdx + 2] = i + resolution + 1;

                    tris[triIdx + 3] = i;
                    tris[triIdx + 4] = i + resolution + 1;
                    tris[triIdx + 5] = i + 1;

                    //+6 weil wir 6 neue Indices hinzugefügt haben
                    triIdx += 6;
                }
            }
        }
        _renderer.sharedMaterial.SetFloat("_Resolution", resolution);
        _mesh.Clear();
        _mesh.vertices = _verts;
        _mesh.triangles = tris;
        _mesh.uv = _uvs;
        _mesh.RecalculateNormals();

    }

}
