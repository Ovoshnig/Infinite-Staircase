using JetBrains.Annotations;
using R3;
using UnityEngine;
using UnityEngine.Audio;
using VContainer;

[RequireComponent(typeof(AudioSource))]
public class PlayerSoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioResource _footstepResource;
    [SerializeField] private AudioResource _landResource;

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
            .AddTo(this);
    }

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
