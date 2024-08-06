using Cysharp.Threading.Tasks;
using Random = System.Random;
using System.Linq;
using System.Threading;
using UnityEngine;
using Zenject;

public class StaircaseGenerator : MonoBehaviour
{
    [SerializeField] private Transform _startTransform;
    [SerializeField] private int _partsCount;

    private readonly CancellationTokenSource _cts = new();
    private SaveSaver _saveSaver;
    private int _seed;
    private GameObject[] _stairs;
    private StairConnection[] _stairConnections;
    private Vector3 _size;


    [Inject]
    private void Construct(SaveSaver saveSaver) => _saveSaver = saveSaver;

    private void Start()
    {
        _seed = _saveSaver.LoadData<int>(SaveConstants.SeedKey, default);
        _stairs = Resources.LoadAll<GameObject>(ResourcesConstants.StairPrefabsPath);
        _size = _stairs[0].GetComponent<Stair>().Size;
        _stairConnections = Resources.LoadAll<StairConnection>(ResourcesConstants.StairConnectionsPath);

        Generate().Forget();
    }

    private void OnDisable()
    {
        if (_cts != null)
        {
            _cts.Cancel();
            _cts.Dispose();
        }
    }

    private async UniTask Generate()
    {
        var random = new Random(_seed);
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
            stair.transform.parent = transform;
            position += stair.transform.TransformDirection(positionDifference);
            rotation += rotationDifference;
            await UniTask.Yield(_cts.Token);
        }
        
        return (position, rotation);
    }
}
