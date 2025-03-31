using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class StairsLoader
{
    private AsyncOperationHandle<IList<GameObject>> _stairsHandle;
    private AsyncOperationHandle<IList<StairConnection>> _connectionsHandle;

    public async UniTask<IEnumerable<GameObject>> LoadStairsAsync(CancellationToken token)
    {
        List<string> labels = new() { "stair", "prefab" };
        _stairsHandle = Addressables.LoadAssetsAsync<GameObject>(
            labels,
            _ => { },
            Addressables.MergeMode.Intersection);
        await _stairsHandle.ToUniTask(cancellationToken: token);

        if (_stairsHandle.Status == AsyncOperationStatus.Succeeded)
            return _stairsHandle.Result;

        return null;
    }

    public async UniTask<IEnumerable<StairConnection>> LoadStairConnectionsAsync(CancellationToken token)
    {
        string label = "stair connection";
        _connectionsHandle = Addressables.LoadAssetsAsync<StairConnection>(label);
        await _connectionsHandle.ToUniTask(cancellationToken: token);

        if (_connectionsHandle.Status == AsyncOperationStatus.Succeeded)
            return _connectionsHandle.Result;

        return null;
    }

    public void ReleaseStairs()
    {
        if (_stairsHandle.IsValid())
            _stairsHandle.Release();

        if (_connectionsHandle.IsValid())
            _connectionsHandle.Release();
    }
}
