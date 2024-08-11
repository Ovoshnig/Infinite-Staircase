using System;
using UnityEngine.Audio;
using Zenject;

public class AudioTuner : IInitializable, IDisposable
{
    private readonly SettingsSaver _settingsSaver;
    private readonly GameSettingsInstaller.AudioSettings _audioSettings;

    private readonly AudioMixerGroup _audioMixerGroup;
    private readonly GamePauser _gamePauser;
    private float _soundsVolume;
    private float _musicVolume;

    [Inject]
    public AudioTuner(SettingsSaver settingsSaver, GameSettingsInstaller.AudioSettings audioSettings,
                         AudioMixerGroup audioMixerGroup, GamePauser gamePauser)
    {
        _settingsSaver = settingsSaver;
        _audioSettings = audioSettings;
        _audioMixerGroup = audioMixerGroup;
        _gamePauser = gamePauser;
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
            { 
                _soundsVolume = value;
                _audioMixerGroup.audioMixer.SetFloat(SettingsConstants.SoundsVolumeKey, value);
            }
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
                _audioMixerGroup.audioMixer.SetFloat(SettingsConstants.MusicVolumeKey, value);
            }
        }
    }

    public void Initialize()
    {
        _soundsVolume = _settingsSaver.LoadData(SettingsConstants.SoundsVolumeKey, _audioSettings.DefaultVolume);
        _musicVolume = _settingsSaver.LoadData(SettingsConstants.MusicVolumeKey, _audioSettings.DefaultVolume);
        SoundsVolume = _soundsVolume;
        MusicVolume = _musicVolume;

        _gamePauser.Paused += OnPaused;
        _gamePauser.Unpaused += OnUnpaused;
    }

    public void Dispose()
    {
        _settingsSaver.SaveData(SettingsConstants.SoundsVolumeKey, _soundsVolume);
        _settingsSaver.SaveData(SettingsConstants.MusicVolumeKey, _musicVolume);

        _gamePauser.Paused -= OnPaused;
        _gamePauser.Unpaused -= OnUnpaused;
    }

    private void OnPaused() => SetSoundSourcesPause(true);

    private void OnUnpaused() => SetSoundSourcesPause(false);

    private void SetSoundSourcesPause(bool pause) => _audioMixerGroup.audioMixer
        .SetFloat(SettingsConstants.SoundsVolumeKey,
        pause ? _audioSettings.MinVolume : _soundsVolume);
}
