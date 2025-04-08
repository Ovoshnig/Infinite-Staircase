using VContainer;
using VContainer.Unity;

public class PauseMenuSliderMediatorsLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register(resolver =>
        {
            PauseMenuSwitch pauseMenuSwitch = resolver.Resolve<PauseMenuSwitch>();
            return pauseMenuSwitch.GetComponentInChildren<SensitivitySliderView>(includeInactive: true);
        }, Lifetime.Singleton);

        builder.Register(resolver =>
        {
            PauseMenuSwitch pauseMenuSwitch = resolver.Resolve<PauseMenuSwitch>();
            return pauseMenuSwitch.GetComponentInChildren<SoundSliderView>(includeInactive: true);
        }, Lifetime.Singleton);

        builder.Register(resolver =>
        {
            PauseMenuSwitch pauseMenuSwitch = resolver.Resolve<PauseMenuSwitch>();
            return pauseMenuSwitch.GetComponentInChildren<MusicSliderView>(includeInactive: true);
        }, Lifetime.Singleton);

        builder.RegisterEntryPoint<SensitivitySliderDataKeeperMediator>(Lifetime.Singleton);
        builder.RegisterEntryPoint<SoundSliderDataKeeperMediator>(Lifetime.Singleton);
        builder.RegisterEntryPoint<MusicSliderDataKeeperMediator>(Lifetime.Singleton);
    }
}
