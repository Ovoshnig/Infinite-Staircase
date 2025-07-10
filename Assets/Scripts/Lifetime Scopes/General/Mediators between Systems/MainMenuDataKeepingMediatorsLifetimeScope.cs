using VContainer;
using VContainer.Unity;

public class MainMenuDataKeepingMediatorsLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        MainMenuCanvasView canvasView = FindFirstObjectByType<MainMenuCanvasView>();
        SensitivitySliderView sensitivitySliderView = canvasView
            .GetComponentInChildren<SensitivitySliderView>(true);

        if (sensitivitySliderView != null)
        {
            builder.RegisterInstance(sensitivitySliderView);
            builder.RegisterEntryPoint(resolver =>
            {
                SensitivitySliderModel sensitivityModel = resolver.Resolve<SensitivitySliderModel>();
                return new SliderMediator(sensitivityModel, sensitivitySliderView);
            }, Lifetime.Scoped);
        }

        SoundSliderView soundSliderView = canvasView
            .GetComponentInChildren<SoundSliderView>(true);

        if (soundSliderView != null)
        {
            builder.RegisterInstance(soundSliderView);
            builder.RegisterEntryPoint(resolver =>
            {
                SoundSliderModel soundVolumeModel = resolver.Resolve<SoundSliderModel>();
                return new SliderMediator(soundVolumeModel, soundSliderView);
            }, Lifetime.Scoped);
        }

        MusicSliderView musicSliderView = canvasView
            .GetComponentInChildren<MusicSliderView>(true);

        if (musicSliderView != null)
        {
            builder.RegisterInstance(musicSliderView);
            builder.RegisterEntryPoint(resolver =>
            {
                MusicSliderModel musicVolumeModel = resolver.Resolve<MusicSliderModel>();
                return new SliderMediator(musicVolumeModel, musicSliderView);
            }, Lifetime.Scoped);
        }
    }
}
