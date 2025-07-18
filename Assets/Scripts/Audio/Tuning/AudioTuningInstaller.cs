using UnityEngine.Audio;
using VContainer;
using VContainer.Unity;

public class AudioTuningInstaller : IInstaller
{
    private readonly AudioMixerGroup _audioMixerGroup;

    public AudioTuningInstaller(AudioMixerGroup audioMixerGroup) =>
        _audioMixerGroup = audioMixerGroup;

    public void Install(IContainerBuilder builder)
    {
        builder.RegisterInstance(_audioMixerGroup);
        builder.Register<AudioMixerTuner>(Lifetime.Singleton).AsSelf();
    }
}
