using UnityEngine;
using VContainer;
using VContainer.Unity;

public class PauseMenuSliderMediatorsLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register(resolver =>
        {
            Canvas windowCanvas = resolver.Resolve<Canvas>();
            return windowCanvas.GetComponentInChildren<SensitivitySliderView>(includeInactive: true);
        }, Lifetime.Singleton);

        builder.Register(resolver =>
        {
            Canvas windowCanvas = resolver.Resolve<Canvas>();
            return windowCanvas.GetComponentInChildren<SoundSliderView>(includeInactive: true);
        }, Lifetime.Singleton);

        builder.Register(resolver =>
        {
            Canvas windowCanvas = resolver.Resolve<Canvas>();
            return windowCanvas.GetComponentInChildren<MusicSliderView>(includeInactive: true);
        }, Lifetime.Singleton);

        builder.RegisterEntryPoint<SensitivitySliderDataKeeperMediator>(Lifetime.Singleton);
        builder.RegisterEntryPoint<SoundSliderDataKeeperMediator>(Lifetime.Singleton);
        builder.RegisterEntryPoint<MusicSliderDataKeeperMediator>(Lifetime.Singleton);
    }
}
