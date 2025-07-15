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
        builder.RegisterEntryPoint<GamePauserAudioMixerTunerMediator>(Lifetime.Singleton);
        builder.RegisterEntryPoint<SoundSliderModel>(Lifetime.Singleton).AsSelf();
        builder.RegisterEntryPoint<MusicSliderModel>(Lifetime.Singleton).AsSelf();
        builder.Register<SliderAudioMixerTunerMediatorFactory>(Lifetime.Singleton);
    }
}
