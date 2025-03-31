using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using R3;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;
using VContainer;

[RequireComponent(typeof(AudioSource))]
public class PlayerSoundPlayer : MonoBehaviour
{
    [SerializeField] private AssetReference _footstepReference;
    [SerializeField] private AssetReference _landReference;

    private PlayerState _playerState;
	private AudioSource _audioSource;
    private PlayerSoundLoader _soundLoader;
    private AudioResource _footstepResource;
    private AudioResource _landResource;

    [Inject]
    public void Construct(PlayerState playerState) => _playerState = playerState;

    private void Awake() 
    {
        _audioSource = GetComponent<AudioSource>();

        _soundLoader = new PlayerSoundLoader();
    }

    private async void Start()
    {
        using CancellationTokenSource cts = new();
        var (footstepResource, landResource) = await _soundLoader
            .LoadSoundsAsync(_footstepReference, _landReference, cts.Token);
        _footstepResource = footstepResource;
        _landResource = landResource;

        _playerState.IsGrounded
            .Where(value => value)
            .Subscribe(_ => PlayLandSound())
            .AddTo(this);
    }

    private void OnDestroy()
    {
        _soundLoader.ReleaseSounds();
        Resources.UnloadAsset(_footstepResource);
        Resources.UnloadAsset(_landResource);
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
