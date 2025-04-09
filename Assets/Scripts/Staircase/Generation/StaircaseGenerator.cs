using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using VContainer.Unity;
using Object = UnityEngine.Object;
using Random = System.Random;

public class StaircaseGenerator : IInitializable, IDisposable
{
    private readonly SaveStorage _saveStorage;
    private readonly StairsLoader _stairsLoader;
    private readonly Transform _startPoint;
    private readonly StaircaseGenerationSettings _staircaseGenerationSettings;
    private readonly CancellationTokenSource _cts = new();
    private GameObject[] _stairs;
    private StairConnection[] _stairConnections;
    private Vector3 _size;
    private Random _random;

    public StaircaseGenerator(SaveStorage saveStorage, StairsLoader stairsLoader,
        Transform staircaseStartPoint, StaircaseGenerationSettings staircaseGenerationSettings)
    {
        _saveStorage = saveStorage;
        _stairsLoader = stairsLoader;
        _startPoint = staircaseStartPoint;
        _staircaseGenerationSettings = staircaseGenerationSettings;
    }

    public async void Initialize()
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

    public void Dispose()
    {
        _cts?.CancelAndDispose();

        _stairsLoader.ReleaseStairs();
    }

    private async UniTask GenerateAsync(CancellationToken token)
    {
        StairConnection[] startingConnections = _stairConnections.Where(x => x.CanBeInStart).ToArray();
        int index = _random.Next(startingConnections.Length);
        StairConnection stairConnection = startingConnections[index];
        Vector3 position = _startPoint.position;
        position.y += _size.y / 2f;
        Vector3 rotation = _startPoint.eulerAngles;

        (position, rotation) = await GenerateSegmentAsync(stairConnection, position, rotation,
            stairConnection.PositionDifference, stairConnection.RotationDifference, token);

        for (int i = 0; i < _staircaseGenerationSettings.SegmentCount; i++)
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
            GameObject stair = Object.Instantiate(_stairs[index], position, Quaternion.Euler(rotation));
            stair.transform.parent = _startPoint;
            position += stair.transform.TransformDirection(positionDifference);
            rotation += rotationDifference;

            await UniTask.Yield(token);
        }

        return (position, rotation);
    }
}
