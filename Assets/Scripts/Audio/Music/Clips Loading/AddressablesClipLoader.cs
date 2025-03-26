using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

public class AddressablesClipLoader : IClipLoader
{
    private AsyncOperationHandle<AudioClip> handle;

    public async UniTask<Dictionary<MusicCategory, IEnumerable<object>>> LoadClipKeysAsync(CancellationToken token)
    {
        Dictionary<MusicCategory, IEnumerable<object>> musicClipKeys = new();

        foreach (MusicCategory category in Enum.GetValues(typeof(MusicCategory)))
        {
            string categoryName = category.ToString();
            categoryName = Regex.Replace(categoryName, "(?<!^)([A-Z])", " $1").ToLower();
            List<string> labels = new() { "audio", "music", categoryName };

            AsyncOperationHandle<IList<IResourceLocation>> handle = Addressables.LoadResourceLocationsAsync(
                labels,
                Addressables.MergeMode.Intersection);
            await handle.ToUniTask(cancellationToken: token);
            IList<IResourceLocation> locations = handle.Result;
            musicClipKeys[category] = locations;
        }

        return musicClipKeys;
    }

    public async UniTask<AudioClip> LoadClipAsync(object address, CancellationToken token)
    {
        handle = Addressables.LoadAssetAsync<AudioClip>((IResourceLocation)address);
        await handle.ToUniTask(cancellationToken: token);

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            AudioClip clip = handle.Result;

            return clip;
        }

        return null;
    }

    public void UnloadClip(AudioClip clip, CancellationToken cancellationToken) => handle.Release();
}
