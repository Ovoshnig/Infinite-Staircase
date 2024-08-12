using System;
using UnityEngine.Audio;
using Zenject;

public class AudioTuner : IInitializable, IDisposable
{
    private readonly SettingsSaver _settingsSaver;
    private readonly GameSettingsInstaller.AudioSettings _audioSettings;

    private readonly AudioMixerGroup _audioMixerGroup;
    private readonly GamePauser _gamePauser;
    private float _soundVolume;
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

    public float SoundVolume
    {
        get
        {
            return _soundVolume;
        }
        set
        {
            if (value >= _audioSettings.MinVolume && value <= _audioSettings.MaxVolume)
            { 
                _soundVolume = value;
                _audioMixerGroup.audioMixer.SetFloat(AudioMixerConstants.SoundGroupName, value);
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
                _audioMixerGroup.audioMixer.SetFloat(AudioMixerConstants.MusicGroupName, value);
            }
        }
    }

    public void Initialize()
    {
        _soundVolume = _settingsSaver.LoadData(SettingsConstants.SoundVolumeKey, _audioSettings.DefaultVolume);
        _musicVolume = _settingsSaver.LoadData(SettingsConstants.MusicVolumeKey, _audioSettings.DefaultVolume);
        SoundVolume = _soundVolume;
        MusicVolume = _musicVolume;

        _gamePauser.Paused += OnPaused;
        _gamePauser.Unpaused += OnUnpaused;
    }

    public void Dispose()
    {
        _settingsSaver.SaveData(SettingsConstants.SoundVolumeKey, _soundVolume);
        _settingsSaver.SaveData(SettingsConstants.MusicVolumeKey, _musicVolume);

        _gamePauser.Paused -= OnPaused;
        _gamePauser.Unpaused -= OnUnpaused;
    }

    private void OnPaused() => SetSoundPause(true);

    private void OnUnpaused() => SetSoundPause(false);

    private void SetSoundPause(bool pause)
    {
        var snapshot = _audioMixerGroup
            .audioMixer.FindSnapshot(
            pause ? AudioMixerConstants.PauseSnapshotName : AudioMixerConstants.NormalSnapshotName);
        snapshot.TransitionTo(_audioSettings.SnapshotTransitionDuration);
    }
}
