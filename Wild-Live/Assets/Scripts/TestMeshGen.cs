using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMeshGen : MonoBehaviour
{
    private void Start()
    {
        var vertices = new List<Vector3>();
        var indices = new List<int>();
        var uvs = new List<Vector2>();

        vertices.Add(Vector3.zero);
        vertices.Add(Vector3.up);
        vertices.Add(Vector3.up + Vector3.right);
        vertices.Add(Vector3.right);

        vertices.Add(new Vector3(2, 0, 0));
        vertices.Add(new Vector3(2, 1, 0));
        vertices.Add(new Vector3(3, 1, 0));
        vertices.Add(new Vector3(3, 0, 0));


        indices.AddRange(new[] { 0, 1, 2 });
        indices.AddRange(new[] { 2, 3, 0 });

        indices.AddRange(new[] { 4, 5, 6 });
        indices.AddRange(new[] { 6, 7, 4 });

        uvs.Add(Vector2.zero);
        uvs.Add(Vector2.up);
        uvs.Add(Vector2.one);
        uvs.Add(Vector2.right);

        uvs.Add(Vector2.zero / 2);
        uvs.Add(Vector2.up / 2);
        uvs.Add(Vector2.one / 2);
        uvs.Add(Vector2.right / 2);

        Mesh mesh = new()
        {
            vertices = vertices.ToArray(),
            triangles = indices.ToArray(),
            uv = uvs.ToArray(),
        };

        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        mesh.RecalculateBounds();

        GetComponent<MeshFilter>().mesh = mesh;
    }
}