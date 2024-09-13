using System;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ProceduralGlassFloor : MonoBehaviour
{
    [SerializeField] private int _width = 10;
    [SerializeField] private int _height = 10;
    [SerializeField] private float _scale = 1f;
    [SerializeField] private int _seed = 12345;
    [SerializeField] private bool _generateColliders = true;

    private Mesh _mesh;
    private Vector3[] _vertices;
    private int[] _triangles;
    private GameObject _colliderContainer;

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

        if (_generateColliders)
        {
            GenerateColliders();
        }
    }

    private void UpdateMesh()
    {
        _mesh.Clear();
        _mesh.vertices = _vertices;
        _mesh.triangles = _triangles;
        _mesh.RecalculateNormals();
    }

    private void GenerateColliders()
    {
        if (_colliderContainer != null)
            DestroyImmediate(_colliderContainer);

        // Создаём новый пустой объект для коллайдеров
        _colliderContainer = new GameObject("ColliderContainer");
        _colliderContainer.transform.SetParent(transform);
        _colliderContainer.transform.localPosition = Vector3.zero;

        for (int z = 0; z < _height; z++)
        {
            for (int x = 0; x < _width; x++)
            {
                Vector3 corner1 = _vertices[z * (_width + 1) + x];
                Vector3 corner2 = _vertices[z * (_width + 1) + (x + 1)];
                Vector3 corner3 = _vertices[(z + 1) * (_width + 1) + x];
                Vector3 corner4 = _vertices[(z + 1) * (_width + 1) + (x + 1)];

                Vector3 center = (corner1 + corner2 + corner3 + corner4) / 4f;
                float maxHeight = Mathf.Max(corner1.y, corner2.y, corner3.y, corner4.y);
                float minHeight = Mathf.Min(corner1.y, corner2.y, corner3.y, corner4.y);

                GameObject colliderObj = new GameObject($"Collider_{x}_{z}");
                colliderObj.transform.SetParent(_colliderContainer.transform);
                colliderObj.transform.localPosition = center;

                BoxCollider boxCollider = colliderObj.AddComponent<BoxCollider>();
                boxCollider.size = new Vector3(1, maxHeight - minHeight, 1);
            }
        }
    }
}
