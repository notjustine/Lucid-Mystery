using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MeshCutter : MonoBehaviour
{
    public int numberOfSlices = 8;
    public float radius = 5f;

    void Start()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = CreatePizzaSlicesMesh(numberOfSlices, radius);
    }

    Mesh CreatePizzaSlicesMesh(int slices, float radius)
    {
        Mesh mesh = new Mesh();
        int verticesPerSlice = 4; // One at the center, two on the edge, and one in the middle of the edge
        Vector3[] vertices = new Vector3[verticesPerSlice * slices];
        int[] triangles = new int[9 * slices]; // Each slice now consists of 3 smaller triangles

        float angleStep = 360.0f / slices;
        for (int i = 0; i < slices; i++)
        {
            // Center point (common for all segments)
            vertices[i * verticesPerSlice] = Vector3.zero;

            // Edge points
            float angle = angleStep * i;
            vertices[i * verticesPerSlice + 1] = Quaternion.Euler(0, angle, 0) * Vector3.forward * radius;
            vertices[i * verticesPerSlice + 2] = Quaternion.Euler(0, angle + angleStep, 0) * Vector3.forward * radius;

            // Middle point (for subdividing the slice)
            vertices[i * verticesPerSlice + 3] = (vertices[i * verticesPerSlice + 1] + vertices[i * verticesPerSlice + 2]) / 2;

            // First smaller triangle (bottom)
            triangles[i * 9] = i * verticesPerSlice;
            triangles[i * 9 + 1] = i * verticesPerSlice + 1;
            triangles[i * 9 + 2] = i * verticesPerSlice + 3;

            // Second smaller triangle (middle)
            triangles[i * 9 + 3] = i * verticesPerSlice;
            triangles[i * 9 + 4] = i * verticesPerSlice + 3;
            triangles[i * 9 + 5] = i * verticesPerSlice + 2;

            // Third smaller triangle (top)
            triangles[i * 9 + 6] = i * verticesPerSlice + 3;
            triangles[i * 9 + 7] = i * verticesPerSlice + 1;
            triangles[i * 9 + 8] = i * verticesPerSlice + 2;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }
}
