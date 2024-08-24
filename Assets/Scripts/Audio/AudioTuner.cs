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

    public ReactiveProperty<float> SoundVolume
    {
        get => _soundVolume;
        set => _soundVolume.Value = Math.Clamp(value.Value, _audioSettings.MinVolume, _audioSettings.MaxVolume);
    }

    public ReactiveProperty<float> MusicVolume
    {
        get => _musicVolume;
        set => _musicVolume.Value = Math.Clamp(value.Value, _audioSettings.MinVolume, _audioSettings.MaxVolume);
    }

    public void Initialize()
    {
        SoundVolume.Value = _settingsSaver.LoadData(SettingsConstants.SoundVolumeKey, 
            _audioSettings.DefaultVolume);
        MusicVolume.Value = _settingsSaver.LoadData(SettingsConstants.MusicVolumeKey, 
            _audioSettings.DefaultVolume);
    }

    public void Dispose()
    {
        _settingsSaver.SaveData(SettingsConstants.SoundVolumeKey, SoundVolume.Value);
        _settingsSaver.SaveData(SettingsConstants.MusicVolumeKey, MusicVolume.Value);
    }
}
