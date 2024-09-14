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

    private SaveStorage _saveStorage;
    private GameObject[] _stairs;
    private StairConnection[] _stairConnections;
    private CancellationTokenSource _cts;
    private Vector3 _size;
    private Random _random;

    [Inject]
    private void Construct(SaveStorage saveStorage) => _saveStorage = saveStorage;

    private void Awake() => _cts = new CancellationTokenSource();

    private void Start()
    {
        int seed = _saveStorage.Get(SaveConstants.SeedKey, 0);
        _random = new Random(seed);
        _stairs = Resources.LoadAll<GameObject>(ResourcesConstants.StairPrefabsPath);
        _size = _stairs[0].GetComponent<Stair>().Size;
        _stairConnections = Resources.LoadAll<StairConnection>(ResourcesConstants.StairConnectionsPath);

        Generate().Forget();
    }

    private void OnDisable() => _cts.CancelAndDispose(ref _cts);

    private async UniTask Generate()
    {
        StairConnection[] startingConnections = _stairConnections.Where(x => x.CanBeInStart).ToArray();
        int index = _random.Next(startingConnections.Length);
        StairConnection stairConnection = startingConnections[index];
        Vector3 position = _startTransform.position;
        position.y += _size.y / 2f;
        Vector3 rotation = _startTransform.eulerAngles;

        (position, rotation) = await GenerateSegment(stairConnection, position, rotation, 
            stairConnection.PositionDifference, stairConnection.RotationDifference);

        for (int i = 0; i < _partsCount; i++)
        {
            index = _random.Next(_stairConnections.Length);
            stairConnection = _stairConnections[index];

            (position, rotation) = await GenerateSegment(stairConnection, position,  rotation, 
                stairConnection.PositionDifference, stairConnection.RotationDifference);
        }
    }

    private async UniTask<(Vector3, Vector3)> GenerateSegment(StairConnection stairConnection, 
        Vector3 position, Vector3 rotation, Vector3 positionDifference, Vector3 rotationDifference)
    {
        int count = _random.Next(stairConnection.MinCount, stairConnection.MaxCount + 1);

        for (int i = 0; i < count; i++)
        {
            int index = _random.Next(_stairs.Length);
            GameObject stair = Instantiate(_stairs[index], position, Quaternion.Euler(rotation));
            stair.transform.parent = transform;
            position += stair.transform.TransformDirection(positionDifference);
            rotation += rotationDifference;

            await UniTask.Yield(_cts.Token);
        }
        
        return (position, rotation);
    }
}
 