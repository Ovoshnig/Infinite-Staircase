using UnityEngine;
using VContainer;

public class SliderMediatorFactory : MediatorViewFactory<SliderMediator, SliderModel, SliderView>
{
    public SliderMediator[] CreateForEachView(IObjectResolver container)
    {
        Canvas canvas = container.Resolve<Canvas>();
        SliderView[] views = canvas.GetComponentsInChildren<SliderView>(true);
        SliderMediator[] mediators = new SliderMediator[views.Length];

        for (int i = 0; i < views.Length; i++)
        {
            SliderView view = views[i];
            SliderModel model = view switch
            {
                SensitivitySliderView => container.Resolve<SensitivitySliderModel>(),
                SoundSliderView => container.Resolve<SoundSliderModel>(),
                MusicSliderView => container.Resolve<MusicSliderModel>(),
                _ => throw new System.Exception($"Unknown slider view type: {view.GetType().Name}"),
            };

            mediators[i] = Create(model, view);
        }

        return mediators;
    }

    protected override SliderMediator CreateMediatorInstance(SliderModel sliderModel, SliderView sliderView) =>
        new(sliderModel, sliderView);
}
