using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class StairsLoader
{
    private AsyncOperationHandle<IList<GameObject>> stairsHandle;
    private AsyncOperationHandle<IList<StairConnection>> connectionsHandle;

    public async UniTask<IEnumerable<GameObject>> LoadStairsAsync()
    {
        List<string> labels = new() { "stair", "prefab" };
        stairsHandle = Addressables.LoadAssetsAsync<GameObject>(
            labels,
            _ => { },
            Addressables.MergeMode.Intersection);
        await stairsHandle.ToUniTask();

        if (stairsHandle.Status == AsyncOperationStatus.Succeeded)
            return stairsHandle.Result;

        return null;
    }

    public async UniTask<IEnumerable<StairConnection>> LoadStairConnectionsAsync()
    {
        string label = "stair connection";
        connectionsHandle = Addressables.LoadAssetsAsync<StairConnection>(label);
        await connectionsHandle.ToUniTask();

        if (connectionsHandle.Status == AsyncOperationStatus.Succeeded)
            return connectionsHandle.Result;

        return null;
    }

    public void ReleaseStairs()
    {
        stairsHandle.Release();
        connectionsHandle.Release();
    }
}
