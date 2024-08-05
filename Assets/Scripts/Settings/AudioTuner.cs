using System;
using UnityEngine.Audio;
using Zenject;

public class AudioTuner : IInitializable, IDisposable
{
    private const string SoundsVolumeKey = "SoundsVolume";
    private const string MusicVolumeKey = "MusicVolume";

    private readonly SettingsSaver _settingsSaver;
    private readonly GameSettingsInstaller.AudioSettings _audioSettings;

    private readonly AudioMixerGroup _audioMixerGroup;
    private float _soundsVolume;
    private float _musicVolume;

    [Inject]
    public AudioTuner(SettingsSaver settingsSaver, GameSettingsInstaller.AudioSettings audioSettings,
                         AudioMixerGroup audioMixerGroup)
    {
        _settingsSaver = settingsSaver;
        _audioSettings = audioSettings;
        _audioMixerGroup = audioMixerGroup;
    }

    public float SoundsVolume
    {
        get
        {
            return _soundsVolume;
        }
        set
        {
            if (value >= _audioSettings.MinVolume && value <= _audioSettings.MaxVolume)
                _soundsVolume = value;
        }
    }
    
    public float MusicVolume
    {
        get
        {
            return _musicVolume;
        }
        set
        {
            if (value >= _audioSettings.MinVolume && value <= _audioSettings.MaxVolume)
            {
                _musicVolume = value;
                _audioMixerGroup.audioMixer.SetFloat(MusicVolumeKey, value);
            }
        }
    }

    public void Initialize()
    {
        _soundsVolume = _settingsSaver.LoadData(SoundsVolumeKey, _audioSettings.DefaultVolume);
        _musicVolume = _settingsSaver.LoadData(MusicVolumeKey, _audioSettings.DefaultVolume);
        SoundsVolume = _soundsVolume;
        MusicVolume = _musicVolume;
    }

    public void Dispose() => SaveVolumeData();

    public void PauseSoundSources() => SetSoundSourcesPauseState(pause: true);

    public void UnpauseSoundSources() => SetSoundSourcesPauseState(pause: false);

    private void SaveVolumeData()
    {
        _settingsSaver.SaveData(SoundsVolumeKey, _soundsVolume);
        _settingsSaver.SaveData(MusicVolumeKey, _musicVolume);
    }

    private void SetSoundSourcesPauseState(bool pause)
    {
        if (pause)
            _audioMixerGroup.audioMixer.SetFloat(SoundsVolumeKey, _audioSettings.MinVolume);
        else
            _audioMixerGroup.audioMixer.SetFloat(SoundsVolumeKey, _soundsVolume);
    }
}
