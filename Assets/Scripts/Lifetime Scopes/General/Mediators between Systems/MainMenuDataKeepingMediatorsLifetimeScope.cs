using UnityEngine;
using VContainer;
using VContainer.Unity;

public class MainMenuDataKeepingMediatorsLifetimeScope : LifetimeScope
{
    [SerializeField] private SensitivitySliderView _sensitivitySliderView;
    [SerializeField] private SoundSliderView _soundSliderView;
    [SerializeField] private MusicSliderView _musicSliderView;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponent(_sensitivitySliderView);
        builder.RegisterComponent(_soundSliderView);
        builder.RegisterComponent(_musicSliderView);

        builder.RegisterEntryPoint<SensitivitySliderDataKeeperMediator>(Lifetime.Singleton);
        builder.RegisterEntryPoint<SoundSliderDataKeeperMediator>(Lifetime.Singleton);
        builder.RegisterEntryPoint<MusicSliderDataKeeperMediator>(Lifetime.Singleton);
    }
}
