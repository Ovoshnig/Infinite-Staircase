using R3;
using Random = System.Random;
using System;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(AudioSource))]
public class PlayerSoundPlayer : MonoBehaviour
{
    private readonly CompositeDisposable _compositeDisposable = new();
    private PlayerState _playerState;
    private AudioSource _audioSource;
    private AudioClip[] _footstepClips;
    private AudioClip[] _landClips;
    private Random _random;

    [Inject]
    private void Construct([Inject(Id = ZenjectIdConstants.PlayerId)] PlayerState playerState) => 
        _playerState = playerState;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        _footstepClips = Resources.LoadAll<AudioClip>(ResourcesConstants.PlayerFootstepPath);
        _landClips = Resources.LoadAll<AudioClip>(ResourcesConstants.PlayerLandPath);

        _random = new Random();

        _playerState.IsGrounded
            .Where(value => value)
            .Subscribe(_ => PlayLandSound())
            .AddTo(_compositeDisposable);
    }

    private void OnDestroy() => _compositeDisposable?.Dispose();

    private void PlayStepSound()
    {
        AudioClip clip = GetRandomClip(_footstepClips);
        _audioSource.PlayOneShot(clip);
    }

    private void PlayLandSound()
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
