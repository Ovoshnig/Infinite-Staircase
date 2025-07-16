using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;
using UnityEngine.ResourceManagement.AsyncOperations;

public class PlayerSoundLoader
{
    private AudioResource _footstepResource;
    private AudioResource _landResource;
    private AsyncOperationHandle<AudioResource> _footstepHandle;
    private AsyncOperationHandle<AudioResource> _landHandle;
    private readonly CancellationTokenSource _cts = new();

    public async UniTask<(AudioResource footstepResource, AudioResource landResource)> LoadSoundsAsync(
        AssetReference footstepReference, AssetReference landReference)
    {
        try
        {
            _footstepHandle = Addressables.LoadAssetAsync<AudioResource>(footstepReference);
            await _footstepHandle.ToUniTask(cancellationToken: _cts.Token);

            if (_footstepHandle.Status == AsyncOperationStatus.Succeeded)
                _footstepResource = _footstepHandle.Result;

            _landHandle = Addressables.LoadAssetAsync<AudioResource>(landReference);
            await _landHandle.ToUniTask(cancellationToken: _cts.Token);

            if (_landHandle.Status == AsyncOperationStatus.Succeeded)
                _landResource = _landHandle.Result;

            return (_footstepResource, _landResource);
        }
        catch (OperationCanceledException)
        {
            return default;
        }
    }

    public void ReleaseSounds()
    {
        _cts.Cancel();
        _cts.Dispose();

        if (_footstepHandle.IsValid())
            _footstepHandle.Release();

        if (_landHandle.IsValid())
            _landHandle.Release();

        Resources.UnloadAsset(_footstepResource);
        Resources.UnloadAsset(_landResource);
    }
}
