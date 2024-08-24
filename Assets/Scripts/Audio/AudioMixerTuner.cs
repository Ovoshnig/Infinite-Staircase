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
    private readonly CompositeDisposable _audioDisposable = new();

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
        _audioDisposable.Add(_audioTuner.SoundVolume
            .Subscribe(value => 
            AudioMixer.SetFloat(AudioMixerConstants.SoundGroupName, value)));
        _audioDisposable.Add(_audioTuner.MusicVolume
            .Subscribe(value =>
            AudioMixer.SetFloat(AudioMixerConstants.MusicGroupName, value)));

        _gamePauser.Paused += OnPaused;
        _gamePauser.Unpaused += OnUnpaused;
    }

    public void Dispose()
    {
        _audioDisposable?.Dispose();

        _gamePauser.Paused -= OnPaused;
        _gamePauser.Unpaused -= OnUnpaused;
    }

    private void OnPaused() => SetPause(true);

    private void OnUnpaused() => SetPause(false);

    private void SetPause(bool pause)
    {
        var snapshot = AudioMixer.FindSnapshot(
            pause ? AudioMixerConstants.PauseSnapshotName : AudioMixerConstants.NormalSnapshotName);
        snapshot.TransitionTo(_audioSettings.SnapshotTransitionDuration);
    }
}
