using R3;
using System;
using UnityEngine.Audio;
using Zenject;

public class AudioMixerTuner : IInitializable, IDisposable
{
    private readonly AudioMixerGroup _audioMixerGroup;
    private readonly AudioTuner _audioTuner;
    private readonly GameSettingsInstaller.AudioSettings _audioSettings;
    private readonly GamePauser _gamePauser;
    private CompositeDisposable _compositeDisposable;

    [Inject]
    public AudioMixerTuner(AudioMixerGroup audioMixerGroup, AudioTuner audioTuner, 
        GameSettingsInstaller.AudioSettings audioSettings, GamePauser gamePauser)
    {
        _audioMixerGroup = audioMixerGroup;
        _audioTuner = audioTuner;
        _audioSettings = audioSettings;
        _gamePauser = gamePauser;
    }

    private AudioMixer AudioMixer => _audioMixerGroup.audioMixer;

    public void Initialize()
    {
        var soundDisposable = _audioTuner.SoundVolume
            .Subscribe(value => 
            AudioMixer.SetFloat(AudioMixerConstants.SoundGroupName, value));

        var musicDisposable = _audioTuner.MusicVolume
            .Subscribe(value =>
            AudioMixer.SetFloat(AudioMixerConstants.MusicGroupName, value));

        var pauseDisposable = _gamePauser.IsPause
            .Subscribe(value => SetSnapshot(value));

        _compositeDisposable = new CompositeDisposable()
        {
            soundDisposable,
            musicDisposable,
            pauseDisposable
        };
    }

    public void Dispose() => _compositeDisposable?.Dispose();

    private void SetSnapshot(bool pause)
    {
        var snapshot = AudioMixer.FindSnapshot(
            pause ? AudioMixerConstants.PauseSnapshotName : AudioMixerConstants.NormalSnapshotName);
        snapshot.TransitionTo(_audioSettings.SnapshotTransitionDuration);
    }
}
