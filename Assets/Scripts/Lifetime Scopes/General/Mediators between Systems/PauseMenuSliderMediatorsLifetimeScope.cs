using UnityEngine;
using VContainer;
using VContainer.Unity;

public class PauseMenuSliderMediatorsLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint(resolver =>
        {
            SensitivitySliderModel sensitivityModel = resolver.Resolve<SensitivitySliderModel>();

            Canvas windowCanvas = resolver.Resolve<Canvas>();
            SensitivitySliderView sensitivityView = windowCanvas
                .GetComponentInChildren<SensitivitySliderView>(includeInactive: true);

            return new SliderMediator(sensitivityModel, sensitivityView);
        }, Lifetime.Scoped);

        builder.RegisterEntryPoint(resolver =>
        {
            SoundSliderModel soundModel = resolver.Resolve<SoundSliderModel>();

            Canvas windowCanvas = resolver.Resolve<Canvas>();
            SoundSliderView soundView = windowCanvas
                .GetComponentInChildren<SoundSliderView>(includeInactive: true);

            return new SliderMediator(soundModel, soundView);
        }, Lifetime.Scoped);

        builder.RegisterEntryPoint(resolver =>
        {
            MusicSliderModel musicModel = resolver.Resolve<MusicSliderModel>();

            Canvas windowCanvas = resolver.Resolve<Canvas>();
            MusicSliderView musicView = windowCanvas
                .GetComponentInChildren<MusicSliderView>(includeInactive: true);

            return new SliderMediator(musicModel, musicView);
        }, Lifetime.Scoped);
    }
}
