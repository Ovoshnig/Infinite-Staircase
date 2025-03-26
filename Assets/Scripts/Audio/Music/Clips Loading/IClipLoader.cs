using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public interface IClipLoader
{
    UniTask<Dictionary<MusicCategory, IEnumerable<object>>> LoadClipKeysAsync(CancellationToken token);

    UniTask<AudioClip> LoadClipAsync(object address, CancellationToken cancellationToken);

    void UnloadClip(AudioClip clip, CancellationToken cancellationToken);
}
