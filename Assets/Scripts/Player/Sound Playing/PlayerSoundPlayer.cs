using Cysharp.Threading.Tasks;
using R3;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;

public class PlayerSoundPlayer : IDisposable
{
    private readonly PlayerSoundLoader _soundLoader = new();
    private readonly Subject<(AudioResource, AudioResource)> _resourcesLoaded = new();
    private readonly CancellationTokenSource _cts = new();

    private AssetReference _footstepReference;
    private AssetReference _landReference;
    private AudioResource _footstepResource;
    private AudioResource _landResource;

    public Observable<(AudioResource, AudioResource)> ResourcesLoaded => _resourcesLoaded;

    public void SetReferences(AssetReference footstepReference, AssetReference landReference)
    {
        _footstepReference = footstepReference;
        _landReference = landReference;

        LoadResources().Forget();
    }

    private async UniTask LoadResources()
    {
        try
        {
            (_footstepResource, _landResource) = await _soundLoader
                .LoadSoundsAsync(_footstepReference, _landReference, _cts.Token);
            _resourcesLoaded.OnNext((_footstepResource, _landResource));
        }
        catch (OperationCanceledException)
        {
            return;
        }
    }

    public void Dispose()
    {
        _cts?.CancelAndDispose();

        _soundLoader.ReleaseSounds();
        Resources.UnloadAsset(_footstepResource);
        Resources.UnloadAsset(_landResource);
    }
}
