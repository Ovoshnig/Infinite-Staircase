using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class PlayerSoundPlayerView : MonoBehaviour
{
    [SerializeField] private AssetReference _footstepReference;
    [SerializeField] private AssetReference _landReference;

    private AudioSource _audioSource;
    private AudioResource _footstepResource;
    private AudioResource _landResource;

    public AssetReference FootstepReference => _footstepReference;
    public AssetReference LandReference => _landReference;

    private AudioSource AudioSource
    {
        get
        {
            if (_audioSource == null)
                _audioSource = GetComponent<AudioSource>();

            return _audioSource;
        }
    }

    public void SetResources(AudioResource footstepResource, AudioResource landResource)
    {
        _footstepResource = footstepResource;
        _landResource = landResource;
    }

    public void PlayStepSound()
    {
        AudioSource.resource = _footstepResource;
        AudioSource.Play();
    }

    public void PlayLandSound()
    {
        AudioSource.resource = _landResource;
        AudioSource.Play();
    }
}
