using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public interface IMusicLoader
{
    UniTask<AudioClip> LoadClipAsync(string path, CancellationToken cancellationToken);
}
