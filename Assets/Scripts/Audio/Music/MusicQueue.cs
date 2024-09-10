using Random = System.Random;
using System.Collections.Generic;
using System.Linq;

public class MusicQueue
{
    private readonly Random _random = new();
    private Queue<string> _clipPathsQueue = new();

    public void SetClips(IEnumerable<string> clips)
    {
        _clipPathsQueue.Clear();
        _clipPathsQueue = new Queue<string>(clips);
    }

    public string GetNextClip()
    {
        if (_clipPathsQueue.Count == 0)
            return null;

        return _clipPathsQueue.Dequeue();
    }

    public void Shuffle()
    {
        var clips = _clipPathsQueue.ToList();
        _clipPathsQueue.Clear();

        while (clips.Count > 0)
        {
            int index = _random.Next(clips.Count);
            _clipPathsQueue.Enqueue(clips[index]);
            clips.RemoveAt(index);
        }
    }
}
