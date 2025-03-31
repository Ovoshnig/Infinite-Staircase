using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using VContainer;
using Random = System.Random;

public class StaircaseGenerator : MonoBehaviour
{
    [SerializeField] private Transform _startTransform;
    [SerializeField] private int _partsCount;

    private readonly CancellationTokenSource _cts = new();
    private SaveStorage _saveStorage;
    private StairsLoader _stairsLoader;
    private GameObject[] _stairs;
    private StairConnection[] _stairConnections;
    private Vector3 _size;
    private Random _random;

    [Inject]
    public void Construct(SaveStorage saveStorage, StairsLoader stairsLoader)
    {
        _saveStorage = saveStorage;
        _stairsLoader = stairsLoader;
    }

    private async void Start()
    {
        int seed = _saveStorage.Get(SaveConstants.SeedKey, 0);
        _random = new Random(seed);

        using CancellationTokenSource cts = new();
        IEnumerable<GameObject> stairs = await _stairsLoader.LoadStairsAsync(cts.Token);
        _stairs = stairs.ToArray();
        _size = _stairs[0].GetComponent<Stair>().Size;

        using CancellationTokenSource cts1 = new();
        IEnumerable<StairConnection> stairConnections = await _stairsLoader
            .LoadStairConnectionsAsync(cts1.Token);
        _stairConnections = stairConnections.ToArray();

        try
        {
            await GenerateAsync(_cts.Token);
        }
        catch (OperationCanceledException)
        {
            return;
        }
    }

    private void OnDestroy()
    {
        _cts?.CancelAndDispose();

        _stairsLoader.ReleaseStairs();
    }

    private async UniTask GenerateAsync(CancellationToken token)
    {
        StairConnection[] startingConnections = _stairConnections.Where(x => x.CanBeInStart).ToArray();
        int index = _random.Next(startingConnections.Length);
        StairConnection stairConnection = startingConnections[index];
        Vector3 position = _startTransform.position;
        position.y += _size.y / 2f;
        Vector3 rotation = _startTransform.eulerAngles;

        (position, rotation) = await GenerateSegmentAsync(stairConnection, position, rotation,
            stairConnection.PositionDifference, stairConnection.RotationDifference, token);

        for (int i = 0; i < _partsCount; i++)
        {
            index = _random.Next(_stairConnections.Length);
            stairConnection = _stairConnections[index];

            (position, rotation) = await GenerateSegmentAsync(stairConnection, position, rotation,
                stairConnection.PositionDifference, stairConnection.RotationDifference, token);
        }
    }

    private async UniTask<(Vector3, Vector3)> GenerateSegmentAsync(StairConnection stairConnection,
        Vector3 position, Vector3 rotation, Vector3 positionDifference, Vector3 rotationDifference,
        CancellationToken token)
    {
        int count = _random.Next(stairConnection.MinCount, stairConnection.MaxCount + 1);

        for (int i = 0; i < count; i++)
        {
            int index = _random.Next(_stairs.Length);
            GameObject stair = Instantiate(_stairs[index], position, Quaternion.Euler(rotation));
            stair.transform.parent = transform;
            position += stair.transform.TransformDirection(positionDifference);
            rotation += rotationDifference;

            await UniTask.Yield(token);
        }

        return (position, rotation);
    }
}
