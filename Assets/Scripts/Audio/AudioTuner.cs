using R3;
using System;
using Zenject;

public class AudioTuner : IInitializable, IDisposable
{
    private readonly ReactiveProperty<float> _soundVolume = new();
    private readonly ReactiveProperty<float> _musicVolume = new();
    private readonly SettingsSaver _settingsSaver;
    private readonly GameSettingsInstaller.AudioSettings _audioSettings;

    [Inject]
    public AudioTuner(SettingsSaver settingsSaver, 
        GameSettingsInstaller.AudioSettings audioSettings)
    {
        _settingsSaver = settingsSaver;
        _audioSettings = audioSettings;
    }

    public float SoundVolume
    {
        get => _soundVolume.Value;
        set => _soundVolume.Value = Math.Clamp(value, _audioSettings.MinVolume, _audioSettings.MaxVolume);
    }

    public float MusicVolume
    {
        get => _musicVolume.Value;
        set => _musicVolume.Value = Math.Clamp(value, _audioSettings.MinVolume, _audioSettings.MaxVolume);
    }

    public ReadOnlyReactiveProperty<float> SoundVolumeReactive => _soundVolume;
    public ReadOnlyReactiveProperty<float> MusicVolumeReactive => _musicVolume;

    public void Initialize()
    {
        SoundVolume = _settingsSaver.LoadData(SettingsConstants.SoundVolumeKey, 
            _audioSettings.DefaultVolume);
        MusicVolume = _settingsSaver.LoadData(SettingsConstants.MusicVolumeKey, 
            _audioSettings.DefaultVolume);
    }

    public void Dispose()
    {
        _settingsSaver.SaveData(SettingsConstants.SoundVolumeKey, SoundVolume);
        _settingsSaver.SaveData(SettingsConstants.MusicVolumeKey, MusicVolume);
    }
}
