using R3;
using System;
using VContainer.Unity;

public class AudioTuner : IPostInitializable, IDisposable
{
    private readonly SettingsStorage _settingsStorage;
    private readonly AudioSettings _audioSettings;
    private readonly ReactiveProperty<float> _soundVolume = new();
    private readonly ReactiveProperty<float> _musicVolume = new();

    public AudioTuner(SettingsStorage settingsStorage,
        AudioSettings audioSettings)
    {
        _settingsStorage = settingsStorage;
        _audioSettings = audioSettings;
    }

    public ReadOnlyReactiveProperty<float> SoundVolume => _soundVolume;
    public ReadOnlyReactiveProperty<float> MusicVolume => _musicVolume;

    public void PostInitialize()
    {
        float soundVolume = _settingsStorage.Get(SettingsConstants.SoundVolumeKey, _audioSettings.DefaultVolume);
        SetSoundVolume(soundVolume);

        float musicVolume = _settingsStorage.Get(SettingsConstants.MusicVolumeKey, _audioSettings.DefaultVolume);
        SetMusicVolume(musicVolume);
    }

    public void Dispose()
    {
        _settingsStorage.Set(SettingsConstants.SoundVolumeKey, _soundVolume.Value);
        _settingsStorage.Set(SettingsConstants.MusicVolumeKey, _musicVolume.Value);
    }

    public void SetSoundVolume(float value)
    {
        value = Math.Clamp(value, _audioSettings.MinVolume, _audioSettings.MaxVolume);
        _soundVolume.Value = value;
    }

    public void SetMusicVolume(float value)
    {
        value = Math.Clamp(value, _audioSettings.MinVolume, _audioSettings.MaxVolume);
        _musicVolume.Value = value;
    }
}
