using Random = System.Random;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ProceduralGlassFloor : MonoBehaviour
{
    [SerializeField] private int _width = 10;
    [SerializeField] private int _height = 10;
    [SerializeField] private float _scale = 1f;
    [SerializeField] private int _seed = 0;
    [SerializeField][Range(0.5f, 1f)] private float _colliderResolution = 1f;

    private Mesh _mesh;
    private Vector3[] _vertices;
    private int[] _triangles;

    [ContextMenu(nameof(GenerateFloor))]
    private void GenerateInEditor() => GenerateFloor();

    private void Start() => GenerateFloor();

    private void GenerateFloor()
    {
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;

        _vertices = new Vector3[(_width + 1) * (_height + 1)];
        _triangles = new int[_width * _height * 6];

        Random random = new(_seed);

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

        int vertex = 0;
        int triangle = 0;

        for (int z = 0; z < _height; z++)
        {
            for (int x = 0; x < _width; x++)
            {
                _triangles[triangle + 0] = vertex + 0;
                _triangles[triangle + 1] = vertex + _width + 1;
                _triangles[triangle + 2] = vertex + 1;
                _triangles[triangle + 3] = vertex + 1;
                _triangles[triangle + 4] = vertex + _width + 1;
                _triangles[triangle + 5] = vertex + _width + 2;

                vertex++;
                triangle += 6;
            }

            vertex++;
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
        if (!gameObject.TryGetComponent<MeshCollider>(out var meshCollider))
            meshCollider = gameObject.AddComponent<MeshCollider>();

        Mesh colliderMesh = new();

        if (_colliderResolution == 1f)
        {
            colliderMesh.vertices = _mesh.vertices;
            colliderMesh.triangles = _mesh.triangles;
        }
        else
        {
            int simplifiedWidth = Mathf.RoundToInt(_width * _colliderResolution);
            int simplifiedHeight = Mathf.RoundToInt(_height * _colliderResolution);

            Vector3[] colliderVertices = new Vector3[(simplifiedWidth + 1) * (simplifiedHeight + 1)];
            int[] colliderTriangles = new int[simplifiedWidth * simplifiedHeight * 6];

            for (int z = 0; z <= simplifiedHeight; z++)
            {
                for (int x = 0; x <= simplifiedWidth; x++)
                {
                    int originX = Mathf.FloorToInt(x / _colliderResolution);
                    int originZ = Mathf.FloorToInt(z / _colliderResolution);

                    int vertexIndex = z * (simplifiedWidth + 1) + x;
                    int originIndex1 = originZ * (_width + 1) + originX;
                    int originIndex2 = originZ * (_width + 1) + Mathf.Min(originX + 1, _width);
                    int originIndex3 = Mathf.Min(originZ + 1, _height) * (_width + 1) + originX;
                    int originIndex4 = Mathf.Min(originZ + 1, _height) * (_width + 1) + Mathf.Min(originX + 1, _width);

                    Vector3 avgPosition = (_vertices[originIndex1] + _vertices[originIndex2] + _vertices[originIndex3] + _vertices[originIndex4]) / 4f;
                    colliderVertices[vertexIndex] = avgPosition;
                }
            }

            int vertex = 0;
            int triangle = 0;

            for (int z = 0; z < simplifiedHeight; z++)
            {
                for (int x = 0; x < simplifiedWidth; x++)
                {
                    colliderTriangles[triangle + 0] = vertex + 0;
                    colliderTriangles[triangle + 1] = vertex + simplifiedWidth + 1;
                    colliderTriangles[triangle + 2] = vertex + 1;
                    colliderTriangles[triangle + 3] = vertex + 1;
                    colliderTriangles[triangle + 4] = vertex + simplifiedWidth + 1;
                    colliderTriangles[triangle + 5] = vertex + simplifiedWidth + 2;

                    vertex++;
                    triangle += 6;
                }

                vertex++;
            }

            colliderMesh.vertices = colliderVertices;
            colliderMesh.triangles = colliderTriangles;
        }

        colliderMesh.RecalculateNormals();
        meshCollider.sharedMesh = colliderMesh;
    }
}
