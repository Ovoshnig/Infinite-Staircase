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

        _random = new Random();
    }

    private void Start() => _footstepClips = Resources.LoadAll<AudioClip>(ResourcesConstants.PlayerFootstepPath);

    private void OnDestroy()
    {
        _playerState.WalkStarted -= OnWalkStarted;
        _playerState.WalkEnded -= OnWalkEnded;

        CancellToken();
    }

    private void OnWalkStarted() => PlayFootsteps().Forget();

    private void OnWalkEnded() => CancellToken();

    private async UniTask PlayFootsteps()
    {
        _cts = new CancellationTokenSource();

        while (isActiveAndEnabled)
        {
            int index = _random.Next(0, _footstepClips.Length);
            AudioClip clip = _footstepClips[index];
            _audioSource.clip = clip;
            _audioSource.Play();
            await UniTask.WaitWhile(() => _audioSource.isPlaying, cancellationToken: _cts.Token);
            float delay = _playerState.IsRunning ? clip.length / 5f : clip.length;
            await UniTask.WaitForSeconds(delay, cancellationToken: _cts.Token);
        }
    }

    private void CancellToken()
    { 
        _cts.Cancel(); 
        _cts.Dispose();
    }
}
