using UnityEngine.Audio;

public class AudioMixerTuner
{
    private readonly AudioMixerGroup _audioMixerGroup;
    private readonly AudioSettings _audioSettings;

    public AudioMixerTuner(AudioMixerGroup audioMixerGroup,
        AudioSettings audioSettings)
    {
        _audioMixerGroup = audioMixerGroup;
        _audioSettings = audioSettings;
    }

    private AudioMixer AudioMixer => _audioMixerGroup.audioMixer;

    public bool SetSoundVolume(float value) => AudioMixer.SetFloat(AudioMixerConstants.SoundGroupName, value);

    public bool SetMusicVolume(float value) => AudioMixer.SetFloat(AudioMixerConstants.MusicGroupName, value);

    public void SetPause(bool value)
    {
        AudioMixerSnapshot snapshot = AudioMixer.FindSnapshot(value
            ? AudioMixerConstants.PauseSnapshotName
            : AudioMixerConstants.NormalSnapshotName);
        snapshot.TransitionTo(_audioSettings.SnapshotTransitionDuration);
    }
}
