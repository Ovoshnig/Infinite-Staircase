using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayerView : MonoBehaviour
{
    private AudioSource _audioSource = null;

    public bool IsPlaying => AudioSource.isPlaying;

    private AudioSource AudioSource
    {
        get 
        {
            if (_audioSource == null)
                _audioSource = GetComponent<AudioSource>();

            return _audioSource;
        }
    }

    public void SetClip(AudioClip clip) => AudioSource.clip = clip;

    public void Play() => AudioSource.Play();

    public void Stop() => AudioSource.Stop();
}
