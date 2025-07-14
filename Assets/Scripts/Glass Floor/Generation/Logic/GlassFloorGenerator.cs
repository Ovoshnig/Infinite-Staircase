using Random = System.Random;
using UnityEngine;
using VContainer.Unity;
using VContainer;

public class GlassFloorGenerator : IInitializable
{
    private readonly SaveStorage _saveStorage;
    private readonly Transform _startPoint;
    private readonly GlassFloorSettings _floorSettings;

    private Random _random;
    private Mesh _mesh;
    private Vector3[] _vertices;
    private int[] _triangles;

    public GlassFloorGenerator(Transform startPoint, GlassFloorSettings floorSettings,
        int seed)
    {
        _startPoint = startPoint;
        _floorSettings = floorSettings;
        _random = new Random(seed);
    }

    [Inject]
    public GlassFloorGenerator(SaveStorage saveStorage, Transform startPoint, 
        GlassFloorSettings floorSettings)
    {
        _saveStorage = saveStorage;
        _startPoint = startPoint;
        _floorSettings = floorSettings;
    }

    public void Initialize()
    {
        int seed = _saveStorage.Get(SaveConstants.SeedKey, 0);
        _random = new Random(seed);

        GenerateFloor();
    }

    public void GenerateFloor()
    {
        _mesh = new Mesh();
        _startPoint.GetComponent<MeshFilter>().mesh = _mesh;

        int length = _floorSettings.Length;
        int width = _floorSettings.Width;
        float height = _floorSettings.Height;

        _vertices = new Vector3[(length + 1) * (width + 1)];
        _triangles = new int[length * width * 6];

        float xOffset = length / 2f;
        float zOffset = width / 2f;

        for (int i = 0, z = 0; z <= width; z++)
        {
            for (int x = 0; x <= length; x++, i++)
            {
                float y = (float)_random.NextDouble() * height;
                _vertices[i] = new Vector3(x - xOffset, y, z - zOffset);
            }
        }

        int vertex = 0;
        int triangle = 0;

        for (int z = 0; z < width; z++)
        {
            for (int x = 0; x < length; x++)
            {
                _triangles[triangle + 0] = vertex + 0;
                _triangles[triangle + 1] = vertex + length + 1;
                _triangles[triangle + 2] = vertex + 1;
                _triangles[triangle + 3] = vertex + 1;
                _triangles[triangle + 4] = vertex + length + 1;
                _triangles[triangle + 5] = vertex + length + 2;

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
        if (!_startPoint.gameObject.TryGetComponent<MeshCollider>(out var meshCollider))
            meshCollider = _startPoint.gameObject.AddComponent<MeshCollider>();

        Mesh colliderMesh = new();

        int length = _floorSettings.Length;
        int width = _floorSettings.Width;
        float colliderResolution = _floorSettings.ColliderResolution;

        if (colliderResolution == 1f)
        {
            colliderMesh.vertices = _mesh.vertices;
            colliderMesh.triangles = _mesh.triangles;
        }
        else
        {
            int simplifiedWidth = Mathf.RoundToInt(length * colliderResolution);
            int simplifiedHeight = Mathf.RoundToInt(width * colliderResolution);

            Vector3[] colliderVertices = new Vector3[(simplifiedWidth + 1) * (simplifiedHeight + 1)];
            int[] colliderTriangles = new int[simplifiedWidth * simplifiedHeight * 6];

            for (int z = 0; z <= simplifiedHeight; z++)
            {
                for (int x = 0; x <= simplifiedWidth; x++)
                {
                    int originX = Mathf.FloorToInt(x / colliderResolution);
                    int originZ = Mathf.FloorToInt(z / colliderResolution);

                    int vertexIndex = z * (simplifiedWidth + 1) + x;
                    int originIndex1 = originZ * (length + 1) + originX;
                    int originIndex2 = originZ * (length + 1) + Mathf.Min(originX + 1, length);
                    int originIndex3 = Mathf.Min(originZ + 1, width) * (length + 1) + originX;
                    int originIndex4 = Mathf.Min(originZ + 1, width) * (length + 1) + Mathf.Min(originX + 1, length);

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
