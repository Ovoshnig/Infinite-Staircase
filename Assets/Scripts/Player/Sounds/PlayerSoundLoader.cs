using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;
using UnityEngine.ResourceManagement.AsyncOperations;

public class PlayerSoundLoader
{
    AsyncOperationHandle<AudioResource> _footstepHandle;
    AsyncOperationHandle<AudioResource> _landHandle;

    public async UniTask<(AudioResource footstepResource, AudioResource landResource)> LoadSoundsAsync(
        AssetReference footstepReference, AssetReference landReference, CancellationToken token)
    {
        AudioResource footstepResource = null;
        AudioResource landResource = null;

        _footstepHandle = Addressables.LoadAssetAsync<AudioResource>(footstepReference);
        await _footstepHandle.ToUniTask(cancellationToken: token);
        if (_footstepHandle.Status == AsyncOperationStatus.Succeeded)
            footstepResource = _footstepHandle.Result;

        _landHandle = Addressables.LoadAssetAsync<AudioResource>(landReference);
        await _landHandle.ToUniTask(cancellationToken: token);
        if (_landHandle.Status == AsyncOperationStatus.Succeeded)
            landResource = _landHandle.Result;

        return (footstepResource, landResource);
    }

    public void ReleaseSounds()
    {
        _footstepHandle.Release();
        _landHandle.Release();
    }
}
