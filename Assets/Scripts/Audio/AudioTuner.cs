using R3;
using System;
using Zenject;

public class AudioTuner : IInitializable, IDisposable
{
    private readonly ReactiveProperty<float> _soundVolume = new();
    private readonly ReactiveProperty<float> _musicVolume = new();
    private readonly SettingsSaver _settingsSaver;
    private readonly GameSettingsInstaller.AudioSettings _audioSettings;
    private CompositeDisposable _compositeDisposable;

    [Inject]
    public AudioTuner(SettingsSaver settingsSaver, 
        GameSettingsInstaller.AudioSettings audioSettings)
    {
        _settingsSaver = settingsSaver;
        _audioSettings = audioSettings;
    }

    public ReactiveProperty<float> SoundVolume => _soundVolume;
    public ReactiveProperty<float> MusicVolume => _musicVolume;

    public void Initialize()
    {
        SoundVolume.Value = _settingsSaver.LoadData(SettingsConstants.SoundVolumeKey, 
            _audioSettings.DefaultVolume);

        MusicVolume.Value = _settingsSaver.LoadData(SettingsConstants.MusicVolumeKey, 
            _audioSettings.DefaultVolume);

        var soundDisposable= SoundVolume
            .Subscribe(value => 
            _soundVolume.Value = Math.Clamp(value, _audioSettings.MinVolume, _audioSettings.MaxVolume));

        var musicDisposable = MusicVolume
            .Subscribe(value =>
            _musicVolume.Value = Math.Clamp(value, _audioSettings.MinVolume, _audioSettings.MaxVolume));

        _compositeDisposable = new CompositeDisposable()
        {
            soundDisposable,
            musicDisposable
        };
    }

    public void Dispose()
    {
        _settingsSaver.SaveData(SettingsConstants.SoundVolumeKey, SoundVolume.Value);
        _settingsSaver.SaveData(SettingsConstants.MusicVolumeKey, MusicVolume.Value);

        _compositeDisposable?.Dispose();
    }
}
