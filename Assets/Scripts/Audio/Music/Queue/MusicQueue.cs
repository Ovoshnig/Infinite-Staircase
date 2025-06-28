using System.Collections.Generic;
using Random = System.Random;

public class MusicQueue
{
    private readonly Random _random = new();
    private Queue<object> _clipKeysQueue = new();

    public void SetClipKeys(IEnumerable<object> clips)
    {
        _clipKeysQueue.Clear();
        _clipKeysQueue = new Queue<object>(clips);
    }

    public object GetNextClipKey() => _clipKeysQueue.TryDequeue(out var key) ? key : null;

    public void ShuffleClipKeys()
    {
        List<object> clips = new(_clipKeysQueue);
        _clipKeysQueue.Clear();

        while (clips.Count > 0)
        {
            int index = _random.Next(clips.Count);
            _clipKeysQueue.Enqueue(clips[index]);
            clips.RemoveAt(index);
        }
    }
}
