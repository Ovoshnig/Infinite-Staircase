using UnityEngine;
using VContainer;
using VContainer.Unity;

public class MenuSliderMediatorsLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder) => 
        builder.Register<SliderMediatorFactory>(Lifetime.Singleton);

    private void Start()
    {
        SliderMediatorFactory sliderMediatorFactory = Container.Resolve<SliderMediatorFactory>();

        Canvas canvas = FindFirstObjectByType<Canvas>();
        SliderView[] views = canvas.GetComponentsInChildren<SliderView>(true);

        foreach (SliderView view in views)
        {
            SliderModel model = view switch
            {
                SensitivitySliderView => Container.Resolve<SensitivitySliderModel>(),
                SoundSliderView => Container.Resolve<SoundSliderModel>(),
                MusicSliderView => Container.Resolve<MusicSliderModel>(),
                _ => throw new System.Exception($"Unknown slider view type: {view.GetType().Name}"),
            };
            sliderMediatorFactory.Create(model, view);
        }
    }
}
