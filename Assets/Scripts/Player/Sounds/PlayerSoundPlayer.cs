using Random = System.Random;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerSoundPlayer : MonoBehaviour
{
    [SerializeField] private PlayerState _playerState;

    private AudioSource _audioSource;
    private AudioClip[] _footstepClips;
    private AudioClip[] _landClips;
    private Random _random;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        _playerState.GroundEntered += OnGroundEntered;

        _random = new Random();
    }

    private void Start()
    {
        _footstepClips = Resources.LoadAll<AudioClip>(ResourcesConstants.PlayerFootstepPath);
        _landClips = Resources.LoadAll<AudioClip>(ResourcesConstants.PlayerLandPath);
    }

    private void OnDestroy() => _playerState.GroundEntered -= OnGroundEntered;

    public void PlayStepSound()
    {
        AudioClip clip = GetRandomClip(_footstepClips);
        _audioSource.PlayOneShot(clip);
    }

    private void OnGroundEntered() => PlayLandingSound();

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
