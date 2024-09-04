using R3;
using System;
using Zenject;

public class AudioTuner : IInitializable, IDisposable
{
    private readonly ReactiveProperty<float> _soundVolume = new();
    private readonly ReactiveProperty<float> _musicVolume = new();
    private readonly SettingsStorage _settingsStorage;
    private readonly GameSettingsInstaller.AudioSettings _audioSettings;
    private CompositeDisposable _compositeDisposable;

    [Inject]
    public AudioTuner(SettingsStorage settingsStorage, 
        GameSettingsInstaller.AudioSettings audioSettings)
    {
        _settingsStorage = settingsStorage;
        _audioSettings = audioSettings;
    }

    public ReactiveProperty<float> SoundVolume => _soundVolume;
    public ReactiveProperty<float> MusicVolume => _musicVolume;

    public void Initialize()
    {
        _soundVolume.Value = _settingsStorage.Get(SettingsConstants.SoundVolumeKey, 
            _audioSettings.DefaultVolume);

        _musicVolume.Value = _settingsStorage.Get(SettingsConstants.MusicVolumeKey, 
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
        _settingsStorage.Set(SettingsConstants.SoundVolumeKey, _soundVolume.Value);
        _settingsStorage.Set(SettingsConstants.MusicVolumeKey, _musicVolume.Value);

        _compositeDisposable?.Dispose();
    }
}
