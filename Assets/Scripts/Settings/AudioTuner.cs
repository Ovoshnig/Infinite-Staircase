using System;
using UnityEngine.Audio;
using Zenject;

public class AudioTuner : IInitializable, IDisposable
{
    private const string SoundsVolumeKey = "SoundsVolume";
    private const string MusicVolumeKey = "MusicVolume";

    private readonly DataSaver _dataSaver;
    private readonly GameSettingsInstaller.AudioSettings _audioSettings;

    private readonly GamePauser _gamePauser;
    private readonly AudioMixerGroup _audioMixerGroup;
    private float _soundsVolume;
    private float _musicVolume;

    [Inject]
    public AudioTuner(DataSaver dataSaver, GameSettingsInstaller.AudioSettings audioSettings,
                         GamePauser gamePauser, AudioMixerGroup audioMixerGroup)
    {
        _dataSaver = dataSaver;
        _audioSettings = audioSettings;
        _gamePauser = gamePauser;
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
        InitializeVolumeData();
        SubscribeToEvents();
    }

    public void Dispose()
    {
        UnsubscribeFromEvents();
        SaveVolumeData();
    }

    public void PauseSoundSources() => SetSoundSourcesPauseState(pause: true);

    public void UnpauseSoundSources() => SetSoundSourcesPauseState(pause: false);

    private void InitializeVolumeData()
    {
        _soundsVolume = _dataSaver.LoadData(SoundsVolumeKey, _audioSettings.DefaultVolume);
        _musicVolume = _dataSaver.LoadData(MusicVolumeKey, _audioSettings.DefaultVolume);
        SoundsVolume = _soundsVolume;
        MusicVolume = _musicVolume;
    }

    private void SubscribeToEvents()
    {
        _gamePauser.GamePaused += PauseSoundSources;
        _gamePauser.GameUnpaused += UnpauseSoundSources;
    }

    private void UnsubscribeFromEvents()
    {
        _gamePauser.GamePaused -= PauseSoundSources;
        _gamePauser.GameUnpaused -= UnpauseSoundSources;
    }

    private void SaveVolumeData()
    {
        _dataSaver.SaveData(SoundsVolumeKey, _soundsVolume);
        _dataSaver.SaveData(MusicVolumeKey, _musicVolume);
    }

    private void SetSoundSourcesPauseState(bool pause)
    {
        if (pause)
            _audioMixerGroup.audioMixer.SetFloat(SoundsVolumeKey, _audioSettings.MinVolume);
        else
            _audioMixerGroup.audioMixer.SetFloat(SoundsVolumeKey, _soundsVolume);
    }
}
