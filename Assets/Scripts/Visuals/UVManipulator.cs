using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class UVManipulator : MonoBehaviour
{
    private void Start()
    {
        GetMeshUV();
    }

    void GetMeshUV()
    {
        Mesh mesh = this.gameObject.GetComponent<MeshFilter>().mesh;

        Vector3[] vertices = GetVertices(mesh);
        Vector2[] uvs = GetUV(vertices);

        mesh.uv = uvs;
    }

    void shit()
    {
        //a triangle def
        int[] triangle = new int[]
        {
            0, 2, 3,
            3, 1, 0
        };
    }

    Vector2[] GetUV(Vector3[] vertices)
    {
        Vector2[] uvs = new Vector2[vertices.Length];

        Array.Sort(vertices);

        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(x: vertices[i].x, y: vertices[i].z);
        }

        return uvs;
    }

    Vector3[] GetVertices(Mesh mesh)
    {
        Vector3[] vertices = mesh.vertices;

        return vertices;
    }
}
