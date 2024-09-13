using System;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ProceduralGlassFloor : MonoBehaviour
{
    [SerializeField] private int _width = 10;
    [SerializeField] private int _height = 10;
    [SerializeField] private float _scale = 1f;
    [SerializeField] private int _seed = 12345;
    [SerializeField][Range(0.5f, 1f)] private float _colliderResolution = 1f; // Регулирует детализацию коллайдера

    private Mesh _mesh;
    private Vector3[] _vertices;
    private int[] _triangles;

    [ContextMenu("Generate Floor")]
    private void GenerateInEditor()
    {
        GenerateFloor();
    }

    private void Start()
    {
        GenerateFloor();
    }

    private void GenerateFloor()
    {
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;

        _vertices = new Vector3[(_width + 1) * (_height + 1)];
        _triangles = new int[_width * _height * 6];

        System.Random random = new System.Random(_seed);

        float xOffset = _width / 2f;
        float zOffset = _height / 2f;

        for (int i = 0, z = 0; z <= _height; z++)
        {
            for (int x = 0; x <= _width; x++, i++)
            {
                float y = (float)random.NextDouble() * _scale;
                _vertices[i] = new Vector3(x - xOffset, y, z - zOffset);
            }
        }

        int vert = 0;
        int tris = 0;
        for (int z = 0; z < _height; z++)
        {
            for (int x = 0; x < _width; x++)
            {
                _triangles[tris + 0] = vert + 0;
                _triangles[tris + 1] = vert + _width + 1;
                _triangles[tris + 2] = vert + 1;
                _triangles[tris + 3] = vert + 1;
                _triangles[tris + 4] = vert + _width + 1;
                _triangles[tris + 5] = vert + _width + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }

        UpdateMesh();

        GenerateMeshCollider();
    }

    private void UpdateMesh()
    {
        _mesh.Clear();
        _mesh.vertices = _vertices;
        _mesh.triangles = _triangles;
        _mesh.RecalculateNormals();
    }

    private void GenerateMeshCollider()
    {
        MeshCollider meshCollider = gameObject.GetComponent<MeshCollider>();
        if (meshCollider == null)
        {
            meshCollider = gameObject.AddComponent<MeshCollider>();
        }

        // Генерация упрощённого меша для коллайдера
        Mesh colliderMesh = new Mesh();

        if (_colliderResolution == 1f)
        {
            // Полное совпадение коллайдера с мешем
            colliderMesh.vertices = _mesh.vertices;
            colliderMesh.triangles = _mesh.triangles;
        }
        else
        {
            // Упрощённая версия коллайдера
            int simplifiedWidth = Mathf.RoundToInt(_width * _colliderResolution);
            int simplifiedHeight = Mathf.RoundToInt(_height * _colliderResolution);

            Vector3[] colliderVertices = new Vector3[(simplifiedWidth + 1) * (simplifiedHeight + 1)];
            int[] colliderTriangles = new int[simplifiedWidth * simplifiedHeight * 6];

            // Генерация вершин для упрощённого коллайдера
            for (int z = 0; z <= simplifiedHeight; z++)
            {
                for (int x = 0; x <= simplifiedWidth; x++)
                {
                    int origX = Mathf.FloorToInt(x / _colliderResolution);
                    int origZ = Mathf.FloorToInt(z / _colliderResolution);

                    int vertexIndex = z * (simplifiedWidth + 1) + x;
                    int origIndex1 = origZ * (_width + 1) + origX;
                    int origIndex2 = origZ * (_width + 1) + Mathf.Min(origX + 1, _width);
                    int origIndex3 = Mathf.Min(origZ + 1, _height) * (_width + 1) + origX;
                    int origIndex4 = Mathf.Min(origZ + 1, _height) * (_width + 1) + Mathf.Min(origX + 1, _width);

                    Vector3 avgPosition = (_vertices[origIndex1] + _vertices[origIndex2] + _vertices[origIndex3] + _vertices[origIndex4]) / 4f;
                    colliderVertices[vertexIndex] = avgPosition;
                }
            }

            // Генерация треугольников для упрощённого коллайдера
            int vert = 0;
            int tris = 0;
            for (int z = 0; z < simplifiedHeight; z++)
            {
                for (int x = 0; x < simplifiedWidth; x++)
                {
                    colliderTriangles[tris + 0] = vert + 0;
                    colliderTriangles[tris + 1] = vert + simplifiedWidth + 1;
                    colliderTriangles[tris + 2] = vert + 1;
                    colliderTriangles[tris + 3] = vert + 1;
                    colliderTriangles[tris + 4] = vert + simplifiedWidth + 1;
                    colliderTriangles[tris + 5] = vert + simplifiedWidth + 2;

                    vert++;
                    tris += 6;
                }
                vert++;
            }

            colliderMesh.vertices = colliderVertices;
            colliderMesh.triangles = colliderTriangles;
        }

        colliderMesh.RecalculateNormals();
        meshCollider.sharedMesh = colliderMesh;
    }
}
