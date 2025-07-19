using VContainer;
using VContainer.Unity;

public class MenuSliderLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder) => 
        builder.Register<SliderMediatorFactory>(Lifetime.Singleton);

    private void Start()
    {
        SliderMediatorFactory sliderMediatorFactory = Container.Resolve<SliderMediatorFactory>();
        sliderMediatorFactory.CreateForEachView(Container);
    }
}
