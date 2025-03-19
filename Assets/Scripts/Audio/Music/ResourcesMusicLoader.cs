using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public sealed class ResourcesMusicLoader : IMusicLoader
{
    public async UniTask<AudioClip> LoadClipAsync(string path, CancellationToken cancellationToken)
    {
        var request = Resources.LoadAsync<AudioClip>(path);

        return await request.ToUniTask(cancellationToken: cancellationToken) as AudioClip;
    }
}
