using Cysharp.Threading.Tasks;
using System.Linq;
using UnityEngine;

public class StaircaseGenerator : MonoBehaviour
{
    [SerializeField] private Transform _startTransform;
    [SerializeField] private int _partsCount;
    [SerializeField] private int _seed;

    private const string StairPrefabsPath = "Prefabs/Staircase/Stairs";
    private const string StairConnectionsPath = "Prefabs/Staircase/Stair Connections";

    private GameObject[] _stairs;
    private StairConnection[] _stairConnections;
    private Vector3 _size;

    private void Start()
    {
        _stairs = Resources.LoadAll<GameObject>(StairPrefabsPath);
        _size = _stairs[0].GetComponent<Stair>().Size;
        _stairConnections = Resources.LoadAll<StairConnection>(StairConnectionsPath);

        Generate().Forget();
    }

    private async UniTask Generate()
    {
        var random = new System.Random(_seed);
        var startingConnections = _stairConnections.Where(x => x.CanBeInStart).ToArray();
        var index = random.Next(startingConnections.Length);
        var stairConnection = startingConnections[index];
        var position = _startTransform.position;
        position.y = _size.y / 2;
        var rotation = _startTransform.rotation.eulerAngles;

        (position, rotation) = await GenerateSegment(random, stairConnection, position, rotation, 
            stairConnection.PositionDifference, stairConnection.RotationDifference);

        for (int i = 0; i < _partsCount; i++)
        {
            index = random.Next(_stairConnections.Length);
            stairConnection = _stairConnections[index];

            (position, rotation) = await GenerateSegment(random, stairConnection, position,  rotation, 
                stairConnection.PositionDifference, stairConnection.RotationDifference);
        }
    }

    private async UniTask<(Vector3, Vector3)> GenerateSegment(System.Random random, StairConnection stairConnection, 
        Vector3 position, Vector3 rotation, Vector3 positionDifference, Vector3 rotationDifference)
    {
        int count = random.Next(stairConnection.MinCount, stairConnection.MaxCount + 1);

        for (int i = 0; i < count; i++)
        {
            var index = random.Next(_stairs.Length);
            var stair = Instantiate(_stairs[index], position, Quaternion.Euler(rotation));
            position += stair.transform.TransformDirection(positionDifference);
            rotation += rotationDifference;
            await UniTask.Yield();
        }
        
        return (position, rotation);
    }
}
