using R3;
using System;
using UnityEngine.Audio;
using VContainer.Unity;

public class AudioMixerTuner : IStartable, IDisposable
{
    private readonly AudioMixerGroup _audioMixerGroup;
    private readonly SoundVolumeKeeper _soundVolumeKeeper;
    private readonly MusicVolumeKeeper _musicVolumeKeeper;
    private readonly AudioSettings _audioSettings;
    private readonly GamePauser _gamePauser;
    private readonly CompositeDisposable _compositeDisposable = new();

    public AudioMixerTuner(AudioMixerGroup audioMixerGroup, 
        SoundVolumeKeeper soundVolumeKeeper, MusicVolumeKeeper musicVolumeKeeper,
        AudioSettings audioSettings, GamePauser gamePauser)
    {
        _audioMixerGroup = audioMixerGroup;
        _soundVolumeKeeper = soundVolumeKeeper;
        _musicVolumeKeeper = musicVolumeKeeper;
        _audioSettings = audioSettings;
        _gamePauser = gamePauser;
    }

    private AudioMixer AudioMixer => _audioMixerGroup.audioMixer;

    public void Start()
    {
        _soundVolumeKeeper.Data
            .Subscribe(value => 
            AudioMixer.SetFloat(AudioMixerConstants.SoundGroupName, value))
            .AddTo(_compositeDisposable);
        _musicVolumeKeeper.Data
            .Subscribe(value =>
            AudioMixer.SetFloat(AudioMixerConstants.MusicGroupName, value))
            .AddTo(_compositeDisposable);
        _gamePauser.IsPause
            .Subscribe(value => OnPauseValueChanged(value))
            .AddTo(_compositeDisposable);
    }

    public void Dispose() => _compositeDisposable?.Dispose();

    private void OnPauseValueChanged(bool pause)
    {
        AudioMixerSnapshot snapshot = AudioMixer.FindSnapshot(
            pause ? AudioMixerConstants.PauseSnapshotName : AudioMixerConstants.NormalSnapshotName);
        snapshot.TransitionTo(_audioSettings.SnapshotTransitionDuration);
    }
}
