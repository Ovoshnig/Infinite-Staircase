using R3;
using Random = System.Random;
using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerSoundPlayer : MonoBehaviour
{
    [SerializeField] private PlayerState _playerState;

    private AudioSource _audioSource;
    private AudioClip[] _footstepClips;
    private AudioClip[] _landClips;
    private Random _random;
    private IDisposable _disposable;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        _footstepClips = Resources.LoadAll<AudioClip>(ResourcesConstants.PlayerFootstepPath);
        _landClips = Resources.LoadAll<AudioClip>(ResourcesConstants.PlayerLandPath);

        _random = new Random();

        _disposable = _playerState.IsGrounded
            .Where(value => !value)
            .Subscribe(_ => PlayLandingSound());
    }

    private void OnDestroy() => _disposable?.Dispose();

    private void PlayStepSound()
    {
        AudioClip clip = GetRandomClip(_footstepClips);
        _audioSource.PlayOneShot(clip);
    }

    private void PlayLandingSound()
    {
        AudioClip clip = GetRandomClip(_landClips);
        _audioSource.PlayOneShot(clip);
    }

    private AudioClip GetRandomClip(AudioClip[] clips)
    {
        int index = _random.Next(0, clips.Length);
        AudioClip clip = clips[index];

        return clip;
    }
}
