public class SliderMediatorFactory : MediatorViewFactory<SliderMediator, SliderModel, SliderView>
{
    public override SliderMediator Create(SliderModel sliderModel, SliderView sliderView)
    {
        SliderMediator sliderMediator = new(sliderModel, sliderView);
        sliderMediator.Initialize();
        Disposables.Add(sliderMediator);
        return sliderMediator;
    }
}
