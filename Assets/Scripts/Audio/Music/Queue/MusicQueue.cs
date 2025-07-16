using System.Collections.Generic;
using System.Linq;
using Random = System.Random;

public class MusicQueue
{
    private readonly Random _random = new();

    private Queue<object> _clipKeysQueue = new();

    public void Clear() => _clipKeysQueue.Clear();

    public void SetClipKeys(IEnumerable<object> clips) => _clipKeysQueue = new Queue<object>(clips);

    public bool TryGetNextClipKey(out object key) => _clipKeysQueue.TryDequeue(out key);

    public void ShuffleClipKeys()
    {
        List<object> clips = new(_clipKeysQueue);
        _clipKeysQueue.Clear();

        while (clips.Any())
        {
            int index = _random.Next(clips.Count);
            _clipKeysQueue.Enqueue(clips[index]);
            clips.RemoveAt(index);
        }
    }
}
