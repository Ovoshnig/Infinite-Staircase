using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using Object = UnityEngine.Object;

public sealed class ResourcesClipLoader : IClipLoader
{
    public async UniTask<Dictionary<MusicCategory, IEnumerable<object>>> LoadClipKeysAsync(CancellationToken token)
    {
        Dictionary<MusicCategory, IEnumerable<object>> musicClipKeys = new();
        Object result = await Resources
            .LoadAsync<TextAsset>(ResourcesConstants.ResourcesListName)
            .ToUniTask(cancellationToken: token);
        TextAsset resourcesList = (TextAsset)result;

        if (resourcesList == null)
        {
            Debug.LogError($"Failed to load the resource list.");

            return musicClipKeys;
        }

        char[] separators = { '\r', '\n' };
        string extension = ".mp3";

        foreach (MusicCategory category in Enum.GetValues(typeof(MusicCategory)))
        {
            string categoryPath = $"{ResourcesConstants.MusicPath}/{category}";

            IEnumerable<object> clipKeys = new List<object>(resourcesList.text
                .Split(separators, StringSplitOptions.RemoveEmptyEntries)
                .Where(c => c.StartsWith(categoryPath) && c.EndsWith(extension))
                .Select(c => c[..^4]));

            musicClipKeys[category] = clipKeys;
        }

        return musicClipKeys;
    }

    public async UniTask<AudioClip> LoadClipAsync(object address, CancellationToken cancellationToken)
    {
        ResourceRequest request = Resources.LoadAsync<AudioClip>(address.ToString());

        return await request.ToUniTask(cancellationToken: cancellationToken) as AudioClip;
    }

    public void UnloadClip(AudioClip clip) => Resources.UnloadAsset(clip);
}
