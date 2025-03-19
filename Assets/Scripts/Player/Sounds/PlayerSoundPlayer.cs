using R3;
using UnityEngine;
using VContainer;
using UnityEngine.Audio;
using JetBrains.Annotations;

[RequireComponent(typeof(AudioSource))]
public class PlayerSoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioResource _footstepResource;
    [SerializeField] private AudioResource _landResource;

    private readonly CompositeDisposable _compositeDisposable = new();
    private PlayerState _playerState;
    private AudioSource _audioSource;

    [Inject]
    public void Construct(PlayerState playerState) => _playerState = playerState;

    private void Awake() => _audioSource = GetComponent<AudioSource>();

    private void Start()
    {
        _playerState.IsGrounded
            .Where(value => value)
            .Subscribe(_ => PlayLandSound())
            .AddTo(_compositeDisposable);
    }

    private void OnDestroy() => _compositeDisposable?.Dispose();

    [UsedImplicitly]
    private void PlayStepSound()
    {
        _audioSource.resource = _footstepResource;
        _audioSource.Play();
    }

    private void PlayLandSound()
    {
        _audioSource.resource = _landResource;
        _audioSource.Play();
    }
}
