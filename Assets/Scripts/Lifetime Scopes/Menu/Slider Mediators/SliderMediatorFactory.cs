using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class SliderMediatorFactory : MediatorViewFactory<SliderMediator, SliderModel, SliderView>
{
    public override SliderMediator Create(SliderModel sliderModel, SliderView sliderView)
    {
        SliderMediator sliderMediator = new(sliderModel, sliderView);
        sliderMediator.Initialize();
        Disposables.Add(sliderMediator);
        return sliderMediator;
    }

    public override List<SliderMediator> CreateForEachView(IObjectResolver container)
    {
        Canvas canvas = container.Resolve<Canvas>();
        SliderView[] views = canvas.GetComponentsInChildren<SliderView>(true);
        List<SliderMediator> mediators = new();

        foreach (var view in views)
        {
            SliderModel model = view switch
            {
                SensitivitySliderView => container.Resolve<SensitivitySliderModel>(),
                SoundSliderView => container.Resolve<SoundSliderModel>(),
                MusicSliderView => container.Resolve<MusicSliderModel>(),
                _ => throw new System.Exception($"Unknown slider view type: {view.GetType().Name}"),
            };
            SliderMediator mediator = Create(model, view);
            mediators.Add(mediator);
        }

        return mediators;
    }
}
