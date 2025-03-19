using R3;
using System;
using VContainer.Unity;

public class AudioTuner : IInitializable, IDisposable
{
    private readonly ReactiveProperty<float> _soundVolume = new();
    private readonly ReactiveProperty<float> _musicVolume = new();
    private readonly SettingsStorage _settingsStorage;
    private readonly AudioSettings _audioSettings;
    private readonly CompositeDisposable _compositeDisposable = new();

    public AudioTuner(SettingsStorage settingsStorage, 
        AudioSettings audioSettings)
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

        SoundVolume
            .Subscribe(value => 
            _soundVolume.Value = Math.Clamp(value, _audioSettings.MinVolume, _audioSettings.MaxVolume))
            .AddTo(_compositeDisposable);

        MusicVolume
            .Subscribe(value =>
            _musicVolume.Value = Math.Clamp(value, _audioSettings.MinVolume, _audioSettings.MaxVolume))
            .AddTo(_compositeDisposable);
    }

    public void Dispose()
    {
        _settingsStorage.Set(SettingsConstants.SoundVolumeKey, _soundVolume.Value);
        _settingsStorage.Set(SettingsConstants.MusicVolumeKey, _musicVolume.Value);

        _compositeDisposable?.Dispose();
    }
}
