using Cysharp.Threading.Tasks;
using Random = System.Random;
using UnityEngine;
using System.Threading;

[RequireComponent(typeof(AudioSource),
                  typeof(PlayerState))]
public class PlayerSoundPlayer : MonoBehaviour
{
    private AudioSource _audioSource;
    private PlayerState _playerState;
    private AudioClip[] _footstepClips;
    private Random _random;
    private CancellationTokenSource _cts;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _playerState = GetComponent<PlayerState>();

        _playerState.WalkStarted += OnWalkStarted;
        _playerState.WalkEnded += OnWalkEnded;
        _playerState.GroundEntered += OnGroundEntered;

        _random = new Random();
    }

    private void Start() => _footstepClips = Resources.LoadAll<AudioClip>(ResourcesConstants.PlayerFootstepPath);

    private void OnDestroy()
    {
        _playerState.WalkStarted -= OnWalkStarted;
        _playerState.WalkEnded -= OnWalkEnded;
        _playerState.GroundEntered -= OnGroundEntered;

        _cts?.CancelAndDispose(ref _cts);
    }

    private void OnWalkStarted()
    {
        _cts = new CancellationTokenSource();
        PlayFootsteps().Forget();
    }

    private void OnWalkEnded() => _cts?.CancelAndDispose(ref _cts);

    private void OnGroundEntered() => PlayLanding();

    private async UniTask PlayFootsteps()
    {
        while (isActiveAndEnabled && !_playerState.IsInAir)
        {
            AudioClip clip = GetRandomClip();
            _audioSource.clip = clip;
            _audioSource.Play();

            if (_cts.IsNullOrCanceled())
                return;

            await UniTask.WaitWhile(() => _audioSource.isPlaying, cancellationToken: _cts.Token);
            float delay = _playerState.IsRunning ? clip.length / 5f : clip.length;

            if (_cts.IsNullOrCanceled())
                return;

            await UniTask.WaitForSeconds(delay, cancellationToken: _cts.Token);
        }
    }

    private void PlayLanding()
    {
        AudioClip clip = GetRandomClip();
        _audioSource.clip = clip;
        _audioSource.Play();
    }

    private AudioClip GetRandomClip()
    {
        int index = _random.Next(0, _footstepClips.Length);
        AudioClip clip = _footstepClips[index];

        return clip;
    }
}
